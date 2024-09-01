import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LeaveTypeRoutingModule } from './leave-type-routing.module';
import { LeaveTypeAddComponent } from './leave-type-add/leave-type-add.component';
import { LeaveTypeListComponent } from './leave-type-list/leave-type-list.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient,HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';

export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http);
}
@NgModule({
  declarations: [
    LeaveTypeAddComponent,
    LeaveTypeListComponent
  ],
  imports: [
    CommonModule,
    LeaveTypeRoutingModule,
    HttpClientModule,
    SharedModule,
    FormsModule,
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
export class LeaveTypeModule { }
