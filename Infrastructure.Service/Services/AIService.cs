using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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

    private const string SystemPrompt =
        "You are a professional real-estate copywriter. Improve the apartment description you are given. " +
        "Respond with STRICT JSON ONLY (no markdown, no code fences, no commentary) matching exactly this schema: " +
        "{ \"improvedDescription\": string, \"amenities\": string[], \"qualityScore\": number (0-100), \"recommendations\": string[] }.";

    private static readonly JsonSerializerOptions ParseOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<AIAnalyzeResponseDTO> AnalyzeDescriptionAsync(string description)
    {
        var model = _configuration["IccUsa:Model"]
            ?? throw new AIServiceException("IccUsa:Model is not configured.");

        var token = _configuration["IccUsa:Token"]
            ?? Environment.GetEnvironmentVariable("ICCUSA_API_KEY")
            ?? throw new AIServiceException("ICC-USA API token is not configured.");

        var requestBody = new
        {
            model,
            temperature = 0.4,
            response_format = new { type = "json_object" },
            messages = new[]
            {
                new { role = "system", content = SystemPrompt },
                new { role = "user", content = description }
            }

        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
        {
            Content = JsonContent.Create(requestBody)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request);
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

    private static string StripJsonFences(string content)
    {
        var trimmed = content.Trim();
        if (!trimmed.StartsWith("```"))
            return trimmed;

        // Remove a leading ```json or ``` fence and the trailing ``` fence.
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
        public string? Role { get; set; }
        public string? Content { get; set; }
    }
}
