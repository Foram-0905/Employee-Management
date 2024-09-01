import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path:'assetsmodule',loadChildren:()=>import('./assets/assets.module').then(m => m.AssetsModule)
  },
  {
    path:'citymodule',loadChildren:()=>import('./city/city.module').then(m=>m.CityModule)
  },
  {
    path:'countrymodule',loadChildren:()=>import('./country/country.module').then(m=>m.CountryModule)
  },
  {
    path:'currencyymodule',loadChildren:()=>import('./currency/currency.module').then(m=>m.CurrencyModule)
  },
  {
    path:'designationmodule',loadChildren:()=>import('./designation/designation.module').then(m=>m.DesignationModule)
  },
  {
    path:'educationlevelmodule',loadChildren:()=>import('./education-level/education-level.module').then(m=>m.EducationLevelModule)
  },
  {
    path:'languagelevelmodule',loadChildren:()=>import('./language-level/language-level.module').then(m=>m.LanguageLevelModule)
  },
  {
    path:'leavecategorymodule',loadChildren:()=>import('./leave-category/leave-category.module').then(m=>m.LeaveCategoryModule)
  },
  {
    path:'leavetypemodule',loadChildren:()=>import('./leave-type/leave-type.module').then(m=>m.LeaveTypeModule)
  },
  {
    
    path:'manageleavemodule',loadChildren:()=>import('./manage-leave/manage-leave.module').then(m=>m.ManageLeaveModule)
  },
  {
    path:'permissionmodule',loadChildren:()=>import('./permission/permission.module').then(m=>m.PermissionModule)
  },
  {
    path:'publicholidaymodule',loadChildren:()=>import('./public-holiday/public-holiday.module').then(m=>m.PublicHolidayModule)
  },
  {
    path:'rolemodule',loadChildren:()=>import('./role/role.module').then(m=>m.RoleModule)
  },
  {
    path:'slgmodule',loadChildren:()=>import('./slg/slg.module').then(m=>m.SlgModule)
  },
  {
    path:'sateregionmodule',loadChildren:()=>import('./state-region/state-region.module').then(m=>m.StateRegionModule)
  },
  {
    path:'useractivemodule',loadChildren:()=>import('./user-active-log/user-active-log.module').then(m=>m.UserActiveLogModule)
  },
 

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfigurationRoutingModule { }
