import { ActivatedRoute, Data, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { CoregLead } from '../shared/models/coreg-lead.model';
import { ListrakFieldItem, ListrakLists } from '../shared/models/listrak.model';

import { ListrakService } from '../shared/services/listrak.service';


@Component({
    selector: 'app-rev1-enter',
    templateUrl: './rev1-enter.component.html'
})

export class Rev1EnterComponent implements OnInit {
    contact: CoregLead;


    constructor(private _route: ActivatedRoute, private _router: Router, private _listrakService: ListrakService) {
        this.contact = new CoregLead;

        this.initializeParameters();
    }

    ngOnInit(): void { }

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

     onSubmit() {
        // need to do listrak joined soi
        // if (this.contact.offerId === '57048') { }

        const listrakFields: ListrakFieldItem[] = [{ name: 'CBR.EmailAddress', value: this.contact.email },
        { name: 'CBR.joinsoi', value: '1' }];

        // old aspx code only had non cert
        this._listrakService.postAdhoc(listrakFields, [ ListrakLists.CBR_US_Non_Cert], 'rev1enter').subscribe();

        this._router.navigate(['/offers1'], { queryParams: { 'email': this.contact.email, 'firstname': this.contact.firstname, 'lastname': this.contact.lastname, 'address': this.contact.address, 'zip': this.contact.zip, 'gender': this.contact.gender, 'birthdate': this.contact.birthDate, 'country': this.contact.countryId, 'offerid': this.contact.offerId, 'affiliateid': this.contact.affiliateId, 'subid': this.contact.subId, 'cbrid': this.contact.cbrLeadId  } });

    }
}
