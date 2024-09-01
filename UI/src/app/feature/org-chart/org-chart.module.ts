import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrgChartRoutingModule } from './org-chart-routing.module';
import { OrgChartComponent } from './org-chart/org-chart.component';
import { OrganizationChartModule } from 'primeng/organizationchart';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
import { TreeModule } from 'primeng/tree';
import { FullCalendarModule } from '@fullcalendar/angular';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}
@NgModule({
  declarations: [
    OrgChartComponent
  ],
  imports: [
    CommonModule,
    TreeModule,
    OrgChartRoutingModule,
    OrganizationChartModule,
    TranslateModule.forRoot(
      {
      loader:{
        provide:TranslateLoader,
        useFactory:HttpLoaderFactory,
        deps:[HttpClient]
      }
    }
    )

  ]
})
export class OrgChartModule { }
