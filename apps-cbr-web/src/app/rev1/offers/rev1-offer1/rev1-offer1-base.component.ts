import { ActivatedRoute} from '@angular/router';
import { Component } from '@angular/core';
import { CoregLead } from '../../../shared/models/coreg-lead.model';

@Component({
  selector: 'app-rev1-offer-bass',
})
export class Rev1Offer1BaseComponent {
  contact: CoregLead;
  

  constructor(private _route: ActivatedRoute) {
    this.contact = new CoregLead;

    this.initializeParameters();

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

}

