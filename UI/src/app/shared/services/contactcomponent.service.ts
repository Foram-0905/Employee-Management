import { Injectable } from '@angular/core';
import { HttpClient ,  HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { State } from '../models/state';
import { City, SaveCity } from '../models/city';
import { RequestWithFilterAndSort , } from '../models/FilterRequset';
import { GetAllContactAddress,GetFilterContactAddress, GetContactAddressByid ,  SaveContactAddress, DeleteContactAddress, GetCity,GetEmployee,GetContact} from '../constant/api.const';
import { ContactAddress } from '../models/contact-address';
import { WorkLocation } from '../models/work-location';
import { GetAllBankDetails , SaveBankDetails} from '../constant/api.const';
import { BankDetails } from '../models/bank-details';
import { GetAllWorklocation, SaveWorkLocation, GetCountry} from '../constant/api.const';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  @Injectable({
    providedIn: 'root'
  })
  export class ContactAddressService {
    constructor(private http: HttpClient) { }
  
   
    GetAllContactAddress(){
      return this.http.get<any>(GetContact);
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
      GetAllBankDetails(): Observable<BankDetails[]> {
        return this.http.get<any>(GetAllBankDetails);
    }
    SaveBankDetails(bankdetails: BankDetails): Observable<any> {
        return this.http.post(SaveBankDetails, bankdetails);
    }
    GetAllWorklocation(): Observable<WorkLocation[]> {
        return this.http.get<any>( GetAllWorklocation);
      }
      SaveWorkLocation(worklocation: WorkLocation): Observable<any> {
        return this.http.post(SaveWorkLocation, worklocation);
      }
  }    