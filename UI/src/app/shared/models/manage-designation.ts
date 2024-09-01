// manage-designation.model.ts

import { SLGGroup } from "./slg-group";

export interface ManageDesignation {
  id: string;
  initialStatus: string; // Assuming string type for foreign key, change to appropriate type if needed
  designation: string;
  displaySequence: string;
  shortWord: string;
  slgGroup?: SLGGroup | null;
}


export interface Savedesignation {
  id: string;
  initialStatus: string; // Assuming string type for foreign key, change to appropriate type if needed
  designation: string;
  displaySequence: string;
  shortWord: string;
  action: number;
}


