import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { GetAllPrediction } from '../model/Prediction';
import { Observable } from 'rxjs';
import { User } from '../model/UserRegister';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private primaryUrl = environment.apiPrimaryUrl;

  constructor(private http: HttpClient) {}

  getAllPredictionById(id: number): Observable<GetAllPrediction[]> {
    return this.http.get<GetAllPrediction[]>(
      `${this.primaryUrl}/all-predictions/${id}`
    );
  }

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.primaryUrl}/dashboard-user/${id}`);
  }
}
