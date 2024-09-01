import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrgChartComponent } from './org-chart/org-chart.component';

const routes: Routes = [
  {path:'orgchart',component:OrgChartComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrgChartRoutingModule { }
