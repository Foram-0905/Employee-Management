import { employee } from "./employee";

export interface Document {
    id: string;
    personalDataProtection: string;
    confidentialityAgreement: string;
    other: string;
    employeeId: string;
    employee?: employee | null;
    action:number;

  }