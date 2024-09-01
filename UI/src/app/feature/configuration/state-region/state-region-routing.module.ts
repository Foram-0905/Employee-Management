import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StateRegionListComponent } from './state-region-list/state-region-list.component';
import { StateRegionAddComponent } from './state-region-add/state-region-add.component';

const routes: Routes = [
  {path:'sateregion',component:StateRegionListComponent},
  {path:'addsateregion',component:StateRegionAddComponent}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StateRegionRoutingModule { }
