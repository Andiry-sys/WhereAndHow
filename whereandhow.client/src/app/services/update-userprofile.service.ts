import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root',
})
export class UpdateUserprofileService {
  private baseURL = 'User';
  constructor(private http: HttpClient) {}

  private getLoggedInUserId() {
    let token = localStorage.getItem('token');
    if (token != null) {
      const startIndex = token.lastIndexOf(',"exp"');
      const endIndex = token.lastIndexOf('}');
      token =
        token.substring(0, startIndex) + token.substring(endIndex + 1) + '}';
      let sim = token.indexOf('.');
      token = token.substring(sim + 1);

      const json = JSON.parse(token);

      const Id =
        json[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ];

      return Id;
    }
    return '';
  }

  public checkIsLosser(): boolean {
    let token = localStorage.getItem('token');
    let IsLosser = false;
    if (token != null) {
      const startIndex = token.lastIndexOf(',"exp"');
      const endIndex = token.lastIndexOf('}');
      token =
        token.substring(0, startIndex) + token.substring(endIndex + 1) + '}';
      let sim = token.indexOf('.');
      token = token.substring(sim + 1);

      const json = JSON.parse(token);
     
      if(json['IsLosser'] == 'True'){
       IsLosser =  true;
      }
            
     
    }    
    return IsLosser; 

   
  }

  public getUser(): Observable<User> {
    return this.http.get<User>(`${this.baseURL}/${this.getLoggedInUserId()}`);
  }

  public getUserById(Id: string): Observable<User> {
    return this.http.get<User>(`${this.baseURL}/${Id}`);
  }

  update(user: any) {
    return this.http
      .put<any>(`${this.baseURL}/update/${this.getLoggedInUserId()}`, user)
      .subscribe(
        (response: any) => {
          const token = response.token;
          this.setToken(token);
        },
        (error: any) => {
          console.error(error);
        }
      );
  }
  setToken(token: string) {
    localStorage.setItem('token', token);
  }
}
