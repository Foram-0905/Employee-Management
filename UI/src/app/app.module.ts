import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, NgModel } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MatIcon } from '@angular/material/icon';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
// import { HttpClient, HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import { CurrencyAddComponent } from './feature/configuration/currency/currency-add/currency-add.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { BeonGridComponent } from './shared/component/beon-grid/beon-grid.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatSortModule } from "@angular/material/sort";
import { LoginComponent } from './core/components/auth/login/login.component';
import { CoreModule } from './core/core.module';
import { JwtInterceptorService } from './core/interceptor/token.interceptor';
import { FeatureRoutingModule } from './feature/feature-routing.module';
import { DashboardComponent } from './feature/dashboard/dashboard.component';
import { core } from '@angular/compiler';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";

export function HttpLoaderFactory(http:HttpClient):TranslateHttpLoader{
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}


@NgModule({
  declarations: [
    AppComponent ,
    DashboardComponent,

  ],

  imports: [
    CoreModule,
   CoreModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    HttpClientModule,
    FormsModule,
    MatSortModule,
    MatIcon,
    MatIconModule,
    MatGridListModule,
    NgxSpinnerModule,
    ReactiveFormsModule,
    TranslateModule.forRoot(
      {
        loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
        }
      }
    ),
    ToastrModule.forRoot({
      progressBar : true
    })
  ],


  providers: [

    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([JwtInterceptorService]))
    
  ],


  bootstrap: [AppComponent]
})
export class AppModule { }
