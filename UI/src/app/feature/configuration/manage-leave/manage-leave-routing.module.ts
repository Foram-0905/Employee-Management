import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageLeaveComponent } from './manage-leave/manage-leave.component';

const routes: Routes = [
  {path:'manageleave',component:ManageLeaveComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageLeaveRoutingModule { }
