import { City } from "./city";
import { Country } from "./country";
import { EducationLevel } from "./education-level";
import { employee } from "./employee";

export interface EmployeeEducation {
    id: string;
    eductionLevel: string; // Assuming string type for foreign key, change to appropriate type if needed
    subjectOfStudy?: string | null;
    institutionName?: string | null;
    city: string; // Assuming string type for foreign key, change to appropriate type if needed
    country: string; // Assuming string type for foreign key, change to appropriate type if needed
    completionDate: string; // Change to appropriate date type if needed
    certificateFile?: string | null;
    anabin?: string | null;
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    isDeleted: boolean;
    employee?: employee | null;
    educationLevel?: EducationLevel | null;
    cityDetails?: City | null;
    countryDetails?: Country | null;
  }