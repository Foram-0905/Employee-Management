import { Injectable } from '@angular/core';
import { HttpClient ,  HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { State } from '../models/state';
import { Contact} from '../models/contact';
import { BehaviorSubject} from 'rxjs';
import { City, SaveCity } from '../models/city';
import { RequestWithFilterAndSort , } from '../models/FilterRequset';
import { GetAllContactAddress,GetFilterContactAddress, GetContactAddressByid ,GetContactByEmployeeId, GetAllState, SaveContactAddress, DeleteContactAddress,GetContactById,GetCity,GetEmployee, getContact,saveContact, GetCityByState,GetStateByCountryId} from '../constant/api.const';
import { ContactAddress } from '../models/contact-address';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  @Injectable({
    providedIn: 'root'
  })
  export class ContactService {
    private contactSubject: BehaviorSubject<Contact> = new BehaviorSubject<Contact>({} as Contact);
    public contact$: Observable<Contact> = this.contactSubject.asObservable();
  
    constructor(private http: HttpClient) { }
    getContacts():Observable<Contact[]>{
        return this.http.get<any>(getContact);
    }
    saveContact(contact : Contact): Observable<any> {
        return this.http.post(saveContact, contact);
    }
    GetContactById(Id:string) {
      return this.http.get<any>(`${GetContactById}/${Id}`);
    }
    GetContactByEmployeeId(Id: string) {
      return this.http.get<any>(`${GetContactByEmployeeId}/${Id}`);
    }
    updateContact(contact: Contact): void {
      this.contactSubject.next(contact);
    }
    GetAllContactAddress(): Observable<ContactAddress[]> {
      return this.http.get<any>(GetAllContactAddress);
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