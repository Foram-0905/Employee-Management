import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getSalaryType } from '../constant/api.const';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  @Injectable({
    providedIn: 'root'
  })
  export class SalaryTypeService {
    constructor(private http: HttpClient) {

    }

    GetAllSalaryType() {
  
        return this.http.get<any>(getSalaryType);
      }
}