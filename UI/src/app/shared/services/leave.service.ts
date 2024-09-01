import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApprovOrRejectLeaves, GetAllLeaves, GetLeaves,GetFilterPendingLeave, GetFilterLeaveHistory, GetLeaveByDate, ApplyLeaveFromCalendar } from '../constant/api.const';
import { Observable } from 'rxjs';
import{ApprovOrRejectLeave}from'../models/employee-leave';
import { ManageLeave } from '../models/manage-leave';

@Injectable({
  providedIn: 'root'
})
export class LeaveService {

  constructor(private http: HttpClient) { }

  applyLeave(leave: ManageLeave): Observable<any> {
    return this.http.post<any>(ApplyLeaveFromCalendar, leave);
  }

  GetEmployeeLeaves(Id:any) {
    return this.http.post<any>(GetLeaves,Id);
  }

  GetEmployeePendingLeaves(Id:any) {
    return this.http.post<any>(GetFilterPendingLeave,Id);
  }

  GetEmployeeHistoryLeaves(Id:any) {
    return this.http.post<any>(GetFilterLeaveHistory,Id);
  }

  GetAllLeaves() {
    return this.http.get<any>(GetAllLeaves);
  }

  GetLeaveByDate(date:any){
    return this.http.post<any>(GetLeaveByDate ,date);

  }
  ApprovOrRejectLeave(leave:ApprovOrRejectLeave): Observable<any> {
    return this.http.post<any>(ApprovOrRejectLeaves, leave);
  }


}
