import { NgModule} from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SlgListComponent } from './slg-list/slg-list.component';

const routes: Routes = [
  {path:'slg',component:SlgListComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SlgRoutingModule { }
