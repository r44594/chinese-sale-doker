import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { RandomDto, RandomIncomeDto } from '../models/random.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RandomService {
  private baseUrl = 'http://localhost:4459/api/Random';
   private http: HttpClient = inject(HttpClient);
   DrawLottery(): Observable<RandomDto[]> {
     return this.http.get<RandomDto[]>(`${this.baseUrl}/Random`);
   }
   GetWinnersReport():Observable<RandomDto[]>{
  return this.http.get<RandomDto[]>(`${this.baseUrl}/winner_of_gift`);
   }
  GetTotalIncome():Observable<RandomIncomeDto>{
     return this.http.get<RandomIncomeDto>(`${this.baseUrl}/zover_all_buy`);
  }
}
