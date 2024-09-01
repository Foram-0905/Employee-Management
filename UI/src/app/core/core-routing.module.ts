import { NgModule } from '@angular/core';
import { NavigationCancellationCode, RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from '../feature/dashboard/dashboard.component';
import { EmployeeListComponent } from '../feature/employee/employee-list/employee-list/employee-list.component';
import { EmployeeProfileComponent } from '../feature/employee/employee-profile/employee-profile/employee-profile.component';
import { OrgChartComponent } from '../feature/org-chart/org-chart/org-chart.component';
import { LeaveComponent } from '../feature/leave/leave/leave.component';
import { ConfigurationModule } from '../feature/configuration/configuration.module';
import { LoginComponent } from './components/auth/login/login.component';
import { PasswordResetComponent } from './components/auth/password-reset/password-reset.component';
import { authGuard, authGuards } from './guards/auth.guard';
import { NavigationComponent } from './components/navigation/navigation/navigation.component';

const routes: Routes = [
 
  
  //  { 
  //    path: '', // Empty path for default route
  //    redirectTo: '/login', // Redirect to login component
  //    pathMatch: 'full' // Ensure full match for the path
  //  },
  // {
  //  path:'dashboard',component:DashboardComponent,canActivate:[authGuard,tokenRefresh]
  // },

  
  


  { path: 'employee-list', component: EmployeeListComponent },

  { path: 'employee-profile', component: EmployeeProfileComponent },

  //{ path: 'org-chart', component: OrgChartComponent },

 // { path: 'leave', component: LeaveComponent },

  // { path: 'configuration', component: ConfigurationModule },

  // {
  //   path:'featureModule',loadChildren:()=>import('../feature/feature.module').then(m=>m.FeatureModule),canActivate:[authGuard,tokenRefresh]
  // },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRoutingModule { }
