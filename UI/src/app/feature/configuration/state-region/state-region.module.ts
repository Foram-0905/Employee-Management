import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StateRegionRoutingModule } from './state-region-routing.module';
import { StateRegionAddComponent } from './state-region-add/state-region-add.component';
import { StateRegionListComponent } from './state-region-list/state-region-list.component';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from "../../../shared/shared.module";
import { NgSelectModule } from '@ng-select/ng-select';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}

@NgModule({
    declarations: [
        StateRegionAddComponent,
        StateRegionListComponent
    ],
    imports: [
        CommonModule,NgSelectModule,
        StateRegionRoutingModule,
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
export class StateRegionModule { }
