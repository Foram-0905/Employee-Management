import { SaveLeaveCategory } from "../constant/api.const";
export interface LeaveCategory {
    // httpResponse: any;
    id: string;
    name: string;
    action: number;
  }
  export interface SaveLeaveCategory {
    id: string;
    name: string;
    action : number;
  }