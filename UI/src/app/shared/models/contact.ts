
import { employee } from "./employee";


export interface Contact {
  id: string;
  workZipCode: string;
  workCity: number;
  workStateId: string;
  workCountryId: string;
  employeeId: string;
  employee?: employee | null;
  action: number;

  contactAdressDetails: ContactAddressDetail[]

  bankDetails?: BankDetail[]
}
export interface ContactAddressDetail {
  id: string
  number: string;
  street: string;
  contactStateId: string;
  contactCountryId: string;
  contactZipCode: string;
  contactCity: string;
  contactPhone1: string;
  contactPhone2: string;
  contactEmailbeON: string;
  contactEmailPrivate: string;
  contactEntitlement: boolean;
  action: number;

}
export interface BankDetail {

  id: string
  bankAccountNumber: string;
  bankIFSCCode: string;
  bankName: string;
  bankAccountHolder: string;
  action: number;

}


