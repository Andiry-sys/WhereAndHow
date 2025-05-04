import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseURL: string = 'Authenticate/';
  constructor(private http: HttpClient, private route: Router) {}

  register(user: any) {
    return this.http
      .post<any>(`${this.baseURL}register`, user)
      .subscribe((res) => {
        this.setToken(res.token);
      });
  }

   login(user: any){
    this.http.post<any>(`${this.baseURL}login`, user).subscribe(
      (response: any) => {
        const token = response.token;
        this.setToken(token);
      },
      (error: any) => {
        console.error(error);

      }

    );
  }
  isUserAuthenticated(): boolean {
    const accessToken = localStorage.getItem('token');
    if(accessToken != null){
      return true;
    }

    return false;

  }

  logout() {
    this.deleteToken();
    this.route.navigate(['login']);
  }
  setToken(token: string) {
    localStorage.setItem('token', token);
  }

  deleteToken() {
    localStorage.removeItem('token');
  }
}
