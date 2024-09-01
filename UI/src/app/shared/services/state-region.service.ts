import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {State}  from '../models/state'
import {SaveState,GetAllState,getStateById,deleteState,getFilterState,GetStateByCountryId} from '../constant/api.const'
import { Observable } from 'rxjs';
import { RequestWithFilterAndSort } from '../models/FilterRequset';

@Injectable({
  providedIn: 'root'
})
export class StateRegionService {

  constructor(private http:HttpClient) {  }

  SaveState_Region(state:State):Observable<any>{
    return this.http.post<any>(SaveState,state);
  }
  GetAllStates()
  {
  	return this.http.get<any>(GetAllState);
  }
  GetStateByCountryId(countryId: string) {
    return this.http.get<any>(`${GetStateByCountryId}/${countryId}`);
  }
  getStateById(id:string){
    return this.http.get<any>(`${getStateById}/${id}`);
  }
  deleteState(id:string){
    return this.http.delete<any>(`${deleteState}/${id}`);
  }
 
  getFilterState(requst:RequestWithFilterAndSort){
    return this.http.post<any>(getFilterState,requst)
  }
}
