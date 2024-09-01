import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SlgRoutingModule } from './slg-routing.module';
import { SlgAddComponent } from './slg-add/slg-add.component';
import { SlgListComponent } from './slg-list/slg-list.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from '../../../shared/shared.module';
import {ToastrModule, ToastrService} from 'ngx-toastr'


export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}

@NgModule({
  declarations: [
    SlgAddComponent,
    SlgListComponent,
  ],
 
  imports: [
    CommonModule,
    SharedModule ,
    SlgRoutingModule,
    FormsModule,
    HttpClientModule,
    TranslateModule.forRoot(
      {
      loader:{
        provide:TranslateLoader,
        useFactory:HttpLoaderFactory,
        deps:[HttpClient]
      }
    }
    ),
    ToastrModule.forRoot({
      closeButton : true,
      timeOut: 5000,
      positionClass: 'toast-top-center',
      progressBar : true
    })
  ]
})
export class SlgModule { }
