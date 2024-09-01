import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ChangeDetectionStrategy, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LeaveCategory } from '../models/leave-category';
import { GetLeaveCategory,GetLeaveCategoryById,DeleteLeaveCategory,SaveLeaveCategory, getFilterLeaveCategory } from '../constant/api.const';
import { RequestWithFilterAndSort } from '../models/FilterRequset';
@Injectable({
    providedIn: 'root'
})
export class LeaveCategoryService {
    constructor(private http: HttpClient) { }
  
    getLeaveCategory(): Observable<LeaveCategory[]> {
      return this.http.get<any>(GetLeaveCategory);
    }
    getLeaveCategoryById(Id:string) {
      return this.http.get<any>(`${GetLeaveCategoryById}/${Id}`);
    }
    saveleaveCategory(leaveCategory: LeaveCategory): Observable<any> {
      return this.http.post(SaveLeaveCategory, leaveCategory);
    }
    deleteLeaveCategory(Id: string){
      return this.http.delete<any>(`${DeleteLeaveCategory}/${Id}`);
    }
    GetFilterLeaveCategory(requst:RequestWithFilterAndSort): Observable<any> {
      return this.http.post<any>(getFilterLeaveCategory, requst);
    }
}  
