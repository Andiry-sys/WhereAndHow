import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { UserLoginRequestDto } from '../Model/userLoginRequestDto';
import { UserRegisterRequestDto } from '../Model/userRegisterRequestDto';

interface JwtPayload {
  exp?: number;
  nameid?: string;
  IsLosser?: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseURL: string = '/api/authenticate/';
  private tokenKey = 'token';

  private _isPartner$ = new BehaviorSubject<boolean>(this.isLosser());
  /** Emits true whenever the stored JWT indicates the user is a partner. */
  public isPartner$ = this._isPartner$.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  register(user: UserRegisterRequestDto): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.baseURL}register`, user).pipe(
      tap((res) => this.setToken(res.token))
    );
  }

  login(user: UserLoginRequestDto): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.baseURL}login`, user).pipe(
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
    this._isPartner$.next(this.isLosser());
  }

  /** Re-issues a fresh JWT from the backend and stores it, updating all subscribers. */
  refreshToken(): Observable<void> {
    return this.http.get<{ token: string }>(`${this.baseURL}refresh`).pipe(
      tap((res) => this.setToken(res.token)),
      map(() => void 0)
    );
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
    return payload?.nameid ?? null;
  }

  isLosser(): boolean {
    const token = this.getToken();
    if (!token) return false;

    const payload = this.decodeToken(token);
    return payload?.IsLosser === 'True';
  }

  private decodeToken(token: string): JwtPayload | null {
    try {
      const payloadBase64 = token.split('.')[1];
      const decodedPayload = atob(payloadBase64);
      return JSON.parse(decodedPayload) as JwtPayload;
    } catch {
      return null;
    }
  }
}
