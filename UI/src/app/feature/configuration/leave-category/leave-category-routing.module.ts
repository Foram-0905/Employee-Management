import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeaveCategoryListComponent } from './leave-category-list/leave-category-list.component';

const routes: Routes = [
  {path:'leavecategory',component:LeaveCategoryListComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LeaveCategoryRoutingModule { }
