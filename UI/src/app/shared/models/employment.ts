import { employee } from "./employee";

export interface Employment {
    id: string;
    noticePeriodStartDate: string; // Change to appropriate date type if needed
    noticePeriodEndDate: string; // Change to appropriate date type if needed
    terminationEndDate: string; // Change to appropriate date type if needed
    terminationStartDate: string; // Change to appropriate date type if needed
    terminationEmployee: string;
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    employee?: employee | null;
  }