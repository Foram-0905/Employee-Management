import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getCountry ,saveCountry, deleteCountry,getCountryById, getFilterCountry,GetStateByCountryId} from '../constant/api.const';
// import { ManageDesignation ,Savedesignation} from '../models/manage-designation';
import { Country } from '../models/country';
import { RequestWithFilterAndSort } from '../models/FilterRequset';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};
@Injectable({
  providedIn: 'root'
})
export class CountryService {

  constructor(private http:HttpClient) {

   }

   saveCountry(Country:Country): Observable<any>{

      return this.http.post<any>(saveCountry, Country);
   }
   GetStateByCountryId(countryId: string): Observable<any> {
    return this.http.get<any>(`${GetStateByCountryId}/${countryId}`);
  }

   getCountryList(){

     return this.http.get<any>(getCountry);
   }
   getCountryById(Id: string) {
    // //debugger;
    return this.http.get<any>(`${getCountryById}/${Id}`);
  }
  
  deleteCountry(Id: string) {
    // //debugger;
    return this.http.delete<any>(`${deleteCountry}/${Id}`);
  }
  getFilterCountry(requst:RequestWithFilterAndSort): Observable<any> {
    return this.http.post<any>(getFilterCountry, requst);
  }
}
