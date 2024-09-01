import { City } from "./city";
import { employee } from "./employee";
import { Country } from "./country";
import { State } from "./state";
// import { StateRegionAddComponent } from "../../feature/configuration/state-region/state-region-add/state-region-add.component";
import { SaveContactAddress } from "../constant/api.const";
import { ContactComponent } from "../../feature/employee/employee-profile/employee-profile/personal-tab/navbar/contact/contact.component";

export interface ContactAddress {
    id: string;
    street: string;
    zipCode: number;
    country:string;
    state:string;
    city: string; // Assuming string type for foreign key, change to appropriate type if needed
    phone1: string;
    phone2?: string | null;
    emailbeON: string;
    emailPrivate: string;
    entitlement: boolean;
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    employee?: employee | null;
    cityDetails?: City | null;
    action:number;
  }
  
export interface SaveContactAddress {
  id: string;
  number: string;
  street: string;
  country:string;
  state:string;
  zipCode: number;
  city: string; // Assuming string type for foreign key, change to appropriate type if needed
  phone1: string;
  phone2?: string | null;
  emailbeON: string;
  emailPrivate: string;
  entitlement: boolean;
  employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
  employee?: employee | null;
  cityDetails?: City | null;
  action:number;
}