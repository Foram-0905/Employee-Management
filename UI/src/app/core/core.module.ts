import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// import { AppRoutingModule } from './app-routing.module';

import { MatSortModule } from "@angular/material/sort";
import { CoreRoutingModule } from './core-routing.module';
import { LoginComponent } from './components/auth/login/login.component';
import { PasswordResetComponent } from './components/auth/password-reset/password-reset.component';
import { HeaderComponent } from './components/header/header/header.component';
import { FooterComponent } from './components/footer/footer/footer.component';
import { NavigationComponent } from './components/navigation/navigation/navigation.component';
import { Error403Component } from './components/error-403/error-403/error-403.component';
import { Error401Component } from './components/error-401/error-401/error-401.component';
import { BrowserModule } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { FormsModule ,ReactiveFormsModule,NgModel} from '@angular/forms';
import { MatIconModule } from '@angular/material/icon'; // Import for Material icons
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from '../app.component';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
// import { HttpClientModule } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
 import { JwtInterceptorService } from './interceptor/token.interceptor';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}
//import { HttpClientModule,provideHttpClient } from '@angular/common/http';


@NgModule({
  declarations: [
   LoginComponent,
   PasswordResetComponent,
     HeaderComponent,
     FooterComponent,
   NavigationComponent,
    Error403Component,
    Error401Component,

  ],
  imports: [
    CommonModule,
    CoreRoutingModule,
    FormsModule,
    BrowserModule,
    MatInputModule,
    MatCardModule,
     MatButtonModule ,
    MatIconModule,

    HttpClientModule,
    BrowserAnimationsModule,
    MatExpansionModule,

    MatSortModule,
    ReactiveFormsModule,
     TranslateModule.forRoot(
      {
      loader:{
        provide:TranslateLoader,
        useFactory:HttpLoaderFactory,
        deps:[HttpClient]
      }
    }
    )

  ],
  providers: [
    provideAnimationsAsync(),

  ], exports: [
    NavigationComponent,
    HeaderComponent

  ],
  bootstrap:[AppComponent]
})
export class CoreModule { }
