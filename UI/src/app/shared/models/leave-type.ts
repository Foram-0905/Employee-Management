import { LeaveCategory } from "./leave-category";

export interface LeaveType {
    id: string;
    typeName: string;
    categoryName: string; // Assuming string type for foreign key, change to appropriate type if needed
    leaveCategory: LeaveCategory | null;
    action:number;
  }