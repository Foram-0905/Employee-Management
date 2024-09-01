import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SLGGroup } from '../models/slg-group';
import {SaveSlgGroup,GetAllSlgGroups, getSLGGroupById, deleteSLGGroup,getFilterslggroup} from '../constant/api.const';
import { Observable } from 'rxjs';
import { RequestWithFilterAndSort } from '../models/FilterRequset';


@Injectable({
  providedIn: 'root'
})
export class SlgGroupService {

  constructor(private http:HttpClient) {  }

  SaveSlggroup(slgGroups: SLGGroup): Observable<any>{

    return this.http.post<any>(SaveSlgGroup, slgGroups);
  }

  GetAllSlggroup(){
    return this.http.get<any>(GetAllSlgGroups);
  }
  deleteSLGGroup(Id: string) {

    return this.http.delete<any>(`${deleteSLGGroup}/${Id}`);
  }
  getSLGGroupById(Id:string){
    return this.http.get<any>(`${getSLGGroupById}/${Id}`);
  }
  getFilterslggroup(requst:RequestWithFilterAndSort){
    return this.http.post<any>(getFilterslggroup,requst)
  }
}
