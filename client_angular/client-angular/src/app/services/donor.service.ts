import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { GetDonor, CreateUpdateDonor } from '../models/donor.model';


type Donor = GetDonor;
type DonorDto = CreateUpdateDonor;
@Injectable({
  providedIn: 'root'
})
export class DonorService {

private baseUrl = 'http://localhost:4459/api/Donor';
  private http = inject(HttpClient);

  getAllDonors(): Observable<GetDonor[]> {
    return this.http.get<GetDonor[]>(this.baseUrl);
  }
  getDonorById(id: number): Observable<GetDonor> {
    return this.http.get<GetDonor>(`${this.baseUrl}/${id}`);
  }
 getDonorByName(name: string): Observable<GetDonor[]> {
  return this.http.get<GetDonor>(`${this.baseUrl}/by-name?donorName=${name}`).pipe(
    map(res => Array.isArray(res) ? res : [res]) 
  );
}
  getDonorsByEmail(email: string): Observable<GetDonor[]> {
    return this.http.get<GetDonor[]>(`${this.baseUrl}/by-email?email=${email}`);
  }
  getDonorsByGiftName(giftName: string): Observable<GetDonor[]> {
    return this.http.get<GetDonor[]>(`${this.baseUrl}/by-gift?giftName=${giftName}`);
  }
  addDonor(dto: CreateUpdateDonor): Observable<any> {
    return this.http.post(`${this.baseUrl}`, dto, { responseType: 'text' });
  }
  updateDonor(id: number, dto: CreateUpdateDonor): Observable<GetDonor> {
    return this.http.put<GetDonor>(`${this.baseUrl}/${id}`, dto);
  }
  deleteDonor(id: number): Observable<string> {
    return this.http.delete(`${this.baseUrl}/${id}`, { responseType: 'text' });
  }
}

