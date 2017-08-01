import { Http, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { CoregLead } from '../../shared/models/coreg-lead.model';
import { ProvideMediaRequest, ProvideMediaUpdateRequest } from '../../shared/models/provide-media.model';
import { environment } from '../../../environments/environment';
import { EngageIqRequest } from "../models/engage-iq.model";
import { CoregPostResponse } from "../models/coreg-post-response.model";

import 'rxjs/add/operator/map';

@Injectable()
export class PostService {
  postUrl: string;

  constructor(private http: Http) {
    this.postUrl = `${environment.host}api/post/`;

  }

  postEngageIq(request: EngageIqRequest): Observable<CoregPostResponse> {
    const routeUrl = `${this.postUrl}engageiq`;

    return this.http.post(routeUrl, request)
      .map((res: Response) => {
        const result = res.json();
        return result;
      });
  }


  postProvideMedia(request: ProvideMediaRequest): Observable<CoregPostResponse> {
      const routeUrl = `${this.postUrl}providemedia`;

        return this.http.post(routeUrl, request)
            .map((res: Response) => {
                const result = res.json();
                return result;
            });
    }


  postProvideMediaUpdate(request: ProvideMediaUpdateRequest): Observable<CoregPostResponse> {
      const routeUrl = `${this.postUrl}providemediaupdate`;

        return this.http.post(routeUrl, request)
            .map((res: Response) => {
                const result = res.json();
                return result;
            });
    }
}
