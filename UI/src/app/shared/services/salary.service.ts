import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Salary } from '../models/Salary';
import { deleteSalary, getFilterSalary, getLastMonth, getSalary, getSalaryByEmployee, getSalaryById, getTransaction, getTwoMonthsAgo, saveSalary } from '../constant/api.const';


const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  @Injectable({
    providedIn: 'root'
  })
  export class SalaryService {
    constructor(private http: HttpClient) {

    }
  
    saveSalary(salary: Salary): Observable<any> {
  
      return this.http.post<any>(saveSalary, salary);
    }
  
    GetAllSalary() {
  
      return this.http.get<any>(getSalary);
    }
  
    getSalaryById(Id: string) {
      return this.http.get<any>(`${getSalaryById}/${Id}`);
    }
    getSalaryByEmployee(Id: string) {
      return this.http.get<any>(`${getSalaryByEmployee}?id=${Id}`);
    }

    deleteSalary(Id: string) {
  
      return this.http.delete<any>(`${deleteSalary}/${Id}`);
    }

    GetFilterSalary(requst:RequestWithFilterAndSort): Observable<any> {
      return this.http.post<any>(getFilterSalary, requst);
    }
    
    GetLastMonthSalary(Id:string) {
  
      return this.http.get<any>(`${getLastMonth}?id=${Id}`);
    }
  
    GetTwoMonthsAgoSalary(Id:string) {
  
      return this.http.get<any>(`${getTwoMonthsAgo}?id=${Id}`);
    }
    getTransaction(){
      return this.http.get<any>(getTransaction);

    }
  }