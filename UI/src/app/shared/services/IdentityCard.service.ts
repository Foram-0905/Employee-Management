import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IdentityCard } from '../models/identity-card';
import { deleteIdentityCard, getIdentityCard, getIdentityCardById, saveIdentityCard,GetIdentityCardByEmployeeId } from '../constant/api.const';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  @Injectable({
    providedIn: 'root'
  })
  export class IdentityCardService {

    constructor(private http: HttpClient) {
  
    }
    saveIdentityCard(identity: IdentityCard): Observable<any> {

        return this.http.post<any>(saveIdentityCard, identity);
      }
    
      getIdentityCard() {
    
        return this.http.get<any>(getIdentityCard);
      }
    
      getIdentityCardById(Id: string) {
        return this.http.get<any>(`${getIdentityCardById}/${Id}`);
      }
    
      GetIdentityCardByEmployeeId(Id: string) {
        return this.http.get<any>(`${GetIdentityCardByEmployeeId}/${Id}`);
      }

      deleteDesignation(Id: string) {
        return this.http.delete<any>(`${deleteIdentityCard}/${Id}`);
      }
}