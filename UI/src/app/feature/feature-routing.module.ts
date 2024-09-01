import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { authGuard } from '../core/guards/auth.guard';


const routes: Routes = [
  {
    path:'daashborad',component:DashboardComponent
  },
  {
    path:'employeemodule',loadChildren:()=>import('./employee/employee.module').then(m=>m.EmployeeModule)
  },
  {
    path:'configurationmodule',loadChildren:()=>import('./configuration/configuration.module').then(m=>m.ConfigurationModule)
  },
  {
    path:'leavemodule',loadChildren:()=>import('./leave/leave.module').then(m=>m.LeaveModule)
  },
  {
    path:'orgchartmodule',loadChildren:()=>import('./org-chart/org-chart.module').then(m=>m.OrgChartModule)
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FeatureRoutingModule { }
