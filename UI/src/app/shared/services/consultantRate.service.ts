import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { consultant_rate } from '../models/consultant_rate';
import { saveConsultantRate } from '../constant/api.const';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  @Injectable({
    providedIn: 'root'
  })
  export class consultantRateService{
    constructor(private http: HttpClient) {

    }

    saveConsultantRate(consultantRate: consultant_rate): Observable<any> {
  
        return this.http.post<any>(saveConsultantRate, consultantRate);
      }
  }