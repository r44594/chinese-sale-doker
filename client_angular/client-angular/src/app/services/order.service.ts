import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { BuyerOrderDto, OrderDto } from '../models/order.model';
import { MostPurchasedGiftDto } from '../models/gift.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

 private baseUrl = 'http://localhost:4459/api/Order';
 private http: HttpClient = inject(HttpClient);
 getNumberOfTickets(giftId: number): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}/GetNumberOfTickets/${giftId}`);
   }
  orderByTotalPrice(): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(`${this.baseUrl}/MaxPrice`);
  }
  getMostPurchasedGift(): Observable<MostPurchasedGiftDto[]> {
    return this.http.get<MostPurchasedGiftDto[]>(`${this.baseUrl}/maxschumgift`);
  }
  getBuyerDetails(): Observable<BuyerOrderDto[]> {
    return this.http.get<BuyerOrderDto[]>(`${this.baseUrl}/GetBuyerDetails`);
  }

} 

