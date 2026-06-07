using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Interfaces;
using Core.Domain.DTOs;
using Infrastructure.Service.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Service.Services;

public class AIService(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<AIService> logger
) : IAIService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<AIService> _logger = logger;

    private static readonly JsonSerializerOptions ParseOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // Matches control characters except tab (0x09) and newline (0x0A / 0x0D).
    private static readonly Regex ControlChars = new(@"[\x00-\x08\x0B\x0C\x0E-\x1F\x7F]", RegexOptions.Compiled);

    // Collapses runs of 3+ newlines to a single blank line.
    private static readonly Regex ExcessiveNewlines = new(@"\n{3,}", RegexOptions.Compiled);

    public async Task<AIAnalyzeResponseDTO> AnalyzeDescriptionAsync(AIAnalyzeRequestDTO request)
    {
        var model = _configuration["IccUsa:Model"]
            ?? throw new AIServiceException("IccUsa:Model is not configured.");

        var token = _configuration["IccUsa:Token"]
            ?? Environment.GetEnvironmentVariable("ICCUSA_API_KEY")
            ?? throw new AIServiceException("ICC-USA API token is not configured.");

        var language = NormalizeLanguage(request.Language);
        var systemPrompt = BuildSystemPrompt(language);
        var userMessage = BuildUserMessage(request);

        var requestBody = new
        {
            model,
            temperature = 0.4,
            response_format = new { type = "json_object" },
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user",   content = userMessage  }
            }
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
        {
            Content = JsonContent.Create(requestBody)
        };
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(httpRequest);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "AI provider request failed.");
            throw new AIServiceException("AI provider request failed.", ex);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "AI provider request timed out.");
            throw new AIServiceException("AI provider request timed out.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            _logger.LogError("AI provider returned {StatusCode}: {Body}", response.StatusCode, errorBody);
            throw new AIServiceException($"AI provider returned {(int)response.StatusCode}.");
        }

        ChatCompletionResponse? envelope;
        try
        {
            envelope = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>(ParseOptions);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI provider envelope.");
            throw new AIServiceException("Failed to parse AI provider response.", ex);
        }

        var content = envelope?.Choices?.FirstOrDefault()?.Message?.Content;
        if (string.IsNullOrWhiteSpace(content))
        {
            _logger.LogError("AI provider returned an empty message.");
            throw new AIServiceException("AI provider returned an empty message.");
        }

        var json = StripJsonFences(content);

        try
        {
            var result = JsonSerializer.Deserialize<AIAnalyzeResponseDTO>(json, ParseOptions);
            if (result is null)
                throw new AIServiceException("AI provider returned an unparseable analysis.");

            return result;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI analysis content: {Content}", content);
            throw new AIServiceException("Failed to parse AI analysis content.", ex);
        }
    }

    // ── Prompt builders ───────────────────────────────────────────────────────

    private static string NormalizeLanguage(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return "English";

        return raw.Trim().ToLowerInvariant() switch
        {
            "ukrainian" or "uk" or "ukrainska" or "українська" => "Ukrainian",
            "english"   or "en"                                => "English",
            _                                                  => "English",
        };
    }

    private static string BuildSystemPrompt(string language) =>
        $$"""
        You are a professional rental listing writer specialising in short-term apartment rentals.
        Your task is to rewrite the apartment description provided by the user.

        RULES:
        - Write exclusively in {{language}}. Every field of the JSON response must be in {{language}}.
        - Use only the facts provided. Never invent features, amenities, or locations.
        - Write in a warm, professional tone suitable for a rental platform.
        - Keep the improved description between 80 and 200 words.
        - Identify amenities explicitly mentioned in the description.
        - Provide 2 to 4 concrete, actionable recommendations to improve the listing.
        - Score description quality from 0 to 100 based on completeness, clarity, and appeal.

        Respond with STRICT JSON ONLY — no markdown, no code fences, no commentary — matching exactly:
        { "improvedDescription": string, "amenities": string[], "qualityScore": number, "recommendations": string[] }
        """;

    private static string BuildUserMessage(AIAnalyzeRequestDTO req)
    {
        var sb = new StringBuilder();
        sb.AppendLine("APARTMENT DATA:");
        sb.AppendLine($"Name: {Sanitize(req.Name)}");
        sb.AppendLine($"Type: {Sanitize(req.TypeRoom)}");
        sb.AppendLine($"Price per night: {(req.Price.HasValue ? $"{req.Price.Value} UAH" : "not provided")}");
        sb.AppendLine($"City: {Sanitize(req.City)}");
        sb.AppendLine($"Street: {Sanitize(req.Street)}");
        sb.AppendLine();
        sb.AppendLine("ORIGINAL DESCRIPTION:");
        sb.Append(Sanitize(req.Description));
        return sb.ToString();
    }

    // Removes control characters and collapses excessive whitespace.
    // Input is placed in the user message only — never in the system prompt.
    private static string Sanitize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "not provided";

        var s = ControlChars.Replace(value.Trim(), string.Empty);
        s = ExcessiveNewlines.Replace(s, "\n\n");
        return s;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string StripJsonFences(string content)
    {
        var trimmed = content.Trim();
        if (!trimmed.StartsWith("```"))
            return trimmed;

        var firstNewline = trimmed.IndexOf('\n');
        if (firstNewline >= 0)
            trimmed = trimmed[(firstNewline + 1)..];

        if (trimmed.EndsWith("```"))
            trimmed = trimmed[..^3];

        return trimmed.Trim();
    }

    private sealed class ChatCompletionResponse
    {
        public List<ChatChoice>? Choices { get; set; }
    }

    private sealed class ChatChoice
    {
        public ChatMessage? Message { get; set; }
    }

    private sealed class ChatMessage
    {
        public string? Role    { get; set; }
        public string? Content { get; set; }
    }
}
