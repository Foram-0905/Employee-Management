// import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
// import { HttpClient, HttpHeaders } from '@angular/common/http';
// import { Injectable } from '@angular/core';
// import { Observable } from 'rxjs';
// import { WorkPermit } from '../models/work-permit';
// import { deleteWorkPermit, getWorkPermit, getWorkPermitById, saveWorkPermit } from '../constant/api.const';

// const httpOptions = {
//     headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
// };
// @Injectable({
//     providedIn: 'root'
// })
// export class WorkPermitService {

//     constructor(private http: HttpClient) {

//     }

//     saveWorkPermit(workPermit: WorkPermit): Observable<any> {

//         return this.http.post<any>(saveWorkPermit, workPermit);
//     }

//     getAllWorkPermit() {

//         return this.http.get<any>(getWorkPermit);
//     }

//     getWorkPermitById(Id: string) {
//         return this.http.get<any>(`${getWorkPermitById}/${Id}`);
//     }

//     deleteWorkPermit(Id: string) {

//         return this.http.delete<any>(`${deleteWorkPermit}/${Id}`);
//     }

// }  