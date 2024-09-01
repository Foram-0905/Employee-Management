import { Injectable } from '@angular/core';
import { Observable ,first} from 'rxjs';
import { EducationLevel } from '../models/education-level'; 
import {SaveEducationlevel,GetEducationLevelById,DeleteEducationLevel,GetAllEducationLevel,getFilterEducationLevel} from '../constant/api.const'
import { HttpClient } from '@angular/common/http';
import { RequestWithFilterAndSort } from '../models/FilterRequset';


@Injectable({
  providedIn: 'root'
})
export class EducationLevelService {

  constructor(private http:HttpClient) { }
  saveEducationLevel(educationLevel:EducationLevel):Observable<any>{
    return this.http.post(SaveEducationlevel,educationLevel);
  }
  getEducationLevelById(id:string):Observable<any>{
    return this.http.get<any>(`${GetEducationLevelById}/${id}`);
  }
  getAllEducationLevel():Observable<any>{
    return this.http.get<any>(GetAllEducationLevel);
  }
  deleteEducationLevel(id:string):Observable<any>{
    return this.http.delete<any>(`${DeleteEducationLevel}/${id}`);
  }
  getFilterEducationLevel(requst:RequestWithFilterAndSort){
    return this.http.post<any>(getFilterEducationLevel,requst)
  }
}
