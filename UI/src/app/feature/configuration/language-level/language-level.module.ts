import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LanguageLevelRoutingModule } from './language-level-routing.module';
import { LanguageLevelAddComponent } from './language-level-add/language-level-add.component';
import { LanguageLevelListComponent } from './language-level-list/language-level-list.component';

import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from "../../../shared/shared.module";
import {ToastrModule, ToastrService} from 'ngx-toastr'

export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}

@NgModule({
    declarations: [
        LanguageLevelAddComponent,
        LanguageLevelListComponent
    ],
    imports: [
        CommonModule,
        LanguageLevelRoutingModule,
        FormsModule,
        HttpClientModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        }),
        SharedModule,
        ToastrModule.forRoot({
            closeButton : true,
            timeOut: 5000,
            positionClass: 'toast-top-center',
            progressBar : true
          })
    ]
})
export class LanguageLevelModule { }
