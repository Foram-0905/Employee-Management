import { employee } from "./employee";
import { SalaryType } from "./Salary_Type";
import { Currency } from "./currency";
import { CreditDebit } from "../constant/enum.const";
export interface Salary {
    id: string;
    salaryType:string;
    amount: number;
    currency:string;
    currenccy:Currency;
    salaryStartDate:Date;
    salaryEndDate:Date;
    employeeId:string;
    firstName:string;
    action:number;
    transactionType:CreditDebit;
  }