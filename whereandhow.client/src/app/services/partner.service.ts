import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PartnerService {
  private readonly baseUrl = '/api/partner';

  constructor(private http: HttpClient) {}

  requestPartner(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/request`, {});
  }
}
