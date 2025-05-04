import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Address } from '../model/address';

@Injectable({
  providedIn: 'root',
})
export class AddressService {
  private addressURL = 'Addresses';

  constructor(private http: HttpClient) {}

  public getAddress(): Observable<Address[]> {
    return this.http.get<Address[]>(this.addressURL);
  }
}
