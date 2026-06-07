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

  public analyzeDescription(description: string): Observable<AIAnalyzeResponse> {
    const body: AIAnalyzeRequest = { description };
    return this.http.post<AIAnalyzeResponse>(`${this.baseUrl}/analyze`, body);
  }
}
