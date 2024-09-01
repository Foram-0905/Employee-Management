import { Injectable } from '@angular/core';
import { Observable ,first} from 'rxjs';
import { SalaryType } from '../models/salarytype';
import {GetSalaryType} from '../constant/api.const'
import { HttpClient } from '@angular/common/http';
import { RequestWithFilterAndSort } from '../models/FilterRequset';


@Injectable({
  providedIn: 'root'
})
export class SalaryTypeService {

  constructor(private http:HttpClient) { }

  getSalaryType():Observable<any>{
    return this.http.get<any>(GetSalaryType);
  }

}