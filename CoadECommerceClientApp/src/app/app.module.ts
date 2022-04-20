import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';

import { LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localePtBr from '@angular/common/locales/pt';
import { BaseModule } from './base/base.module';
import { LojaVitrineModule } from './loja-vitrine/loja-vitrine.module';
import { BusinessModule } from './business/business.module';

import { AuthModule } from './auth/auth.module';
import { LoginComponent } from './login/login.component';
import { LoaderHttpInterceptor } from './base/interceptors/loader-http-interceptor';
import { AuthenticationInterceptor } from './base/interceptors/authentication-interceptor';
import { AppRouterModule } from './app-router/app-router.module';

registerLocaleData(localePtBr);

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    BaseModule,
    BusinessModule,
    LojaVitrineModule,
    AuthModule,
    AppRouterModule
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt' },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoaderHttpInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
