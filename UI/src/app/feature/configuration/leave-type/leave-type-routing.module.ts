import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeaveTypeListComponent } from './leave-type-list/leave-type-list.component';

const routes: Routes = [
  {path:'leavetype',component:LeaveTypeListComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LeaveTypeRoutingModule { }
