import { Injectable } from '@angular/core';
import { HttpClient ,  HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { EmployeeType } from '../models/employeeType';
import { City, SaveCity } from '../models/city';
import { RequestWithFilterAndSort , } from '../models/FilterRequset';
import { getCountry,GetCity,GetAllState,GetEmployeeType, GetJobHistory , GetJobHistoryByEmployeeId,SaveJobHistory, GetJobHistoryByid, DeleteJobHistory,GetFilterJobHistory} from '../constant/api.const';
import { JobHistory } from '../models/job-history';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root'
})

export class JobHistoryService {
  constructor(private http: HttpClient) { }

  GetJobHistory(): Observable<JobHistory[]> {
    return this.http.get<any>(GetJobHistory);
  }
  GetJobHistoryByid(Id:string) {
    return this.http.get<any>(`${GetJobHistoryByid}/${Id}`);
  }
  SaveJobHistory(jobhistory: JobHistory): Observable<any> {
    return this.http.post(SaveJobHistory, jobhistory);
  }
  DeleteJobHistory(Id: string){

    return this.http.delete<any>(`${DeleteJobHistory}/${Id}`);
  }

  GetJobHistoryByEmployeeId(Id: string) {
    return this.http.get<any>(`${GetJobHistoryByEmployeeId}/${Id}`);
  }
  
  GetFilterJobHistory(request:RequestWithFilterAndSort): Observable<any> {
    return this.http.post<any>(GetFilterJobHistory, request);
  }
  GetEmployeeType(): Observable<EmployeeType[]> {
    return this.http.get<any>(GetEmployeeType);
  }


 }


