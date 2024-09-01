import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeProfileComponent } from './employee-profile/employee-profile.component';
import { FinancialTabComponent } from './employee-profile/financial-tab/financial-tab.component';
import { PersonalTabComponent } from './employee-profile/personal-tab/personal-tab.component';

const routes: Routes = [
  {path:'employeeprofile',component:EmployeeProfileComponent},
  {path:'financialdetails',component:FinancialTabComponent},
  {path:'personaldetails',component:PersonalTabComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeProfileRoutingModule { }
