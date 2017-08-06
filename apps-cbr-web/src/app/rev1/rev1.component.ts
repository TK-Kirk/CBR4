import { Component, ViewChild, OnInit, Renderer2 } from '@angular/core';
import { Rev1Service } from './services/rev1.service';
import { CoregLead } from '../shared/models/coreg-lead.model';
import { DOCUMENT } from '@angular/platform-browser';
import { Inject, Injectable } from '@angular/core';
import { ScriptComponent } from '../shared/components/script.component';



@Component({
    selector: 'rev1',
    templateUrl: 'rev1.component.html'
})
export class Rev1Component {
    private lead: CoregLead;
    email = 'tk07082017.10@webhenmedia.com';
    offerId = '51001';



    constructor(private renderer2: Renderer2, @Inject(DOCUMENT) private _document: any, private _rev1Service: Rev1Service) {
    }

    onClick() {
        this.onGetLead(this.email, this.offerId);
    }

    private onGetLead(email: string, offerId: string) {
        this._rev1Service.getLeads(email, offerId)
            .subscribe((data: CoregLead) => {
                this.lead = data;
            });
    }

}
