import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EducationLevelListComponent } from './education-level-list/education-level-list.component';

const routes: Routes = [
  {path:'educationlevel',component:EducationLevelListComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EducationLevelRoutingModule { }
