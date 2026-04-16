import { OrderDTO } from './../Model/orderDTO';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private baseURL = '/api/order';

  constructor(private http: HttpClient) {}

  public sendOrder(order: OrderDTO):Observable<OrderDTO> {
    return this.http.post<OrderDTO>(`${this.baseURL}/send-order-email`, order);
  }


}
