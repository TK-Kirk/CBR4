﻿import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { Rev1Component } from './rev1.component';
import { Rev1HeaderComponent } from './rev1-header.component';
import { Rev1FooterComponent } from './rev1-footer.component';
import { Rev1SignupComponent } from './rev1-signup.component';
import { Rev1EnterComponent } from './rev1-enter.component';
import { Rev1RejectComponent } from './rev1-reject.component';
import { Rev1Offer1Component } from './offers/rev1-offer1/rev1-offer1.component';
import { Rev1OfferQ1Component } from './offers/rev1-offer1/rev1-offerq1.component';
//import { Rev1Offer1BaseComponent } from './offers/rev1-offer1/rev1-offer1-base.component';
import { TiburonContentComponent } from './tiburon-content';
import { Rev1Service } from './services/rev1.service';
import { ScriptComponent } from '../shared/components/script.component';
import { Rev1RoutingModule, rev1Routes } from './rev1-routing.module';

import { CoregService } from '../shared/services/coreg.service';
import { PostService } from '../shared/services/post.service';
import { XVerifyService } from '../shared/services/xverify.service';
import { ListrakService } from '../shared/services/listrak.service';


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
    Rev1Offer1Component,
    Rev1OfferQ1Component,
    Rev1RejectComponent,
    TiburonContentComponent
  ],
  providers: [
    Rev1Service, XVerifyService, ListrakService, PostService, CoregService
  ],
  exports: [
    Rev1Component,
    ScriptComponent,
    Rev1RoutingModule
  ]
})
export class Rev1Module {

}
