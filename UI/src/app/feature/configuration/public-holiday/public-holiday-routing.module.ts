import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PublicHolidayListComponent } from './public-holiday-list/public-holiday-list.component';

const routes: Routes = [
  {path:'publicholiday',component:PublicHolidayListComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicHolidayRoutingModule { }
