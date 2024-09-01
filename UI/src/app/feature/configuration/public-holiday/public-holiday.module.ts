import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { PublicHolidayRoutingModule } from './public-holiday-routing.module';
import { PublicHolidayAddComponent } from './public-holiday-add/public-holiday-add.component';
import { PublicHolidayListComponent } from './public-holiday-list/public-holiday-list.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import {MatFormFieldModule} from '@angular/material/form-field';
import { NgSelectModule } from '@ng-select/ng-select';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    PublicHolidayAddComponent,
    PublicHolidayListComponent
  ],
  imports: [
    CommonModule,
    PublicHolidayRoutingModule,
    SharedModule,
    HttpClientModule,
    FormsModule,MatFormFieldModule,NgSelectModule,
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
export class PublicHolidayModule { }
