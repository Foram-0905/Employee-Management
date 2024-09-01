import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { map } from 'rxjs/operators';
import { Currency } from '../models/currency';
import { GetCurrency, SaveCurrency, getCountry, getCurrencyById, deleteCurrencyById,getFilterCurrency } from '../constant/api.const';

@Injectable({
  providedIn: 'root'
})

export class CurrencyService {
  constructor(private http: HttpClient) { }


  getCurrencies(): Observable<Currency[]> {
    return this.http.get<any>(GetCurrency)
  }
  getCurrencyById(Id: string) {
    return this.http.get<any>(`${getCurrencyById}/${Id}`)
  }

  addCurrency(currency: Currency): Observable<any> {
    return this.http.post(SaveCurrency, currency);
  }
  deleteCurrency(Id: string) {

    return this.http.delete<any>(`${deleteCurrencyById}/${Id}`);
  }
  GetFilterCurrency(requst:RequestWithFilterAndSort): Observable<any> {
    return this.http.post<any>(getFilterCurrency, requst);
  }

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
}
