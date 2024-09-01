import { City } from "./city";
import { Country } from "./country";
import { employee } from "./employee";
// import { Country } from "./country";
import { State } from "./state";

export interface WorkLocation {
    id: string;
    zipCode: number;
    city: string; // Assuming string type for foreign key, change to appropriate type if needed
    federalState: string;
    country: string; // Assuming string type for foreign key, change to appropriate type if needed
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    employee?: employee | null;
    cityDetails?: City | null;
    countryDetails?: Country | null;
    action : number;
  }
  export interface SaveWorkLocation {
    // action: import("c:/Synobiz/Angular/workspace/UI/beOnHR-ui/src/app/shared/constant/enum.const").ActionEnum;
    id: string;
    zipCode: number;
    city: string; // Assuming string type for foreign key, change to appropriate type if needed
    federalState: string;
    country: string; // Assuming string type for foreign key, change to appropriate type if needed
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    employee?: employee | null;
    cityDetails?: City | null;
    countryDetails?: Country | null;
    action: number;
    
  }