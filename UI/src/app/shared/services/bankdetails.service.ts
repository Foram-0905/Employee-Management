import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RequestWithFilterAndSort, } from '../models/FilterRequset';
import { map } from 'rxjs/operators';
import { GetAllBankDetails , SaveBankDetails} from '../constant/api.const';
import { BankDetails } from '../models/bank-details';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};
@Injectable({
    providedIn: 'root'
})
export class BankDetailsService {
    constructor(private http: HttpClient) { }

    GetAllBankDetails(): Observable<BankDetails[]> {
        return this.http.get<any>(GetAllBankDetails);
    }
    SaveBankDetails(bankdetails: BankDetails): Observable<any> {
        return this.http.post(SaveBankDetails, bankdetails);
    }
}
