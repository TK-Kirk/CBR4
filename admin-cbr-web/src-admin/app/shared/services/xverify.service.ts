import { Http, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../environments/environment';
import 'rxjs/add/operator/map';

@Injectable()
export class XVerifyService {
    xverifyUrl: string;
    hostUrl: string;
    constructor(private http: Http) {
      this.xverifyUrl = `${environment.host}api/xverify/`;
    }

    getEmailIsValid(email: string): Observable<boolean> {
        const routeUrl = `${this.xverifyUrl}verifyemail/${email}/`;

        return this.http.get(routeUrl)
            .map((res: Response) => {
                const result: boolean = res.json();
                return result;
            });
    }
}
