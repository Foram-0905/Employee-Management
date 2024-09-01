import { Injectable } from '@angular/core';
import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Asset_type } from '../models/assets_type';
import { GetAssetstype } from '../constant/api.const';

@Injectable({
  providedIn: 'root'
})
export class AssettypeService {

  constructor(private http: HttpClient) { }

  getAssetstype(): Observable<Asset_type[]> {
    return this.http.get<any>(GetAssetstype)
  }
}