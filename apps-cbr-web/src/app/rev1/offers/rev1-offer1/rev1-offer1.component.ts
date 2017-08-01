import { NgForm } from '@angular/forms';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Component, Inject, OnInit, AfterViewChecked, ViewChild } from '@angular/core';
import { DOCUMENT } from '@angular/platform-browser';

import { CoregCampaignType } from '../../../shared/enums/coreg-campaign-type.enum';
import { ScriptComponent } from '../../../shared/components/script.component';
import { PostService } from '../../../shared/services/post.service';
import { ProvideMediaRequest, ProvideMediaUpdateRequest } from '../../../shared/models/provide-media.model';
import { CoregLead } from '../../../shared/models/coreg-lead.model';
import { CoregCampaignCode } from '../../../shared/models/coreg-campaign-code.model';
import { CoregDisplay } from '../../../shared/models/coreg-display.model';
import { EngageIqRequest } from "../../../shared/models/engage-iq.model";
import { CoregPostResponse } from "../../../shared/models/coreg-post-response.model";



import * as postscribe from 'postscribe';

@Component({
  selector: 'app-rev1-offer1',
  templateUrl: './rev1-offer1.component.html',
  styleUrls: ['./rev1-offer1.component.css']
})
export class Rev1Offer1Component implements OnInit, AfterViewChecked {
  campaignType: CoregCampaignType;


  //private CAMPAIGN_CODE_DEBT_COM = 'wm9EdfezDE8RXU9Rxt21LA';
  //private CAMPAIGN_CODE_SPRING_POWER_GAS = '6lIEmSzGTZ-OW52pU7Ir5g';
  //private CAMPAIGN_CODE_DIRECT_ENERGY = 'yGK2ea4AmDf1fVJbMg05kQ';
  private subIdTag = 'rev1';
  contact: CoregLead;
  campaignCodes: CoregCampaignCode;

  debtCom: CoregDisplay;
  debtComResponse: CoregPostResponse;
  debtComRequest: ProvideMediaRequest;

  directEnergy: CoregDisplay;
  directEnergyResponse: CoregPostResponse;
  directEnergyRequest: ProvideMediaRequest;

  taxotere: CoregDisplay;
  taxotereResponse: CoregPostResponse;
  taxotereRequest: EngageIqRequest;
  //@ViewChild('taxotereform') contactform: NgForm;

  railroadCancer: CoregDisplay;
  railroadCancerResponse: CoregPostResponse;
  railroadCancerRequest: EngageIqRequest;
 // @ViewChild('railroadCancerform') contactform: NgForm;

  herniaMesh: CoregDisplay;
  herniaMeshResponse: CoregPostResponse;
  herniaMeshRequest: EngageIqRequest;


