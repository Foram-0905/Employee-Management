import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RoleListComponent } from '../../feature/configuration/role/role-list/role-list.component';
import {  GetAllRole, GetRoleByid, SaveRole, DeleteRole, GetFilterRole } from '../constant/api.const';
import { Role } from '../models/role';
import { RequestWithFilterAndSort } from '../models/FilterRequset';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};
@Injectable({
    providedIn: 'root'
})
export class roleservice {
    constructor(private http: HttpClient) {
    }
    getRoleList() {
        return this.http.get<any>(GetAllRole);
      }
      getRoleById(Id: string) {

        return this.http.get<any>(`${GetRoleByid}/${Id}`);
      }

      saveRole(Role: Role): Observable<any> {

        return this.http.post<any>(SaveRole, Role);
      }
      deleterole(Id: string) {

        return this.http.delete<any>(`${DeleteRole}/${Id}`);
      }
      GetFilterRole(requst:RequestWithFilterAndSort): Observable<any> {
        return this.http.post<any>(GetFilterRole, requst);
      }
    }
