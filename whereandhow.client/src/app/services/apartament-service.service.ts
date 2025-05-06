import { Apartamet } from './../Model/Apartament';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApartametRes } from '../Model/ApartamentRes';
import { OrderDTO } from '../Model/OrderDTO';

@Injectable({
  providedIn: 'root',
})
export class ApartamentService {
  private apartamentURL: string = '/api/apartment';

  constructor(private http: HttpClient) {}

  public getApartamets(): Observable<ApartametRes[]> {
    return this.http
      .get<{ aparataments: ApartametRes[] }>(
        `${this.apartamentURL}/apartaments`
      )
      .pipe(map((response) => response.aparataments));
  }

  public getSearchApartament(search: any): Observable<Apartamet[]> {
    return this.http.get<Apartamet[]>(
      `${this.apartamentURL}/searchapartament`,
      { params: search }
    );
  }

  public getApartamentById(Id: string): Observable<ApartametRes> {
    return this.http.get<ApartametRes>(`${this.apartamentURL}/room/${Id}`);
  }

  public createApartament(apartament: FormData): Observable<any> {
    return this.http.post<Apartamet>(
      `${this.apartamentURL}/addroom`,
      apartament
    );
  }

  public orderApartament(order: OrderDTO): Observable<OrderDTO> {
    return this.http.post<OrderDTO>(`${this.apartamentURL}/order`, order);
  }
}
