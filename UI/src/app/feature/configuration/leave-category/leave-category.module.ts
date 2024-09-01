import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { LeaveCategoryRoutingModule } from './leave-category-routing.module';
import { LeaveCategoryAddComponent } from './leave-category-add/leave-category-add.component';
import { LeaveCategoryListComponent } from './leave-category-list/leave-category-list.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [

    LeaveCategoryListComponent
  ],
  imports: [
    CommonModule,
    LeaveCategoryRoutingModule, SharedModule,FormsModule,
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
export class LeaveCategoryModule { }
