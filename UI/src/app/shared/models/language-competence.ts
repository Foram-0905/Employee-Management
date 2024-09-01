import { employee } from "./employee";
import { LanguageLevel } from "./language-level";

export interface LanguageCompetence {
    id: string;
    name: string;
    level: string; // Assuming string type for foreign key, change to appropriate type if needed
    languagesCertificate?: string | null;
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    // employee?: Employee | null;
    action: number;
    // languageLevel?: LanguageLevel | null;
  }