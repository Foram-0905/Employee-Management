import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { deleteLeaveType, getFilterLeaveType, GetLeaveCategory, getLeaveType, getLeaveTypeByID, saveLeaveType } from '../constant/api.const';
import { LeaveType } from '../models/leave-type';
import { RequestWithFilterAndSort } from '../models/FilterRequset';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};
@Injectable({
  providedIn: 'root'
})
export class leaveTypeService {
  constructor(private http: HttpClient) {

  }
  getLeaves() {

    return this.http.get<any>(getLeaveType);
  }
  getLeaveCategory() {
    return this.http.get<any>(GetLeaveCategory);

  }
  getLeavesById(Id: string) {
    // // debugger;
    return this.http.get<any>(`${getLeaveTypeByID}/${Id}`);
  }
  saveLeaveType(LeaveType: LeaveType): Observable<any> {

    return this.http.post<any>(saveLeaveType, LeaveType);
  }
  deleteLeaveType(Id: string) {
    // // debugger;
    return this.http.delete<any>(`${deleteLeaveType}/${Id}`);
  }
  getFilterLeaveType(requst:RequestWithFilterAndSort): Observable<any> {
    return this.http.post<any>(getFilterLeaveType, requst);
  }

}