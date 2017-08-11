import { Http, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { CoregCampaignDetail } from '../../shared/models/coreg-campaign-detail.model';
import { environment } from '../../../environments/environment';

import 'rxjs/add/operator/map';

@Injectable()
export class CoregService {
    constructor(private http: Http) {
    }

    getCampaigns(): Observable<CoregCampaignDetail[]> {
        const routeUrl = `${environment.host}api/coreg/campaigns`;

        return this.http.get(routeUrl)
            .map((res: Response) => {
                const result = res.json();
                return result;
            });
    }

  setCampaignActive(selected: boolean, coregCampaignDetail: CoregCampaignDetail) {
    const routeUrl = `${environment.host}api/coreg/campaigns/${coregCampaignDetail.coregCampaignId}/active/${selected}`;

    return this.http.put(routeUrl, null)
      .map((res: Response) => {
        const result = res.json();
        return result;
      });
  }
}
