export interface EmployeeLeave {
  id: string;
  employeeId: string; // Assuming string type for foreign key, change to appropriate type if needed
  leaveType: string; // Assuming string type for foreign key, change to appropriate type if needed
  startDate: string; // Change to appropriate date type if needed
  endDate: string; // Change to appropriate date type if needed
  appliedDate: string; // Change to appropriate date type if needed
  approvedByOfficeManagement: string;
  approvedByTeamLead: string;
  isPending: boolean;
}


export interface typesOFLeaves {
  leaveType: any;
  takenLeave: any;
}

export interface ApprovOrRejectLeave {
  id?: any;
  approvedBy?:any;
  approvedByName?:any;
  approvedOrreject?:any;
  rejecteReson?:any;
}

export interface annualLeaveOfficemangement {
  id:any;
  available:any;
  taken:any;
  remainingDays:any;
  pendingDays:any;
}






