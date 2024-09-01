import { Injectable } from '@angular/core';
import { HttpClient ,  HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { City, SaveCity } from '../models/city';
import { Country } from '../models/country';
import { State } from '../models/state';
import { WorkLocation , } from '../models/work-location';
import { RequestWithFilterAndSort , } from '../models/FilterRequset';
import { GetAllWorklocation, SaveWorkLocation, GetAllState,GetCountry, GetCity,GetEmployee} from '../constant/api.const';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  @Injectable({
    providedIn: 'root'
  })
  export class WorkLocationService {
    constructor(private http: HttpClient) { }
  
    GetAllWorklocation(): Observable<WorkLocation[]> {
      return this.http.get<any>( GetAllWorklocation);
    }
    SaveWorkLocation(worklocation: WorkLocation): Observable<any> {
      return this.http.post(SaveWorkLocation, worklocation);
    }
    GetCity(): Observable<City[]> {
        return this.http.get<any>(GetCity);
      }
  }    