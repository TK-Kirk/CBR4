import { BrowserModule } from '@angular/platform-browser';
import { Http, HttpModule, RequestOptions, XHRBackend } from '@angular/http';
import { NgModule, ViewContainerRef } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { APP_INITIALIZER } from '@angular/core';
import { CookieModule } from 'ngx-cookie';

import { Rev1Module } from './rev1/rev1.module';
import { Rev1RoutingModule } from './rev1/rev1-routing.module';



import { AppComponent } from './app.component';
import { routing } from './app.routes';

@NgModule({
    declarations: [
        AppComponent,
    ],
    imports: [
        BrowserModule,
        CookieModule.forRoot(),
        BrowserAnimationsModule,
        HttpModule,
        routing,
        Rev1Module,
        Rev1RoutingModule
    ],
    bootstrap: [AppComponent],
    providers: [
    ]
})
export class AppModule {
}
