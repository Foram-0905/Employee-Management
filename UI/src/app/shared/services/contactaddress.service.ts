import { Injectable } from '@angular/core';
import { HttpClient ,  HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { State } from '../models/state';
import { City, SaveCity } from '../models/city';
import { RequestWithFilterAndSort , } from '../models/FilterRequset';
import { GetAllContactAddress,GetFilterContactAddress, GetContactAddressByid ,getContact, GetAllState, SaveContactAddress, DeleteContactAddress, GetCity} from '../constant/api.const';
import { ContactAddress } from '../models/contact-address';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  @Injectable({
    providedIn: 'root'
  })
  export class ContactAddressService {
    constructor(private http: HttpClient) { }
  
    GetAllContactAddress(){
      return this.http.get<any>(getContact);
    }

    GetContactAddressByid(Id:string) {
      return this.http.get<any>(`${GetContactAddressByid}/${Id}`);
    }
    SaveContactAddress(contactaddress: ContactAddress): Observable<any> {
      return this.http.post(SaveContactAddress, contactaddress);
    }
    DeleteContactAddress(Id: string){
  
      return this.http.delete<any>(`${DeleteContactAddress}/${Id}`);
    }
    GetFilterContactAddress(requst:RequestWithFilterAndSort): Observable<any> {
      return this.http.post<any>(GetFilterContactAddress, requst);
    }
    GetCity(): Observable<City[]> {
        return this.http.get<any>(GetCity);
      }
      GetAllStates()
      {
        return this.http.get<any>(GetAllState);
      }
  }    