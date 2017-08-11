import { NgForm } from '@angular/forms';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Component, Inject, OnInit, AfterViewChecked, ViewChild } from '@angular/core';
import { DOCUMENT } from '@angular/platform-browser';

import { CoregCampaignType } from '../../../shared/enums/coreg-campaign-type.enum';
import { ScriptComponent } from '../../../shared/components/script.component';
import { CoregService } from '../../../shared/services/coreg.service';
import { PostService } from '../../../shared/services/post.service';
import { ProvideMediaRequest, ProvideMediaUpdateRequest } from '../../../shared/models/provide-media.model';
import { CoregLead } from '../../../shared/models/coreg-lead.model';
import { CoregCampaignCode } from '../../../shared/models/coreg-campaign-code.model';
import { CoregDisplay } from '../../../shared/models/coreg-display.model';
import { EngageIqRequest } from '../../../shared/models/engage-iq.model';
import { CoregPostResponse } from '../../../shared/models/coreg-post-response.model';
import { CoregPostRequestBase } from '../../../shared/models/coreg-post-request-base.model';
import { environment } from '../../../../environments/environment';



import * as postscribe from 'postscribe';
import { CoregCampaignDetail } from '../../../shared/models/coreg-campaign-detail.model'

@Component({
  selector: 'app-rev1-offer1',
  templateUrl: './rev1-offer1.component.html',
  styleUrls: ['./rev1-offer1.component.css']
})
export class Rev1Offer1Component implements OnInit, AfterViewChecked {
  campaignType: CoregCampaignType;
  campaigns: CoregCampaignDetail[];

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

  railroadCancer: CoregDisplay;
  railroadCancerResponse: CoregPostResponse;
  railroadCancerRequest: EngageIqRequest;

  herniaMesh: CoregDisplay;
  herniaMeshResponse: CoregPostResponse;
  herniaMeshRequest: EngageIqRequest;

  xarelto: CoregDisplay;
  xareltoResponse: CoregPostResponse;
  xareltoRequest: EngageIqRequest;

  backbrace: CoregDisplay;
  backbraceResponse: CoregPostResponse;
  backbraceRequest: EngageIqRequest;

  medicalAlert: CoregDisplay;
  medicalAlertResponse: CoregPostResponse;
  medicalAlertRequest: EngageIqRequest;

  paingel: CoregDisplay;
  paingelResponse: CoregPostResponse;
  paingelRequest: EngageIqRequest;

  motorVehicleAccident: CoregDisplay;
  motorVehicleAccidentResponse: CoregPostResponse;
  motorVehicleAccidentRequest: EngageIqRequest;

  toluna: CoregDisplay;
  tolunaResponse: CoregPostResponse;
  tolunaRequest: EngageIqRequest;


  sprint: CoregDisplay;
  sprintResponse: CoregPostResponse;
  sprintRequest: EngageIqRequest;


  constructor(private _route: ActivatedRoute,
    private _router: Router,
    private _postservice: PostService,
    private _coregservice: CoregService,
    @Inject(DOCUMENT) private document: any) {

    this.contact = new CoregLead;
    this.campaigns = [];

    this.getCampaigns();
    this.initializeParameters();

    this.campaignCodes = new CoregCampaignCode();

    // only set the prpperties that differ by campaign
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

    this.xarelto = new CoregDisplay();
    this.xareltoResponse = new CoregPostResponse;
    this.xareltoRequest = new EngageIqRequest();
    this.xareltoRequest.campaignCodeId = CoregCampaignType.EngageIQ_Xarelto;
    this.xareltoRequest.campaignCode = this.campaignCodes.EngageIQ_Xarelto;

    this.backbrace = new CoregDisplay();
    this.backbraceResponse = new CoregPostResponse;
    this.backbraceRequest = new EngageIqRequest();
    this.backbraceRequest.campaignCodeId = CoregCampaignType.EngageIQ_BackBrace;
    this.backbraceRequest.campaignCode = this.campaignCodes.EngageIQ_BackBrace;

    this.medicalAlert = new CoregDisplay();
    this.medicalAlertResponse = new CoregPostResponse;
    this.medicalAlertRequest = new EngageIqRequest();
    this.medicalAlertRequest.campaignCodeId = CoregCampaignType.EngageIQ_MedicalAlert;
    this.medicalAlertRequest.campaignCode = this.campaignCodes.EngageIQ_MedicalAlert;

    this.paingel = new CoregDisplay();
    this.paingelResponse = new CoregPostResponse;
    this.paingelRequest = new EngageIqRequest();
    this.paingelRequest.campaignCodeId = CoregCampaignType.EngageIQ_PainGel;
    this.paingelRequest.campaignCode = this.campaignCodes.EngageIQ_PainGel;

    this.motorVehicleAccident = new CoregDisplay();
    this.motorVehicleAccidentResponse = new CoregPostResponse;
    this.motorVehicleAccidentRequest = new EngageIqRequest();
    this.motorVehicleAccidentRequest.campaignCodeId = CoregCampaignType.EngageIQ_MotorVehicleAccident;
    this.motorVehicleAccidentRequest.campaignCode = this.campaignCodes.EngageIQ_MotorVehicleAccident;

    this.toluna = new CoregDisplay();
    this.tolunaResponse = new CoregPostResponse;
    this.tolunaRequest = new EngageIqRequest();
    this.tolunaRequest.campaignCodeId = CoregCampaignType.EngageIQ_Toluna;
    this.tolunaRequest.campaignCode = this.campaignCodes.EngageIQ_Toluna;

    this.sprint = new CoregDisplay();
    this.sprintResponse = new CoregPostResponse;
    this.sprintRequest = new EngageIqRequest();
    this.sprintRequest.campaignCodeId = CoregCampaignType.Centerfield_Sprint;
    this.sprintRequest.campaignCode = this.campaignCodes.Centerfield_Sprint;

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

  offerIsHidden(id: CoregCampaignType): boolean {
    if (this.campaigns.length === 0) {
      return false;
    }
    const c = this.campaigns.find(x => x.coregCampaignId === id);
    if (c == undefined) {
      return false;
    }
    return !c.active;
  }

  getCampaigns() {
    this._coregservice.getCampaigns()
      .subscribe((data: CoregCampaignDetail[]) => {
        this.campaigns = data;
      });
  }

  engageIqSelected(selected: boolean,
    offer: CoregDisplay,
    request: EngageIqRequest,
    response: CoregPostResponse,
    form: NgForm,
    event: any) {

    // if they check NO then restore to original visibilty
    if (!selected) {
      offer.validationOn = false;
      offer.showQuestions = false;
      response.success = false;
      return true;
    }

    // if selected = yes while thequestions are visible
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

    // clicked yes with the questions hidden so show the questions
    // and uncheck the yes button
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

  centerfieldMediaSelected(selected: boolean,
    offer: CoregDisplay,
    request: CoregPostRequestBase,
    response: CoregPostResponse,
    form: NgForm,
    event: any) {

    // if they check NO then restore to original visibilty
    if (!selected) {
      offer.validationOn = false;
      offer.showQuestions = false;
      response.success = false;
      return true;
    }

    // if selected = yes while thequestions are visible
    // then validate form. If valid then submit lead
    if (offer.showQuestions) {

      offer.validationOn = true;

      if (!form.valid) {
        offer.answer = null;
        event.target.checked = false;
        return true;
      }


      return this.centerfieldMediaPostData(offer, request, response);
    }

    // clicked yes with the questions hidden so show the questions
    // and uncheck the yes button
    offer.showQuestions = true;
    offer.answer = null;
    event.target.checked = false;

    return true;
  }

  centerfieldMediaPostData(offer: CoregDisplay, request: CoregPostRequestBase, response: CoregPostResponse) {

    request.zip = this.contact.zip;
    request.email = this.contact.email;
    request.cbrLeadId = this.contact.cbrLeadId;
    request.subIdTag = this.subIdTag;

    this._postservice.postCenterfieldMedia(request)
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

  provideMediaSelected(selected: boolean,
    offer: CoregDisplay,
    request: ProvideMediaRequest,
    response: CoregPostResponse) {
    offer.showConsent = selected;
    if (!selected) {
      offer.showUpdate = false;
      offer.showConsent = false;
      return;
    }

    if (selected) {

      if (!offer.consentChecked) {
        offer.answer = null; // uncheck yes
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
          response.invalidPhone = data.invalidPhone;
        }
      });
  }

  onContinue() {
    //this._router.navigate(['/offersq1']);

    //this._router.navigate([`${environment.cbrURL}/SoiPath/Q1.aspx`], { queryParams: { 'email': this.contact.email, 
    //'firstname': this.contact.firstname, 'lastname': this.contact.lastname, 'address': this.contact.address, 
    //'zip': this.contact.zip, 'gender': this.contact.gender, 'birthdate': this.contact.birthDate, 'country': this.contact.countryId,
    //'offerid': this.contact.offerId, 'affiliateid': this.contact.affiliateId, 'subid': this.contact.subId, 'cbrid': this.contact.cbrLeadId

    const c = this.contact;

    //http://www.cashbackresearch.com/SoiPath/Q2.aspx?firstname=Thomas&lastname=Kirk&email=tk08062017.1@webhenmedia.com
    //&offerid=51001&affiliateid=00000&subid=&country=US&address=132%20Street
    //&zip=68135&gender=M&dob=1/3/2001

    var gender = 'M';
    if (this.contact.gender === 'female') {
      gender = 'F';
    }

    const dob = `${this.contact.getBirthDateMonth()}/${this.contact.getBirthDateDay()}/${this.contact.getBirthDateYear()}`;

    window.location.href = `${environment.cbrURL}/SoiPath/Q1.aspx?email=${c.email}&firstname=${c.firstname}
&lastname=${c.lastname}&address=${c.address}&zip=${c.zip}&gender=${gender}&dob=${dob}
&country=${c.countryId}&offerid=${c.offerId}&affiliate=${c.affiliateId}&subid=${c.subId}&cbrid=${c.cbrLeadId}`;


  }

}

