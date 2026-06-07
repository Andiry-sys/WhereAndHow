import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AIAnalyzeRequest } from '../Model/aiAnalyzeRequest';
import { AIAnalyzeResponse } from '../Model/aiAnalyzeResponse';

@Injectable({
  providedIn: 'root',
})
export class AiService {
  private readonly baseUrl = '/api/ai';

  constructor(private http: HttpClient) {}

  public analyzeDescription(
    description: string,
    metadata?: {
      name?: string;
      price?: number;
      typeRoom?: string;
      city?: string;
      street?: string;
    }
  ): Observable<AIAnalyzeResponse> {
    const body: AIAnalyzeRequest = {
      description,
      language: this.detectLanguage(description),
      ...metadata,
    };
    return this.http.post<AIAnalyzeResponse>(`${this.baseUrl}/analyze`, body);
  }

  /**
   * Detects whether the text is Ukrainian or English by comparing the count of
   * Ukrainian Cyrillic characters (U+0400–U+04FF) against Latin alphabetic
   * characters. Returns "Ukrainian" when Cyrillic dominates; "English" otherwise.
   * Falls back to "English" for empty or very short input.
   */
  private detectLanguage(text: string): 'Ukrainian' | 'English' {
    const sample = text.trim().slice(0, 200);
    if (sample.length < 5) return 'English';

    const cyrillicCount = (sample.match(/[Ѐ-ӿ]/g) ?? []).length;
    const latinCount    = (sample.match(/[A-Za-z]/g)        ?? []).length;

    return cyrillicCount > latinCount ? 'Ukrainian' : 'English';
  }
}
