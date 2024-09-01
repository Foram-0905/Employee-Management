import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { employee } from '../models/employee';
import { GetEmployee,DeleteEmployee,SaveEmployee, GetAvailableLeave,GetFilterEmployee, GetEmployeeById,
        GetEmploymentType,GetTypeofEmployment,Gettaxclass,GetMaritalStatus,GetEmployeenStatus,GetLeaveTypeEmployee,GetDeliverymethod,getEmployeeByLeader,getEmployeeByHr } from '../constant/api.const';
import { RequestWithFilterAndSort } from '../models/FilterRequset';
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  
  find(arg0: (availLeave: { leaveName: any; }) => boolean) {
    throw new Error('Method not implemented.');
  }

  constructor(private http: HttpClient) { }

  SaveEmployee(Employee :employee):Observable<any>{
    return  this.http.post(SaveEmployee,Employee );
  }

  getEmployee(){
    return this.http.get(GetEmployee);
  }

  getEmployeeById(id:string){
    return this.http.get<any>(`${GetEmployeeById}/${id}`);
  }

  
  GetAvailableLeave(id:string){
    return this.http.get<any>(`${GetAvailableLeave}/${id}`);
  }

  getEmployeeByLeader(id:string){
    return this.http.get<any>(`${getEmployeeByLeader}/${id}`);
  }

  getEmployeeByHr(id:string){
    return this.http.get<any>(`${getEmployeeByHr}/${id}`);
  }

  DeleteEmployee(id:string){
    return this.http.delete<any>(`${DeleteEmployee}/${id}`);
  }

  getFilterEmployee(requst:RequestWithFilterAndSort){
    return this.http.post<any>(GetFilterEmployee,requst)
  }

  Gettaxclass(){
    return this.http.get(Gettaxclass);
  }

  GetTypeofEmployment(){
    return this.http.get(GetTypeofEmployment);
  }
  GetMaritalStatus(){
    return this.http.get(GetMaritalStatus);
  }
  GetEmployeenStatus(){
    return this.http.get(GetEmployeenStatus);
  }
  GetLeaveTypeEmployee(){
    return this.http.get(GetLeaveTypeEmployee);
  }
  GetDeliverymethod(){
    return this.http.get(GetDeliverymethod);
  }
  GetEmploymentType(){
    return this.http.get(GetEmploymentType);
  }

}
