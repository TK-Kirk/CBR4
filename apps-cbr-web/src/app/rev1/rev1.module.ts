import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { Rev1Component } from './rev1.component';
import { Rev1HeaderComponent } from './rev1-header.component';
import { Rev1FooterComponent } from './rev1-footer.component';
import { Rev1SignupComponent } from './rev1-signup.component';
import { Rev1EnterComponent } from './rev1-enter.component';
import { Rev1OffersComponent } from './rev1-offers.component';
import { Rev1Service } from './services/rev1.service';
import { ScriptComponent } from '../shared/components/script.component';
import { Rev1RoutingModule, rev1Routes } from './rev1-routing.module';

import { XVerifyService } from '../shared/services/xverify.service';
import { ListrakService } from '../shared/services/listrak.service';
import { Rev1Offer1Component } from './offers/rev1-offer1/rev1-offer1.component';


export { Rev1Component };


@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        Rev1RoutingModule,
        RouterModule.forChild(rev1Routes)
    ],
    declarations: [
        Rev1Component,
        Rev1HeaderComponent,
        Rev1FooterComponent,
        Rev1SignupComponent,
        ScriptComponent,
        Rev1RoutingModule.components,
        Rev1Offer1Component
    ],
    providers: [
        Rev1Service, XVerifyService, ListrakService
    ],
    exports: [
        Rev1Component,
        ScriptComponent,
        Rev1RoutingModule
    ]
})
export class Rev1Module {

}