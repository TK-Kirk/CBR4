import { Http, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { CoregLead } from '../../shared/models/coreg-lead.model';
import { ProvideMediaResponse, ProvideMediaRequest, ProvideMediaUpdateRequest } from '../../shared/models/provide-media.model';
import { environment } from '../../../environments/environment';

import 'rxjs/add/operator/map';

@Injectable()
export class PostService {
  postUrl: string;

  constructor(private http: Http) {
    this.postUrl = `${environment.host}api/post/`;

  }

    postProvideMedia(request: ProvideMediaRequest): Observable<ProvideMediaResponse> {
      const routeUrl = `${this.postUrl}providemedia`;

        return this.http.post(routeUrl, request)
            .map((res: Response) => {
                const result = res.json();
                return result;
            });
    }


      postProvideMediaUpdate(request: ProvideMediaUpdateRequest): Observable<ProvideMediaResponse> {
      const routeUrl = `${this.postUrl}providemediaupdate`;

        return this.http.post(routeUrl, request)
            .map((res: Response) => {
                const result = res.json();
                return result;
            });
    }
}
