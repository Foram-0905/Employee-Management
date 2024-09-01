import { Injectable } from '@angular/core';
import { HttpClient ,  HttpHeaders} from '@angular/common/http';
import { Observable,first } from 'rxjs';
import { EmployeeType } from '../models/employeeType';
import { map } from 'rxjs/operators';
import { RequestWithFilterAndSort , } from '../models/FilterRequset';
import { GetEmployeeType} from '../constant/api.const';
import { JobHistory } from '../models/job-history';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root'
})

export class EmployeeTypeService {
  constructor(private http: HttpClient) { }

  GetEmployeeType(): Observable<EmployeeType[]> {
    return this.http.get<any>(GetEmployeeType);
  }
  getFilterEmployeeType(requst:RequestWithFilterAndSort){
    return this.http.post<any>(GetEmployeeType,requst)
  }

 }