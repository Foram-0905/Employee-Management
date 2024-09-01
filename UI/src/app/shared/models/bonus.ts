import { SalaryType } from "./salarytype";
import { SaveBonus } from "../constant/api.const";
import { employee } from "./employee";

export interface Bonus{
    id:string;
    entitlement:boolean;
    startingDate:Date|null;
    endingDate:Date|null;
    bonusamount:string;
    salaryType:string;
    employeeId:string;
    employeeDetails:employee|null;
    salarytypeDetails:SalaryType|null;
}

export interface SaveBonus{
    id:string;
    entitlement:boolean;
    startingDate:Date|null;
    endingDate:Date|null;
    bonusamount:string;
    salaryType:string;
    employeeId:string;
    employeeDetails:employee|null;
    salarytypeDetails:SalaryType|null;
    action:number;
}
