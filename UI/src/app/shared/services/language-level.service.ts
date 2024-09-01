import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import {LanguageLevel} from '../models/language-level'
import {SavelanguageLevel,GetAllLanguageLevel,DeleteLanguageLevel,GetLanguageLevelById,getFilterLanguageLevel} from '../constant/api.const'
import { RequestWithFilterAndSort } from '../models/FilterRequset';

@Injectable({
  providedIn: 'root'
})
export class LanguageLevelService {

  constructor(private http:HttpClient) { }

  SaveLanguagelevel(Languagelevel:LanguageLevel):Observable<any>{
    return  this.http.post(SavelanguageLevel,Languagelevel);
  }

  GetAllLanguagelevel(){
    return this.http.get(GetAllLanguageLevel);
  }
  DeleteLanguagelevel(id:string){
    return this.http.delete<any>(`${DeleteLanguageLevel}/${id}`);
  }
  GetLanguagelevelById(id:string){
    return this.http.get<any>(`${GetLanguageLevelById}/${id}`);
  }
 
  getFiltersLanguagelevel(requst:RequestWithFilterAndSort){
    return this.http.post<any>(getFilterLanguageLevel,requst)
  }

}
