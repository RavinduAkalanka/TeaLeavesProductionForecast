import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserRegister } from '../model/UserRegister';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

    private primaryUrl = environment.apiPrimaryUrl;

  constructor(private http: HttpClient) {}

  registerUser(data: UserRegister): Observable<UserRegister> {
  return this.http.post<UserRegister>(`${this.primaryUrl}/register`, data);
  }
}
