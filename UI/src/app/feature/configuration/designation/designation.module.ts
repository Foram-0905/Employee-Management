import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DesignationRoutingModule } from './designation-routing.module';
import { DesignationAddComponent } from './designation-add/designation-add.component';
import { DesignationListComponent } from './designation-list/designation-list.component';

import { BeonGridComponent } from '../../../shared/component/beon-grid/beon-grid.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule, NgModel } from '@angular/forms';

import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from '../../../shared/shared.module';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}

@NgModule({
  declarations: [
    DesignationAddComponent,
    DesignationListComponent
  ],
  imports: [
    CommonModule,
    DesignationRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    SharedModule,
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
export class DesignationModule { }
