// employee.ts

import { ActionEnum } from "../constant/enum.const";
import { ManageDesignation } from "./manage-designation";
import { SLGGroup } from "./slg-group";

export interface employee {
  id: string;
  employeeNumber: number;
  currentStatusId: string;
  employementTypeId: string;
  typeofEmploymentId: string;
  workingHours: string;
  jobTitle: string;
  contractualStartDate: string; // Date ke liye format ko customize karna padega
  contractualEndDate: string; // Date ke liye format ko customize karna padega
  email: string;
  firstName: string;
  middleName: string;
  lastName: string;
  gender: string;
  birthdate: string; // Date ke liye format ko customize karna padega
  birthCity: string;
  birthCountryId: string;
  nationality: string;
  taxId?: string;
  socialSecurity?: string;
  taxClassId?: string;
  healthInsaurance?: string;
  maritalStatusId?: string;
  religiousaffiliation?: string;
  profilePhoto?: string;
  personalSheet?: string;
  socialSecurityFile?: string;
  employeeStatusId: string;

  //*********Children information**********//
  employeehavechildren: boolean;
  child_FirstName?: string;
  familyName?: string;
  chIld_BirthDate?: string; // Date ke liye format ko customize karna padega
  locationchildregistered?: string;
  socialcareinsurance?: string;
  birthCertificate?: string;

  //*********Language Competence**********//
  languageCompetences: LanguageCompetence[];

  //*********LeaveType**********//
  employeeYearlyLeaveBalances: EmployeeYearlyLeaveBalance[];

  //*********Job Title and Role**********//
  roleId: string;
  slgStatus: string;
  designation: string;

  slgStatusname?:string;
  rolename?:string;

  //*********Leadership**********//
  leader1Id?: string;
  leader2Id?: string;
  defaulLeaderId?: string;

  //*********Probation**********//
  istheemployeeonprobation: boolean;
  startDate?: string; // Date ke liye format ko customize karna padega
  endDate?: string; // Date ke liye format ko customize karna padega
  prob_AdjustedEndDate?: string; // Date ke liye format ko customize karna padega
  adjustedDocument?: string;
  adjustedenddatecheck?: boolean;
  probationUnlimited?: boolean;

  //*********End of Employment**********//
  isthistheendofemployment: boolean;
  noticePeriodStartDate?: string; // Date ke liye format ko customize karna padega
  noticePeriodEndDate?: string; // Date ke liye format ko customize karna padega

  //*********Termination of Employment**********//
  isTerminated: boolean;
  terminationStartDate?: string; // Date ke liye format ko customize karna padega
  terminationEndDate?: string; // Date ke liye format ko customize karna padega
  terminationofemplyee?: string;
  dateofreceipt?: string; // Date ke liye format ko customize karna padega
  deliverymethodId?: string;

  officeEmail?:string;
  fullName?:string;
  phoneNumber?:string;
  workLocation?:number;


  action: number;
}
  
export interface EmployeeYearlyLeaveBalance {
  id: string;
  leaveStartDate: string; // Date ke liye format ko customize karna padega
  leaveEndDate: string; // Date ke liye format ko customize karna padega
  leaveTypeEmployee: string;
  leaveQuota: number;
  adjustedEndDate?: boolean; // Date ke liye format ko customize karna padega
}
export interface LanguageCompetence {
  id: string;
  name: string;
  level: string;
  levelName:string;
  languagesCertificate: string;
   action: number;
}