import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { saveDocument } from '../constant/api.const';
import { DocumentList } from '../models/documentlist';
import { Document } from '../models/document';
import { getDocumentlist,getFilterDocumentlist,saveDocumentlist,getDocumemtlistById,deleteDocumentlist,GetDocumentListByEmployeeId,GetDocumentListByEntityId } from '../constant/api.const';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  @Injectable({
    providedIn: 'root'
  })
export class DocumentListService {
  constructor(private http: HttpClient) {
}
  saveDocumentList(document: DocumentList): Observable<any> {
    return this.http.post<any>(saveDocumentlist, document);
  }
  getDocumentlist(): Observable<DocumentList[]> {
    return this.http.get<any>(getDocumentlist);
  }
  getDocumemtlistById(Id: string) {
    return this.http.get<any>(`${getDocumemtlistById}/${Id}`);
  }
  GetDocumentListByEntityId(Id: string,fileName:string){
    return this.http.get<any>(`${GetDocumentListByEntityId}/${Id}?fileName=${fileName}`);
  }
  GetDocumentbyEmployeeId(Id: string){
    return this.http.get<any>(`${GetDocumentListByEmployeeId}/${Id}`);
  }
  deleteDocumentlist(Id: string) {
    return this.http.delete<any>(`${deleteDocumentlist}/${Id}`);
  }
  getFilterDocumentlist(requst:RequestWithFilterAndSort): Observable<any> {
    return this.http.post<any>(getFilterDocumentlist, requst);
  }
}