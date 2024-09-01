import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LanguageCompetence } from '../models/language-competence';
import {SaveLanguageCompetence,DeleteLanguageCompetence,GetLanguageCompetence,GetLanguageCompetenceByEmployeeId} from '../constant/api.const'


@Injectable({
  providedIn: 'root'
})
export class LanguageCompetenceService {

  constructor(private http:HttpClient) { }
  // SaveLanguageCompetence(LanguageCompetence:LanguageCompetence):Observable<any>{
  //   return  this.http.post(SaveLanguageCompetence,LanguageCompetence);
  // }

  // GetAllLanguageCompetence(){
  //   return this.http.get(GetLanguageCompetence);
  // }
  // DeleteLanguageCompetence(id:string){
  //   return this.http.delete<any>(`${DeleteLanguageCompetence}/${id}`);
  // }
  // GetLanguageCompetenceByEmployeeid(EmployeeId:string){
  //   return this.http.get<any>(`${GetLanguageCompetenceByEmployeeId}/${EmployeeId}`);
  // }

}
