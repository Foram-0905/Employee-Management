import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DesignationListComponent } from './designation-list/designation-list.component';
import { DesignationAddComponent } from './designation-add/designation-add.component';

const routes: Routes = [
  {path:'designation',component:DesignationListComponent},
  {path:'addDesignation',component:DesignationAddComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DesignationRoutingModule { }
