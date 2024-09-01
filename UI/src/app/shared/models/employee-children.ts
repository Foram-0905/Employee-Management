import { employee } from "./employee";

export interface EmployeeChildren {
    id: string;
    firstName: string;
    familyName: string;
    birthDate: string; // Change to appropriate date type if needed
    locationChildRegistered: string;
    socialCareInsurance: string;
    birthCertificate: string;
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    employee?: employee | null;
  }