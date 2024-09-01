import { Injectable } from '@angular/core';
import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Asset_Status } from '../models/assets_status';
import { GetAssetsStatus } from '../constant/api.const';

@Injectable({
  providedIn: 'root'
})
export class AssetStatusService {

  constructor(private http: HttpClient) { }

  getAssetsStatus(): Observable<Asset_Status[]> {
    return this.http.get<any>(GetAssetsStatus)
  }
}