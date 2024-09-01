import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PublicHoliday } from '../models/public-holiday';
import { getAllPublicHoliiday, getPublicHolidayById, savePublicHoliday, deletePublicHoliday, getFilterPublicHoliday } from '../constant/api.const';
import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root'
})
export class PublicHolidayService {
  constructor(private http: HttpClient) {

  }

  getHolidayList() {

    return this.http.get<any>(getAllPublicHoliiday);
  }
  getHolidayListById(Id: string) {
    // // debugger;
    return this.http.get<any>(`${getPublicHolidayById}/${Id}`);
  }
  savePublicHoliday(PublicHoliday: PublicHoliday): Observable<any> {

    return this.http.post<any>(savePublicHoliday, PublicHoliday);
  }
  deletePublicHoliday(Id: string) {
    // // debugger;
    return this.http.delete<any>(`${deletePublicHoliday}/${Id}`);
  }
  getFilterPublicHoliday(requst:RequestWithFilterAndSort): Observable<any> {
    return this.http.post<any>(getFilterPublicHoliday, requst);
  }
}  