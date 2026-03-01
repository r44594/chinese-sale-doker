import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { GetUsers, LoginDto, LoginResponseDto, RegisterDto } from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'http://localhost:4459/api/User';
  private http: HttpClient = inject(HttpClient);

  
  register(dto: RegisterDto): Observable<GetUsers> {
  return this.http.post<GetUsers>(`${this.baseUrl}/register`, dto);
  }

  login(credentials: LoginDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.baseUrl}/login`, credentials).pipe(
      tap(response => {
        if (response?.token) {
          this.saveUserData(response);
        }
      })
    );
  }
   saveUserData(data: LoginResponseDto): void {
    localStorage.setItem('token', data.token);
    localStorage.setItem('tokenType', data.tokenType);
    localStorage.setItem('userName', data.user.userName);
  const roleValue = data.user.role === 'admin' ? 'Admin' : data.user.role;
  localStorage.setItem('role', roleValue);
    localStorage.setItem('userId', data.user.id.toString());
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
  getUserName(): string | null {
    return localStorage.getItem('userName');
  }
  getUserRole(): string | null {
    return localStorage.getItem('role');
  }
  isLoggedIn(): boolean {
    return !!this.getToken();
  }
  logout() {
    localStorage.clear();
    window.location.reload();
  }
}
