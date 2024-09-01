import { Injectable } from '@angular/core';
import { Observable ,first} from 'rxjs';
import { Education } from '../models/education';
import {SaveEducation,GetEducationById,DeleteEducation,GetEducation,GetFilterEducation,GetEducationByEmployee} from '../constant/api.const'
import { HttpClient } from '@angular/common/http';
import { RequestWithFilterAndSort } from '../models/FilterRequset';


@Injectable({
  providedIn: 'root'
})
export class EducationService {

  constructor(private http:HttpClient) { }
  saveEducation(education:Education):Observable<any>{
    return this.http.post(SaveEducation,education);
  }
  getEducationById(id:string):Observable<any>{
    return this.http.get<any>(`${GetEducationById}/${id}`);
  }
  getEducation():Observable<any>{
    return this.http.get<any>(GetEducation);
  }
  deleteEducation(id:string):Observable<any>{
    return this.http.delete<any>(`${DeleteEducation}/${id}`);
  }
  getFilterEducation(request:RequestWithFilterAndSort){
    return this.http.post<any>(GetFilterEducation,request)
  }
  getEducationbyEmployee(id:string):Observable<any>{
    return this.http.get<any>(`${GetEducationByEmployee}/${id}`);
  }
}