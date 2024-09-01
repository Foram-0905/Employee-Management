import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserActiveLogComponent } from './user-active-log/user-active-log.component';

const routes: Routes = [
  {path:'useractivelog',component:UserActiveLogComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserActiveLogRoutingModule { }
