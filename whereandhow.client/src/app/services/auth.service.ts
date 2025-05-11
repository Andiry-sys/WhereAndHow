import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { UserLoginRequestDto } from '../../app/Model/userLoginRequestDto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseURL: string = '/api/authenticate/';
  private tokenKey = 'token';

  constructor(private http: HttpClient, private router: Router) { }

  register(user: any): Observable<any> {
    return this.http.post<any>(`${this.baseURL}register`, user).pipe(
      tap((res) => this.setToken(res.token))
    );
  }

  login(user: UserLoginRequestDto): Observable<any> {
    return this.http.post<any>(`${this.baseURL}login`, user).pipe(
      tap((res) => this.setToken(res.token))
    );
  }

  logout(): void {
    this.deleteToken();
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const payload = this.decodeToken(token);
      const exp = payload?.exp;
      if (!exp) return false;

      const isExpired = Date.now() >= exp * 1000;
      return !isExpired;
    } catch {
      return false;
    }
  }

  setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  deleteToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  getUserId(): string | null {
    const token = this.getToken();
    if (!token) return null;

    const payload = this.decodeToken(token);
    return payload?.[
      'nameid'
    ] ?? null;
  }

  isLosser(): boolean {
    const token = this.getToken();
    if (!token) return false;

    const payload = this.decodeToken(token);
    return payload?.IsLosser === 'True';
  }

  private decodeToken(token: string): any {
    try {
      const payloadBase64 = token.split('.')[1];
      const decodedPayload = atob(payloadBase64);
      return JSON.parse(decodedPayload);
    } catch {
      return null;
    }
  }
}
