import { employee } from "./employee";

export interface WorkPermitDetail {
  id: string;
  permitType: string;
  permitStartDate: string; // Change to appropriate date type if needed
  permitExpiryDate: string; // Change to appropriate date type if needed
  document: string;
}

export interface IdentityCard {
  id: string;
  passport: string;
  visaStartDate: string; // Change to appropriate date type if needed
  visaExpiryDate: string; // Change to appropriate date type if needed
  visa: string;
  blueCardStartDate: string; // Change to appropriate date type if needed
  blueCardExpiryDate: string; // Change to appropriate date type if needed
  blueCard: string;
  employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed

  action: number;
  workPermitDetails?: WorkPermitDetail[];
}

// "id": "7bcb7af4-871a-4b7b-1ac8-08dc789aef7f",
// "passport": "string",
// "visaStartDate": "1998-01-01",
// "visaExpiryDate": "1998-01-01",
// "visa": "string",
// "blueCardStartDate": "1998-01-01",
// "blueCardExpiryDate": "1998-01-01",
// "blueCard": "string",
// "employeeId": "bc0425a8-34cd-46ec-9cb8-2eda5192a747",