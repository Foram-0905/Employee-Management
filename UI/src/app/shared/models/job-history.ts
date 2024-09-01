import { City } from "./city";
import { Country } from "./country";
import { DocumentList } from "./documentlist";
import { employee } from "./employee";

import { EmployeeType } from "./employeeType";
import { State } from "./state";

export interface JobHistory {
    id: string;
    companyName: string;
    positionHeld: string;
    employmentType: string;
    employmentTypeName:string;
    stateId:string;
    stateName:string;
    zipCode: number;
    city: string;
    cityName:string;
    country: string;
    countryName:string; 
    startDate: string; 
    endDate: string; 
    document: string;
    leavingReason: string;
    employeeId: string;
    employee?: employee | null;
    cityDetails?: City | null;
    countryDetails?: Country | null;
    employeeTypeDetails?:EmployeeType|null;
    action: number;
    filename?:any;
  }
  