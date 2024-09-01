import { permission } from './../models/permission';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GetAllrole, getPermissionById, setPermission } from '../constant/api.const';
import { map, Observable, tap } from 'rxjs';
import { LoginService } from '../../core/services/login.service';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};
@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  private permissions: any[] = [];

  constructor(private http: HttpClient,private loginService:LoginService) { }

  loadPermissions(): Observable<any> {
   
    const LoginUser = this.loginService.currentUserValue;
    return this.http.get<any>(`${getPermissionById}/${LoginUser.roleId}`).pipe(map((User) => {
    

      this.permissions = User.httpResponse;
    })
    );
  }

  hasPermission(permission: string): boolean {
    return this.permissions.includes(permission);
  }
  GetAllrole() {
    return this.http.get<any>(GetAllrole);
  }

  getPermissionById(id: any) {
    return this.http.get<any>(`${getPermissionById}/${id}`);
  }

  savePermission(Requst: permission): Observable<any> {
    return this.http.post<any>(setPermission, Requst);
  }

}
