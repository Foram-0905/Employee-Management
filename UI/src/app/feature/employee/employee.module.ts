import { NgModule } from '@angular/core'; 
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}
import { EmployeeRoutingModule } from './employee-routing.module';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    SharedModule,FormsModule,
    EmployeeRoutingModule,
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
export class EmployeeModule { }
