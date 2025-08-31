import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Prediction } from '../model/Prediction';

@Injectable({
  providedIn: 'root'
})
export class PredictionService {
  private primaryUrl = environment.apiPrimaryUrl;

  constructor(private http: HttpClient) { }

  prediction(data: Prediction): Observable<any> {
    return this.http.post(`${this.primaryUrl}/predict`, data);
  }
}
