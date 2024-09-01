import { Injectable } from '@angular/core';
import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ManageAssets } from '../models/assets';
import { GetAssets, GetAssetsbyId, SaveAssets, DeleteAssets,GetEmployee,GetFilterAssets,GetAssetsStatus,GetAssetstype } from '../constant/api.const';

@Injectable({
  providedIn: 'root'
})
export class AssetService {

  constructor(private http: HttpClient) { }

  getAssets(): Observable<ManageAssets[]> {
    return this.http.get<any>(GetAssets)
  }
  getAssetsById(Id: string) {
    return this.http.get<any>(`${GetAssetsbyId}/${Id}`)
  }
  SaveAssets(assets: ManageAssets): Observable<any> {
    return this.http.post(SaveAssets, assets);
  }
  DeleteAssets(Id: string) {
    return this.http.delete<any>(`${DeleteAssets}/${Id}`);
  }
  GetFilterAssets(requst:RequestWithFilterAndSort): Observable<any> {
    return this.http.post<any>(GetFilterAssets, requst);
  }

  // getEmployeeNames(): Observable<{ id: string, firstName: string }[]> {
  //   return this.http.get<any>(GetEmployee).pipe(
  //     map(response => {
  //       if (response.isSuccess && response.httpResponse) {
  //         return response.httpResponse.map((employee: any) => ({
  //           id: employee.id,
  //           firstName: employee.firstName
  //         }));
  //       } else {
  //         // Handle error or empty response
  //         return [];
  //       }
  //     })
  //   );
  // }
}
