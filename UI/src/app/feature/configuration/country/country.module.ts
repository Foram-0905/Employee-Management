import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CountryRoutingModule } from './country-routing.module';
import { CountryAddComponent } from './country-add/country-add.component';
import { CountryListComponent } from './country-list/country-list.component';
import { FormsModule } from '@angular/forms';
import { BeonGridComponent } from '../../../shared/component/beon-grid/beon-grid.component';
import { AgGridModule } from 'ag-grid-angular';
import { SharedModule } from '../../../shared/shared.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ActionButtonsRendererComponent } from '../../../shared/component/beon-grid/actionButtonsRendererComponent.component';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}

@NgModule({
  declarations: [
   CountryAddComponent,
   CountryListComponent,

   ActionButtonsRendererComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    CountryRoutingModule,
    SharedModule,
    AgGridModule,
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
export class CountryModule { }
