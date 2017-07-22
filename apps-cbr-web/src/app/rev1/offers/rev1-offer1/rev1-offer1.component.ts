import { ActivatedRoute, Data, Router } from '@angular/router';
import { Component, Inject, OnInit, AfterViewChecked } from '@angular/core';

import { CoregLead } from '../../../shared/models/coreg-lead.model';
import { ScriptComponent } from '../../../shared/components/script.component';

import { DOCUMENT } from '@angular/platform-browser';

import * as postscribe from 'postscribe';

@Component({
  selector: 'app-rev1-offer1',
  templateUrl: './rev1-offer1.component.html',
  styleUrls: ['./rev1-offer1.component.css']
})
export class Rev1Offer1Component implements OnInit, AfterViewChecked {

    contact: CoregLead;
    trustedform: string;
    constructor(private _route: ActivatedRoute, private _router: Router, @Inject(DOCUMENT) private document: any) {
        this.contact = new CoregLead;

        this.initializeParameters();
    }


  ngOnInit() {
        // this.trustedform = this.document.getElementsByName('xxTrustedFormCertUrl').value;
  }

  ngAfterViewChecked() {
        // this.trustedform =   this.document.getElementById('xxTrustedFormCertUrl_0').value;
  }

getTrustedForm(){
    this.trustedform =   this.document.getElementById('xxTrustedFormCertUrl_0').value;
}

    private initializeParameters() {
        this._route
            .queryParams
            .subscribe((params: any): void => {
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
}
