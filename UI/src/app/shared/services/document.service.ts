import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { saveDocument } from '../constant/api.const';
import { Document } from '../models/document';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  @Injectable({
    providedIn: 'root'
  })

export class DocumentService {

    
  constructor(private http: HttpClient) {

  }
  
  saveDocument(document: Document): Observable<any> {

    return this.http.post<any>(saveDocument, document);
  }
}