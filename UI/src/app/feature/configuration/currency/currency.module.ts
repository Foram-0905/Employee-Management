import { NgModule } from '@angular/core';
import { NgModel} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CurrencyRoutingModule } from './currency-routing.module';
import { CurrencyAddComponent } from './currency-add/currency-add.component';
import { CurrencyListComponent } from './currency-list/currency-list.component';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../../../shared/shared.module';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    CurrencyAddComponent,
    CurrencyListComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,SharedModule,
    CurrencyRoutingModule,
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
export class CurrencyModule { }
