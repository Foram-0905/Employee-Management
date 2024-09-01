import { Country } from "./country";

export interface State {
    id: string;
    name: string;
    countryId: string; // Assuming string type for foreign key, change to appropriate type if needed
    action:number;
    
  }