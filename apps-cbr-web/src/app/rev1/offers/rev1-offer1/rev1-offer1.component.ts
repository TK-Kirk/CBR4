import { ActivatedRoute, Data, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { CoregLead } from '../../../shared/models/coreg-lead.model';


@Component({
  selector: 'app-rev1-offer1',
  templateUrl: './rev1-offer1.component.html',
  styleUrls: ['./rev1-offer1.component.css']
})
export class Rev1Offer1Component implements OnInit {

    contact: CoregLead;

    constructor(private _route: ActivatedRoute, private _router: Router) {
        this.contact = new CoregLead;

        this.initializeParameters();
    }


  ngOnInit() {
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
