import { Country } from "./country";
import { State } from "./state";

export interface City {
    id: string;
    name: string;
    countryId: string; // Assuming string type for foreign key, change to appropriate type if needed
    state: string; // Assuming string type for foreign key, change to appropriate type if needed
    stateId?: State | null;
    country?: Country | null;
    action:number;
  }
  
export interface SaveCity {
  id: string;
  name: string;
  countryId: string; // Assuming string type for foreign key, change to appropriate type if needed
  state: string; // Assuming string type for foreign key, change to appropriate type if needed
  stateId?: State | null;
  country?: Country | null;
  action:number;
}