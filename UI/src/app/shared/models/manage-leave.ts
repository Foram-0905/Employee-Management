import { FilterService } from 'primeng/api';
import { RequestWithFilterAndSort } from './FilterRequset';
import { employee } from "./employee";
import { LeaveType } from "./leave-type";

export interface ManageLeave {
    id: string;
    employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
    leavetype: string; // Assuming string type for foreign key, change to appropriate type if needed
    startDate: string; // Change to appropriate date type if needed
    endDate: string; // Change to appropriate date type if needed
    appliedDate: string; // Change to appropriate date type if needed
    leaveDay: string;
    isPending: boolean;
    isApproved: boolean;
    employee?: employee | null;
    leaveTypeDetails?: LeaveType | null;
    RejecteReson:string;
    leave_duration :string;
    leave_End:string;
    leave_Start_From:string;
    reason:string;
    action?: number;
  }

  export interface leavesAccordingLogin{
    pageType:any;
    id:any;
    filterRequset?:RequestWithFilterAndSort;
  }
  export interface leaveAccordingDate{
    date:any;
    id:any;
    filterRequset?:RequestWithFilterAndSort;
  }
