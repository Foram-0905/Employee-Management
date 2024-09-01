import { Injectable } from '@angular/core';
import { HttpClient ,  HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { City, SaveCity } from '../models/city';
import { RequestWithFilterAndSort , } from '../models/FilterRequset';
// import { ManageDesignation, SaveCity } from '../models/city';
import { getCountry,GetCity,GetAllState, GetCityById ,GetCityByState, DeleteCityById, SaveCities, GetFilterCity} from '../constant/api.const';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root'
})

export class CityService {
  constructor(private http: HttpClient) { }

  getCity(): Observable<City[]> {
    return this.http.get<any>(GetCity);
  }
  GetCityByState(stateId:string) {
    return this.http.get<any>(`${GetCityByState}/${stateId}`);
  }

  getCityById(Id:string) {
    return this.http.get<any>(`${GetCityById}/${Id}`);
  }
  addCity(city: City): Observable<any> {
    return this.http.post(SaveCities, city);
  }
  deleteCityById(Id: string){

    return this.http.delete<any>(`${DeleteCityById}/${Id}`);
  }
  GetFilterCity(requst:RequestWithFilterAndSort): Observable<any> {
    return this.http.post<any>(GetFilterCity, requst);
  }

  

  // saveCities(city: SaveCity): Observable<any> {

  //   // debugger
  //   return this.http.post<any>(SaveCities, city);
  // }

  getCountryNames(): Observable<{ id: string, name: string }[]> {
    return this.http.get<any>(getCountry).pipe(
      map(response => {
        if (response.isSuccess && response.httpResponse) {
          // Filter out countries where isDeleted is false
          return response.httpResponse
            .filter((country: any) => !country.isDeleted)
            .map((country: any) => ({
              id: country.id,
              name: country.countryName
            }));
        } else {
          // Handle error or empty response
          return [];
        }
      })
    );
  }

 getStateName(): Observable<{ id: string, name: string }[]> {
  return this.http.get<any>(GetAllState).pipe(
    map(response => {
      if (response.isSuccess && response.httpResponse) {
        // Filter out countries where isDeleted is false
        return response.httpResponse
          .filter((state: any) => !state.isDeleted)
          .map((state: any) => ({
            id: state.id,
            name: state.name
          }));
      } else {
        return [];
      }
    })
  );
}}


