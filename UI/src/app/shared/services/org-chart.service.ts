import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GetEmployeeforChart } from '../constant/api.const';

@Injectable({
  providedIn: 'root'
})
export class OrgChartService {

  constructor(private http: HttpClient) { }

  getEmployee(){
    return this.http.get(GetEmployeeforChart);
  }

}
