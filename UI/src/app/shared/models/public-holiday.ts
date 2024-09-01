import { Country } from "./country";
import { State } from "./state";

export interface PublicHoliday {
    id: string;
    country: string; // Assuming string type for foreign key, change to appropriate type if needed
    state: string; // Assuming string type for foreign key, change to appropriate type if needed
    holidayName: string;
    holidayDate: string; // Change to appropriate date type if needed
    stateDetails?: State | null;
    countryDetails?: Country | null;
    action :number;
  }