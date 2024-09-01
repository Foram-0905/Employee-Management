import { SaveCurrency } from './../constant/api.const';
import { Country } from "./country";

export interface Currency {
    id?: string;
    country: string; // Assuming string type for foreign key, change to appropriate type if needed
    shortWord: string;
    symbol: string;
    countryDetails?: Country | null;

  }

  export interface SaveCurrency {
    id?: string;
    country: string; // Assuming string type for foreign key, change to appropriate type if needed
    shortWord: string;
    symbol: string;
    countryDetails?: Country | null;
    action:number;
  }
