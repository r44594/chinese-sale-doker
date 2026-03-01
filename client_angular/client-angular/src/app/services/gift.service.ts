import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AfterRandom, CategoryDto, Get } from '../models/gift.model';
import {CreateUpdate} from "../models/gift.model"

type Gift = Get;
type GiftCreateUpdate = CreateUpdate;
@Injectable({
  providedIn: 'root'
})
export class GiftService {
 private baseUrl = 'http://localhost:4459/api/Gift';
  http: HttpClient = inject(HttpClient);

 getAllGifts(): Observable<Get[]> {
  return this.http.get<Get[]>(this.baseUrl);
}

getGiftById(id: number): Observable<Get> {
  return this.http.get<Get>(`${this.baseUrl}/${id}`);
}

addGift(dto: CreateUpdate) : Observable<Get> {  
  return this.http.post<Get>(`${this.baseUrl}`, dto);
}

 updateGift(id: number, dto: CreateUpdate): Observable<Get> {
    return this.http.put<Get>(`${this.baseUrl}/${id}`, dto);
  }
  
 deleteGift(id: number): Observable<string> {
    return this.http.delete(`${this.baseUrl}/${id}`, { responseType: 'text' });
  }
   getDonorByGiftId(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}/donor`);
  }
searchGiftsByName(giftName: string): Observable<Get[]> {
    return this.http.get<Get[]>(
      `${this.baseUrl}/search-by-name?giftName=${giftName}`
    );
}
searchByDonor(donorName: string): Observable<Get[]> {
    return this.http.get<Get[]>(
      `${this.baseUrl}/search-by-donor?donorName=${donorName}`
    );
  }
    getGiftsByCategory(categoryName: string): Observable<Get[]> {
    return this.http.get<Get[]>(
      `${this.baseUrl}/by-category?categoryName=${categoryName}`
    );
  }

GetOrderedByPrice(): Observable<Get[]> {
    return this.http.get<Get[]>(
      `${this.baseUrl}/ordered-by-price`
    );
}
 getAllGiftsAfterRandom(): Observable<AfterRandom[]> {
  return this.http.get<AfterRandom[]>(`${this.baseUrl}/after-random`);
}
  GetByBuyersCount(buyerCount: number): Observable<Get[]> {
  return this.http.get<Get[]>(
    `${this.baseUrl}/by-buyers-count?buyerCount=${buyerCount}`
  );
}
getAllCategories(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(`${this.baseUrl}/get-all-category`);
  }

}





