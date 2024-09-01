import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './core/components/auth/login/login.component';
import { DashboardComponent } from './feature/dashboard/dashboard.component';
import { authGuard, authGuards,  } from './core/guards/auth.guard';
import { NavigationComponent } from './core/components/navigation/navigation/navigation.component';
import { PasswordResetComponent } from './core/components/auth/password-reset/password-reset.component';
import { Error401Component } from './core/components/error-401/error-401/error-401.component';
import { EmployeeListComponent } from './feature/employee/employee-list/employee-list/employee-list.component';


const routes: Routes = [
  {
   path:'login',component:LoginComponent,canActivate:[authGuards]
  },




  {
    path: '', // Empty path for default route
    redirectTo: '/login', // Redirect to login component
    pathMatch: 'full' // Ensure full match for the path
  },
  // { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path:'passwordreset',component:PasswordResetComponent },

 {
  path:'dashboard',component:DashboardComponent,canActivate:[authGuard]
 },




  // {
  //   path:'coreModule',loadChildren:()=>import('./core/core.module').then(m=>m.CoreModule),canActivate:[authGuard]
  // },

 {
  path:'dashboard',component:DashboardComponent,canActivate:[authGuard]
 },

  //{path:'navigation',component:NavigationComponent,canActivate:[authGuard]},

  {
    path:'featureModule',loadChildren:()=>import('./feature/feature.module').then(m=>m.FeatureModule),canActivate:[authGuard]
  },
 
  //module is lazy loades and can be accessed by /employee
  {
    path:'employee',loadChildren:()=>import('./feature/employee/employee-profile/employee-profile.module').then(m=>m.EmployeeProfileModule),canActivate:[authGuard]
  },
  {
    path:'employees',loadChildren:()=>import('./feature/employee/employee-list/employee-list.module').then(m=>m.EmployeeListModule),canActivate:[authGuard]
  },

  {
    path:'employee',loadChildren:()=>import('./feature/leave/leave.module').then(m=>m.LeaveModule),canActivate:[authGuard]
  },
  {
    path:'employee',loadChildren:()=>import('./feature/org-chart/org-chart.module').then(m=>m.OrgChartModule),canActivate:[authGuard]
  },
  //  module is lazy loaded and can be accessed by /configurations

    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/assets/assets.module').then(m => m.AssetsModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/city/city.module').then(m=>m.CityModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/country/country.module').then(m=>m.CountryModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/currency/currency.module').then(m=>m.CurrencyModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/designation/designation.module').then(m=>m.DesignationModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/education-level/education-level.module').then(m=>m.EducationLevelModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/language-level/language-level.module').then(m=>m.LanguageLevelModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/leave-category/leave-category.module').then(m=>m.LeaveCategoryModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/leave-type/leave-type.module').then(m=>m.LeaveTypeModule),canActivate:[authGuard]
    },
    {

      path:'configuration',loadChildren:()=>import('./feature/configuration/manage-leave/manage-leave.module').then(m=>m.ManageLeaveModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/permission/permission.module').then(m=>m.PermissionModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/public-holiday/public-holiday.module').then(m=>m.PublicHolidayModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/role/role.module').then(m=>m.RoleModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/slg/slg.module').then(m=>m.SlgModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/state-region/state-region.module').then(m=>m.StateRegionModule),canActivate:[authGuard]
    },
    {
      path:'configuration',loadChildren:()=>import('./feature/configuration/user-active-log/user-active-log.module').then(m=>m.UserActiveLogModule),canActivate:[authGuard]
    },
    { path:'Error401',component:Error401Component },
// Wildcard route for any invalid paths
 { path: '**', redirectTo: '/Error401' } // Redirect to dashboard for any unmatched paths

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule] 
})
export class AppRoutingModule { }
