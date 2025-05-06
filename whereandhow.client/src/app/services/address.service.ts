import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Addresses } from '../Model/Addresses';

@Injectable({
  providedIn: 'root',
})
export class AddressService {
  private addressURL = '/api/address';

  constructor(private http: HttpClient) {}

  public getAddress(): Observable<Addresses[]> {
    return this.http.get<Addresses[]>(this.addressURL);
  }
}
