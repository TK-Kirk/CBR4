import { Http, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { CoregLead } from '../../shared/models/coreg-lead.model';
import { environment } from '../../../environments/environment';

import 'rxjs/add/operator/map';

@Injectable()
export class Rev1Service {
    constructor(private http: Http) {
    }

    getLeads(email: string, offerId: string): Observable<CoregLead> {
        const routeUrl = `${environment.host}api/leads/${email}/${offerId}`;

        return this.http.get(routeUrl)
            .map((res: Response) => {
                const result = res.json();
                return result;
            });
    }



    postLead(lead: CoregLead): Observable<CoregLead> {
        const routeUrl = `${environment.host}api/leads/coreg`;

        return this.http.post(routeUrl, lead)
            .map((res: Response) => {
                const result = res.json();
                return result;
            });
    }





}