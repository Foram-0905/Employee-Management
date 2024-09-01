import { employee } from "./employee";

export interface Probation {
    id: string;
    startDate: string; // Change to appropriate date type if needed
    endDate: string; // Change to appropriate date type if needed
    adjustedEndDate?: string | null; // Change to appropriate date type if needed
    adjustedDocument?: string | null;
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    employee?: employee | null;
  }