import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BasketDto, CheckoutDto } from '../models/basket.model';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private baseUrl = 'http://localhost:4459/api/Basket';
  http: HttpClient = inject(HttpClient);
  addToBasket(dto: BasketDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/add-to-basket`, dto);
  }

 checkout(checkoutData: CheckoutDto): Observable<{ orderId: number }> {
    return this.http.post<{ orderId: number }>(`${this.baseUrl}/checkout`, checkoutData);
}
  GetBasketByUserIdAsync(userId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${userId}`);
  }
removeBasketItem(basketItemId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/remove-item/${basketItemId}`);
  }
  
}
