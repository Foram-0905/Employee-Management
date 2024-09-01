import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EducationLevelRoutingModule } from './education-level-routing.module';
import { EducationLevelAddComponent } from './education-level-add/education-level-add.component';
import { EducationLevelListComponent } from './education-level-list/education-level-list.component';

import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from "../../../shared/shared.module";
import { FormsModule } from '@angular/forms';

export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}

@NgModule({
    declarations: [
        EducationLevelAddComponent,
        EducationLevelListComponent
    ],
    imports: [
        CommonModule,
        EducationLevelRoutingModule,
        FormsModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        }),
        SharedModule
    ]
})
export class EducationLevelModule { }
