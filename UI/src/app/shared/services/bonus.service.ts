import { Injectable } from '@angular/core';
import { Observable ,first} from 'rxjs';
import { Bonus } from '../models/bonus';
import {SaveBonus,GetBonus,GetBonusById,GetFilterBonus,DeleteBonus} from '../constant/api.const'
import { HttpClient } from '@angular/common/http';
import { RequestWithFilterAndSort } from '../models/FilterRequset';


@Injectable({
  providedIn: 'root'
})
export class BonusService {

  constructor(private http:HttpClient) { }
  saveBonus(savebonus:Bonus):Observable<any>{
    return this.http.post(SaveBonus,savebonus);
  }
  getBonusById(id:string):Observable<any>{
    return this.http.get<any>(`${GetBonusById}/${id}`);
  }
  getBonus():Observable<any>{
    return this.http.get<any>(GetBonus);
  }
  deleteBonus(id:string):Observable<any>{
    return this.http.delete<any>(`${DeleteBonus}/${id}`);
  }
  getFilterBonus(request:RequestWithFilterAndSort){
    return this.http.post<any>(GetFilterBonus,request)
  }
}