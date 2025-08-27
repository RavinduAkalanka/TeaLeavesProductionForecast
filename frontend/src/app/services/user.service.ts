import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User, UserLogin, UserRegister, UserUpdate } from '../model/UserRegister';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private primaryUrl = environment.apiPrimaryUrl;

  constructor(private http: HttpClient) {}

  registerUser(data: UserRegister): Observable<UserRegister> {
    return this.http.post<UserRegister>(`${this.primaryUrl}/register`, data);
  }

  loginUser(data: UserLogin): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.primaryUrl}/login`, data);
  }

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.primaryUrl}/user/${id}`);
  }

  updateUser(id: number, data: UserUpdate): Observable<any> {
  return this.http.put(`${this.primaryUrl}/user/${id}`, data);
}
}
