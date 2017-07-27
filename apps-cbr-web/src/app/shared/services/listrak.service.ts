import { Http, Response, Headers } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { CoregLead } from '../models/coreg-lead.model';
import { CoregPostRequest, ListrakLists, ListrakFieldItem, AdHocRequest } from '../models/listrak.model';
import { environment } from '../../../environments/environment';
import 'rxjs/add/operator/map';



@Injectable()
export class ListrakService {
    listrakUrl: string;
    constructor(private http: Http) {
      this.listrakUrl = `${environment.host}api/listrak/`;
    }

    postToLists(data: CoregLead, lists: ListrakLists[], pagename: string): Observable<boolean> {
        const routeUrl = `${this.listrakUrl}coreg`;
        const request: CoregPostRequest = new CoregPostRequest;
        request.data = data;
        request.listsToUpdate = lists;
        request.pageName = pagename;

        const headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this.http.post(routeUrl, request, headers)
            .map((res: Response) => {
                const result: boolean = res.json();
                return result;
            });
    }


    postAdhoc(data: ListrakFieldItem[], lists: ListrakLists[], pagename: string): Observable<boolean> {
        const routeUrl = `${this.listrakUrl}addhocUpdate`;
        const request: AdHocRequest = new AdHocRequest;
        request.fieldItems = data;
        request.listsToUpdate = lists;
        request.pageName = pagename;

        const headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this.http.post(routeUrl, request, headers)
            .map((res: Response) => {
                const result: boolean = res.json();
                return result;
            });
    }
}