import { HttpClient } from '@angular/common/http';
import { applyLeave, deleteLeave, GetFilterLeaves, getLeavesById, getLeaveType } from './../constant/api.const';
import { Injectable } from '@angular/core';
import { ManageLeave } from '../models/manage-leave';
import { Observable } from 'rxjs';
import { RequestWithFilterAndSort } from '../models/FilterRequset';

@Injectable({
  providedIn: 'root'
})
export class ManageLeaveService {

  constructor(private http: HttpClient) { }


    applyLeave(leave: ManageLeave): Observable<any> {
      return this.http.post<any>(applyLeave, leave);
    }

    getLeaves(requst:RequestWithFilterAndSort): Observable<any> {
      return this.http.post<any>(GetFilterLeaves, requst);
    }

    getLeavesById(Id: string) {
      return this.http.get<any>(`${getLeavesById}/${Id}`);
    }

    deleteLeave(id: string): Observable<any> {
      const url = `${deleteLeave}?id=${id}`;
      return this.http.delete<any>(url);
    }
    

    // deleteLeave(id: string): Observable<any> {
    //   const url = `${this.baseUrl}/DeleteLeave?id=${id}`;
    //   return this.http.delete<any>(url);
    // }

}
