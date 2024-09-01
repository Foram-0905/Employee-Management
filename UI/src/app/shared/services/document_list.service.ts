import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Document } from '../models/document';
@Injectable({
    providedIn: 'root'
  })
  export class DocumentsListService {
    constructor(private http: HttpClient) {
  
    }
    saveIdentityCard(documents: Document): Observable<any> {

        return this.http.post<any>(saveIdentityCard, documents);
      }
    
      getIdentityCard() {
    
        return this.http.get<any>(getIdentityCard);
      }
    
      getIdentityCardById(Id: string) {
        return this.http.get<any>(`${getIdentityCardById}/${Id}`);
      }
    
      deleteDesignation(Id: string) {
        return this.http.delete<any>(`${deleteIdentityCard}/${Id}`);
      }
  }  