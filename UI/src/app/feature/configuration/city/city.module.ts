import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CityRoutingModule } from './city-routing.module';
//  import { CityAddComponent } from './city-add/city-add.component';
 import { HttpClient } from '@angular/common/http';
 import { HttpClientModule } from '@angular/common/http';
import { CityListComponent } from './city-list/city-list.component';
import { FormsModule } from '@angular/forms';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from '../../../shared/shared.module';
import { ActionButtonsRendererComponent } from '../../../shared/component/beon-grid/actionButtonsRendererComponent.component';
import { AgGridModule } from 'ag-grid-angular';
import { NgSelectModule } from '@ng-select/ng-select';

// import { FormsModule } from '@angular/forms';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}
@NgModule({
  declarations: [
    CityListComponent,
    // CityAddComponent,
   // ActionButtonsRendererComponent
  ],
  imports: [
    CommonModule,
    CityRoutingModule,
    NgSelectModule,
    FormsModule, HttpClientModule , SharedModule,AgGridModule,
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
export class CityModule { }
