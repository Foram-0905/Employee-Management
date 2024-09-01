import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManagePermissionComponent } from './manage-permission/manage-permission.component';

const routes: Routes = [
  {path:'managepermission',component:ManagePermissionComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PermissionRoutingModule { }