  constructor(private _route: ActivatedRoute, private _router: Router, private _postservice: PostService, @Inject(DOCUMENT) private document: any) {
    this.contact = new CoregLead;

    this.initializeParameters();

    this.campaignCodes = new CoregCampaignCode();

    //only set the prpperties that differ by campaign
    this.debtCom = new CoregDisplay();
    this.debtComResponse = new CoregPostResponse();
    this.debtComRequest = new ProvideMediaRequest();
    this.debtComRequest.campaignCodeId = CoregCampaignType.ProvideMedia_Debtcom;
    this.debtComRequest.campaignCode = this.campaignCodes.ProvideMedia_Debtcom;

    this.directEnergy = new CoregDisplay();
    this.directEnergyResponse = new CoregPostResponse;
    this.directEnergyRequest = new ProvideMediaRequest();
    this.directEnergyRequest.campaignCodeId = CoregCampaignType.ProvideMedia_DirectEnergy;
    this.directEnergyRequest.campaignCode = this.campaignCodes.ProvideMedia_DirectEnergy;

    this.taxotere = new CoregDisplay();
    this.taxotereResponse = new CoregPostResponse;
    this.taxotereRequest = new EngageIqRequest();
    this.taxotereRequest.campaignCodeId = CoregCampaignType.EngageIQ_Taxotere;
    this.taxotereRequest.campaignCode = this.campaignCodes.EngageIQ_Taxotere;

    this.railroadCancer = new CoregDisplay();
    this.railroadCancerResponse = new CoregPostResponse;
    this.railroadCancerRequest = new EngageIqRequest();
    this.railroadCancerRequest.campaignCodeId = CoregCampaignType.EngageIQ_RailroadCancer;
    this.railroadCancerRequest.campaignCode = this.campaignCodes.EngageIQ_RailroadCancer;

    this.herniaMesh = new CoregDisplay();
    this.herniaMeshResponse = new CoregPostResponse;
    this.herniaMeshRequest = new EngageIqRequest();
    this.herniaMeshRequest.campaignCodeId = CoregCampaignType.EngageIQ_HernaMesh;
    this.herniaMeshRequest.campaignCode = this.campaignCodes.EngageIQ_HernaMesh;
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



  engageIqSelected(selected: boolean, offer: CoregDisplay, request: EngageIqRequest, response: CoregPostResponse, form: NgForm, event: any) {

    // if they check NO then restore to original visibilty
    if (!selected) {
      offer.validationOn = false;
      offer.showQuestions = false;
      response.success = true;
      return true;
    }

    //if selected = yes while thequestions are visible
    // then validate form. If valid then submit lead
    if (offer.showQuestions) {

      offer.validationOn = true;

      if (!form.valid) {
        offer.answer = null;
        event.target.checked = false;
        return true;
      }


      return this.engageiqPostData(offer, request, response);
    }

    //clicked yes with the questions hidden so show the questions
    //and uncheck the yes button
    offer.showQuestions = true;
    offer.answer = null;
    event.target.checked = false;

    return true;
  }

  engageiqPostData(offer: CoregDisplay, request: EngageIqRequest, response: CoregPostResponse) {

    request.zip = this.contact.zip;
    request.email = this.contact.email;
    request.cbrLeadId = this.contact.cbrLeadId;
    request.subIdTag = this.subIdTag;

    this._postservice.postEngageIq(request)
      .subscribe((data: CoregPostResponse) => {
        response.success = data.success;
        offer.showUpdate = false;

        if (data.ipIsIrReputable) {
          this._router.navigate(['/campaignComplete']);
        }

        if (data.zipIpVerificationFailed) {
          if (offer.zipMatchAttempts === 0) {
            offer.showUpdate = true;
            response.invalidZip = true;
            offer.zipMatchAttempts = 1;
            return;
          } else {
            this._router.navigate(['/campaignComplete']);
          }
        }

      });
  }

  provideMediaSelected(selected: boolean, offer: CoregDisplay, request: ProvideMediaRequest, response: CoregPostResponse) {
    offer.showConsent = selected;
    if (!selected) {
      offer.showUpdate = false;
      offer.showConsent = false;
      return;
    }

    if (selected) {

      if (!offer.consentChecked) {
        offer.answer = null;  // uncheck yes
        return;
      }

      request.trustedForm = this.getTrustedForm();
      request.zip = this.contact.zip;
      request.email = this.contact.email;
      request.cbrLeadId = this.contact.cbrLeadId;
      request.subIdTag = this.subIdTag;

      offer.showUpdate = false;

      this._postservice.postProvideMedia(request)
        .subscribe((data: CoregPostResponse) => {
          response.success = data.success;

          if (data.ipIsIrReputable) {
            this._router.navigate(['/campaignComplete']);
          }

          if (data.zipIpVerificationFailed) {
            offer.showUpdate = true;
            if (offer.zipMatchAttempts === 0) {
              response.invalidZip = true;
              offer.zipMatchAttempts = 1;
            } else {
              this._router.navigate(['/campaignComplete']);
            }
          }

          if (!data.success && !data.zipIpVerificationFailed) {
            offer.showUpdate = true;
            response.invalidAddress = data.invalidAddress;
            response.invalidZip = data.invalidZip;
            response.invalidPhone = data.invalidPhone;
          }
        });

    }
  }

  provideMediaUpdateData(offer: CoregDisplay, request: ProvideMediaRequest, response: CoregPostResponse) {
    const updateRequest: ProvideMediaUpdateRequest = {
      phone: this.contact.phone,
      address: this.contact.address,
      zip: this.contact.zip,
      retryRequest: request
    };

    this._postservice.postProvideMediaUpdate(updateRequest)
      .subscribe((data: CoregPostResponse) => {
        response.success = data.success;

        if (data.ipIsIrReputable) {
          this._router.navigate(['/campaignComplete']);
        }

        if (data.zipIpVerificationFailed) {
          this._router.navigate(['/campaignComplete']);
        }
        if (!data.success && !data.zipIpVerificationFailed) {
          offer.showUpdate = true;
          response.invalidAddress = data.invalidAddress;
          response.invalidZip = data.invalidZip;
          response.invalidPhone = data.invalidPhone;        }
      });
  }

 


 
}

