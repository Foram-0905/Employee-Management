import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
 
  {
    path:'employelistmodule',loadChildren:()=>import('./employee-list/employee-list.module').then(m=>m.EmployeeListModule)
  },
  {
    path:'employeeprofilemodule',loadChildren:()=>import('./employee-profile/employee-profile.module').then(m=>m.EmployeeProfileModule)
  },
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeRoutingModule { }
