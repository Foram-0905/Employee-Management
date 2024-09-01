import { employee } from "./employee";

export interface BankDetails {
    id: string;
    accountNumber: string;
    ifscCode: string;
    bankName: string;
    accountHolder: string;
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    employee?: employee | null;
    action: number;
  }
  export interface SaveBankDetails {
    id: string;
    accountNumber: string;
    ifscCode: string;
    bankName: string;
    accountHolder: string;
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    employee?: employee | null;
    action: number;
  }