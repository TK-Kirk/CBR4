import { NgForm } from '@angular/forms';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Component, Inject, OnInit, AfterViewChecked, ViewChild, DoCheck } from '@angular/core';
import { DOCUMENT } from '@angular/platform-browser';

import { CoregCampaignType } from '../../../shared/enums/coreg-campaign-type.enum';
import { ScriptComponent } from '../../../shared/components/script.component';
import { PostService } from '../../../shared/services/post.service';
import { ProvideMediaRequest, ProvideMediaUpdateRequest } from '../../../shared/models/provide-media.model';
import { CoregLead } from '../../../shared/models/coreg-lead.model';
import { CoregCampaignCode } from '../../../shared/models/coreg-campaign-code.model';
import { CoregDisplay } from '../../../shared/models/coreg-display.model';
import { EngageIqRequest } from '../../../shared/models/engage-iq.model';
import { CoregPostResponse } from '../../../shared/models/coreg-post-response.model';
import { CoregPostRequestBase } from '../../../shared/models/coreg-post-request-base.model';




import * as postscribe from 'postscribe';
//import { Rev1Offer1BaseComponent } from './rev1-offer1-base.component';

@Component({
  selector: 'app-rev1-offerq1',
  templateUrl: './rev1-offerq1.component.html',
  styleUrls: ['./rev1-offerq1.component.css']
})
// export class Rev1OfferQ1Component extends Rev1Offer1BaseComponent implements DoCheck {
export class Rev1OfferQ1Component implements OnInit {
  contact: CoregLead;
  campaignType: CoregCampaignType;


  constructor(private _route: ActivatedRoute, private _router: Router, private _postservice: PostService, @Inject(DOCUMENT) private document: any) {
    //super(_route);
    this.contact = new CoregLead;

    this.initializeParameters();

  }
  ngOnInit() {
    //const url = '<script src='http://ldsapi.tmginteractive.com/GenerateQuestionBaseMultiOffer.aspx?Publisher=572055&Placement=11676800&affid=<%= AffiliateId %>&subid=<%= SubId %>&tmg_firstname=<%= First %>&tmg_lastname=<%= Last %>&tmg_email=<%= Email %>&tmg_address=<%= Address %>&tmg_zip=<%= Zip %>&tmg_gender=<%= Gender %>&tmg_dob=<%= Birthdate %>&tmg_phone=<%= Phone %>&Redirect=<%= redirectUrl %>&redirect=' type='text/javascript'></script>'

    const redirect = 'http://devcoreg.cashbackresearch.com';
    const url = 'http://ldsapi.tmginteractive.com/GenerateQuestionBaseMultiOffer.aspx';
    const params = `?Publisher=572055&Placement=11676800&affid=${this.contact.affiliateId}&subid=${this.contact.subId}
&tmg_firstname=${this.contact.firstname}&tmg_lastname=${this.contact.lastname}&tmg_email=${this.contact.email}
&tmg_address=${this.contact.address}&tmg_zip=${this.contact.zip}&tmg_gender=${this.contact.gender}&tmg_dob=${this.contact.birthDate}
&tmg_phone=${this.contact.phone}&redirect=${redirect}`;

    console.log(url + params);



  //postscribe('#mydiv', '<h1>Hello PostScribe</h1>');

    postscribe('#mydiv', `<script src='${url}${params}' type='text/javascript'></script>`);

}



private initializeParameters() {
  this._route
    .queryParams
    .subscribe((params: any): void => {
      this.contact.cbrLeadId = params['cbrid'];
      this.contact.email = params['email'];
      this.contact.firstname = params['firstname'];
      this.contact.lastname = params['lastname'];
      this.contact.address = params['address'];
      this.contact.zip = params['zip'];
      this.contact.gender = params['gender'];
      this.contact.birthDate = new Date(params['birthdate']);
      this.contact.subId = params['subid'];
      this.contact.affiliateId = params['affiliateid'];
      this.contact.offerId = params['offerid'];
      this.contact.countryId = params['country'];

      this.contact.birthDateDay = this.contact.getBirthDateDay();
      this.contact.birthDateMonth = this.contact.getBirthDateMonth();
      this.contact.birthDateYear = this.contact.getBirthDateYear();
    });
}

onContinue() {
  this._router.navigate(['/offersq2']);
}

}

