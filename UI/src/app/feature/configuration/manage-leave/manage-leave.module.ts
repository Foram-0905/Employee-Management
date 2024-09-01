import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ManageLeaveRoutingModule } from './manage-leave-routing.module';
import { ManageLeaveComponent } from './manage-leave/manage-leave.component';
import { SharedModule } from "../../../shared/shared.module";
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}

@NgModule({
    declarations: [
        ManageLeaveComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ManageLeaveRoutingModule,
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
export class ManageLeaveModule { }
