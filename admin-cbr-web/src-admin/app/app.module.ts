import { BrowserModule } from '@angular/platform-browser';
import { Http, HttpModule, RequestOptions, XHRBackend } from '@angular/http';
import { NgModule, ViewContainerRef } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { APP_INITIALIZER } from '@angular/core';

import { CoregModule } from './coreg/coreg.module';



import { AppComponent } from './app.component';
import { routing } from './app.routes';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpModule,
    routing,
    CoregModule

  ],
  bootstrap: [AppComponent],
  providers: [
  ]
})
export class AppModule {
}
