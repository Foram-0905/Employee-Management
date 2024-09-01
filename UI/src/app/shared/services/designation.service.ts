import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { deleteDesignation, GetAllDesignation, getDesignationById, GetFilterDesignation, SaveDesignation } from '../constant/api.const';
import { ManageDesignation, Savedesignation } from '../models/manage-designation';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};
@Injectable({
  providedIn: 'root'
})
export class DesignationService {

  constructor(private http: HttpClient) {

  }

  saveDesignation(designation: Savedesignation): Observable<any> {
    return this.http.post<any>(SaveDesignation, designation);
  }

  GetAllDesignation() {

    return this.http.get<any>(GetAllDesignation);
  }

  getDesignationById(Id: string) {
    return this.http.get<any>(`${getDesignationById}/${Id}`);
  }

  deleteDesignation(Id: string) {

    return this.http.delete<any>(`${deleteDesignation}/${Id}`);
  }

  GetFilterDesignation(requst:RequestWithFilterAndSort): Observable<any> {
    return this.http.post<any>(GetFilterDesignation, requst);
  }

}
