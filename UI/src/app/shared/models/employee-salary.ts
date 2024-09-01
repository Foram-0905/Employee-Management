import { Currency } from "./currency";

export interface EmployeeSalary {
    id: string;
    salaryType: string;
    amount: string;
    currency: string; // Assuming string type for foreign key, change to appropriate type if needed
    startDate: string; // Change to appropriate date type if needed
    endDate: string; // Change to appropriate date type if needed
    currencyDetails?: Currency | null;
  }