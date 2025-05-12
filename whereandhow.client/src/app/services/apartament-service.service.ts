import { Apartamet } from './../Model/Apartament';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApartametRes } from '../Model/ApartamentRes';
import { OrderDTO } from '../Model/OrderDTO';
import { SearchApartament } from '../Model/SearchApartament';

@Injectable({
  providedIn: 'root',
})
export class ApartamentService {
  private apartamentURL: string = '/api/apartment';

  constructor(private http: HttpClient) {}

  public getApartamets(): Observable<ApartametRes[]> {
    return this.http.get<ApartametRes[]>(`${this.apartamentURL}/apartaments`);
  }

  public getSearchApartament(search: SearchApartament): Observable<ApartametRes[]> {
    return this.http.post<ApartametRes[]>(`${this.apartamentURL}/searchapartament`,search);
  }

  public getApartamentById(Id: string): Observable<ApartametRes> {
    return this.http.get<ApartametRes>(`${this.apartamentURL}/room/${Id}`);
  }

  public createApartament(apartament: FormData): Observable<any> {
    return this.http.post(
      `${this.apartamentURL}/addroom`,
      apartament, 
     { responseType: 'text' as const }     
    );
  }

  public orderApartament(order: OrderDTO): Observable<OrderDTO> {
    return this.http.post<OrderDTO>(`${this.apartamentURL}/order`, order);
  }
}
