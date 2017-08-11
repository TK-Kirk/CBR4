import { NgForm } from '@angular/forms';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';


import { CoregService } from './services/coreg.service';
import { CoregCampaignDetail } from '../shared/models/coreg-campaign-detail.model';


@Component({
    selector: 'campaigns',
    templateUrl: './campaigns.component.html'
})

export class CampaignsComponent implements OnInit {
  campaigns: CoregCampaignDetail[];


  constructor(private _route: ActivatedRoute, private _router: Router, private _coregService: CoregService) {
        this.campaigns = [];

    }

    ngOnInit(): void {
      this.loadCampaigns();
    }

  loadCampaigns() {
    this._coregService.getCampaigns()
      .subscribe((data: CoregCampaignDetail[]) => {
        this.campaigns = data;
      });
  }

  onRadioClick(selected: boolean, item: CoregCampaignDetail) {
    this._coregService.setCampaignActive(selected, item)
      .subscribe(() => {
        item.active = selected;
      });
  }

}
