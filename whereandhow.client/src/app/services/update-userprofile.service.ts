import { User } from '../Model/user';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class UpdateUserprofileService {
  private baseURL = '/api/user';
  constructor(private http: HttpClient, private authService: AuthService) {}

  private getLoggedInUserId() {
    return this.authService.getUserId();
  }

  public checkIsLosser(): boolean {
    return this.authService.isLosser();
  }

  public getUser(): Observable<User> {
    return this.http.get<User>(`${this.baseURL}/${this.getLoggedInUserId()}`);
  }

  public getUserById(Id: string): Observable<User> {
    return this.http.get<User>(`${this.baseURL}/${Id}`);
  }

  update(user: User): void {
    this.http
      .put<{ token: string }>(`${this.baseURL}/update/${this.getLoggedInUserId()}`, user)
      .subscribe({
        next: (response) => {
          this.authService.setToken(response.token);
        },
        error: (error: unknown) => {
          console.error(error);
        },
      });
  }
}
