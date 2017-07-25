import { ActivatedRoute, Data, Router } from '@angular/router';
import { Component, Inject, OnInit, AfterViewChecked } from '@angular/core';
import { DOCUMENT } from '@angular/platform-browser';

import { CoregLead } from '../../../shared/models/coreg-lead.model';
import { ScriptComponent } from '../../../shared/components/script.component';
import { PostService } from '../../../shared/services/post.service';
import { ProvideMediaResponse, ProvideMediaRequest, ProvideMediaUpdateRequest } from '../../../shared/models/provide-media.model';


import * as postscribe from 'postscribe';

@Component({
  selector: 'app-rev1-offer1',
  templateUrl: './rev1-offer1.component.html',
  styleUrls: ['./rev1-offer1.component.css']
})
export class Rev1Offer1Component implements OnInit, AfterViewChecked {
  private CAMPAIGN_CODE_DEBT_COM = 'wm9EdfezDE8RXU9Rxt21LA';
  private CAMPAIGN_CODE_SPRING_POWER_GAS = '6lIEmSzGTZ-OW52pU7Ir5g';
  private CAMPAIGN_CODE_DIRECT_ENERGY = 'yGK2ea4AmDf1fVJbMg05kQ';
  private subIdTag = 'rev1';
  contact: CoregLead;

  // debt.com
  debtcomAnswer: string;
  showDebtComConsent = false;
  debtComConsentChecked = false;
  showDebtComUpdate: boolean;
  showDebtComZip: boolean;
  showDebtComAddress: boolean;
  showDebtComPhone: boolean;
  debtComMessage: string;

  constructor(private _route: ActivatedRoute, private _router: Router, private _postservice: PostService, @Inject(DOCUMENT) private document: any) {
    this.contact = new CoregLead;

    this.initializeParameters();
  }


  ngOnInit() {
    // this.trustedform = this.document.getElementsByName('xxTrustedFormCertUrl').value;
  }

  ngAfterViewChecked() {
    // this.trustedform =   this.document.getElementById('xxTrustedFormCertUrl_0').value;
  }

  getTrustedForm() {
    // return  this.document.getElementById('xxTrustedFormCertUrl_0').value;
    return this.document.getElementsByName('xxTrustedFormCertUrl')[0].value;
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


  debtComSelection(selected: boolean) {
    this.showDebtComConsent = selected;
    if (!selected) {
      this.showDebtComUpdate = false;
      return;
    }

    if (selected) {

      if (!this.debtComConsentChecked) {
        this.debtcomAnswer = null;  // uncheck yes
        return;
      }

      // const tf = this.document.getElementsByName('xxTrustedFormCertUrl').value;
      const tf: string = this.getTrustedForm();

      const request: ProvideMediaRequest = {
        trustedForm: tf, campaignCode: this.CAMPAIGN_CODE_DEBT_COM,
        subIdTag: this.subIdTag, cbrLeadId: this.contact.cbrLeadId
      };


      this._postservice.postProvideMedia(request)
        .subscribe((data: ProvideMediaResponse) => {
          if (!data.success && data.other !== 'Failed zip/ip verifiction.') {
            this.showDebtComUpdate = true;
            this.showDebtComPhone = data.invalidPhone;
            this.showDebtComAddress = data.invalidAddress;
            this.showDebtComZip = data.invalidZip;
            this.debtComMessage = data.message;
          }
        });

    }
  }


  updateData() {
    const tf: string = this.getTrustedForm();
    const request: ProvideMediaUpdateRequest = {
      phone: this.contact.phone,
      address: this.contact.address,
      zip: this.contact.zip,
      retryRequest: {
        trustedForm: tf, campaignCode: this.CAMPAIGN_CODE_DEBT_COM,
        subIdTag: this.subIdTag, cbrLeadId: this.contact.cbrLeadId
      }
    };

    this._postservice.postProvideMediaUpdate(request)
      .subscribe((data: ProvideMediaResponse) => {
        if (!data.success && data.other !== 'Failed zip/ip verifiction.') {
          //this.showDebtComPhone = data.invalidPhone;
          //this.showDebtComAddress = data.invalidAddress;
          //this.showDebtComZip = data.invalidZip;
        }
        this.showDebtComUpdate = false;
      });
  }


}
