import { ActivatedRoute, Data, Router } from '@angular/router';
import { NgForm } from '@angular/forms';


import { Component, OnInit, ViewChild } from '@angular/core';
import { CookieService } from 'ngx-cookie';

import { TiburonContentComponent } from './tiburon-content';
import { CoregLead } from '../shared/models/coreg-lead.model';
import { CoregPostRequest, ListrakLists } from '../shared/models/listrak.model';
import { TextValue } from '../shared/models/text-value.model';
import { Rev1Service } from './services/rev1.service';
import { XVerifyService } from '../shared/services/xverify.service';
import { ListrakService } from '../shared/services/listrak.service';

import * as postscribe from 'postscribe';



@Component({
    selector: 'rev1-signup',
    templateUrl: 'rev1-signup.component.html'
})
export class Rev1SignupComponent implements OnInit {

    @ViewChild('contactform') contactform: NgForm;
    validationOn = false;
    invalidEmail = false;
    invalidBirthdate = false;
    contact: CoregLead;
    yearlist: TextValue[] = [];
    monthlist: TextValue[] = [];
    daylist: TextValue[] = [];
    transactionid: string;
    isDuplicate: boolean;

    constructor(private _cookieService: CookieService, private _route: ActivatedRoute, private _router: Router,
    private _xverifyService: XVerifyService, private _rev1Service: Rev1Service, private _listrakService: ListrakService) {
        this.contact = new CoregLead;
        const c = this.getCookie('emailaddress');
        if (this.isMobile()) {
            this.contact.device = 'mobile';
        } else {
            this.contact.device = 'desktop';
        }
    }

    ngOnInit(): void {
        this.setUpDateDropdowns();

        this.initializeParameters();

        this.validationOn = false;
    }

    private isMobile(): boolean {
        const a = navigator.userAgent;
        // tslint:disable-next-line:max-line-length
        if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) {
            console.log('is mobile');

            return true;
        }
        console.log('is desktop');
        return false;
    }

    private initializeParameters() {
        this._route
            .queryParams
            .subscribe((params: any): void => {
                this.transactionid = params['transaction_id'];
                this.writeCookie();

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

                if (this.contact.firstname === '{fname}') {
                    this.contact.firstname = null;
                }
                if (this.contact.lastname === '{lname}') {
                    this.contact.lastname = null;
                }
                if (this.contact.address === '{address}') {
                    this.contact.address = null;
                }
                if (this.contact.zip === '{zip}') {
                    this.contact.zip = null;
                }

                if (this.contact.gender != null) {
                    this.contact.gender = this.contact.gender.toLowerCase();
                    if (this.contact.gender === 'm') {
                        this.contact.gender = 'male';
                    }
                    if (this.contact.gender === 'f') {
                        this.contact.gender = 'female';
                    }
                }

                if (this.contact.affiliateId != null) {
                    ('00000' + this.contact.affiliateId).substring(Math.min(('' + this.contact.affiliateId).length, 5));
                }


                if (this.contact.offerId == null || this.contact.offerId === '') {
                    if (this.isMobile()) {
                        this.contact.offerId = '51002'; // CBR (SOI) v2 mobile
                    } else {
                        this.contact.offerId = '51001'; // CBR (SOI) v2 desktop
                    }
                }


            });
    }

    verifyEmail() {
        this._xverifyService.getEmailIsValid(this.contact.email)
            .subscribe((data: boolean) => {
                this.invalidEmail = !data;
            });
    }

    submit(): void {
        this.validationOn = true;
        if (this.validateBirthdate() && this.contactform.valid) {
            this._rev1Service.postLead(this.contact)
                .subscribe((data: CoregLead) => {
                  const cbrId = data.cbrLeadId;
                  if (!data.isDuplicate) {
                    this._listrakService.postToLists(data, [ListrakLists.CBR_US_Certified, ListrakLists.CBR_US_Non_Cert], 'rev1signup').subscribe();

                    this._router.navigate(['/rev1enter'], { queryParams: { 'email': this.contact.email, 'firstname': this.contact.firstname, 'lastname': this.contact.lastname, 'address': this.contact.address, 'zip': this.contact.zip, 'gender': this.contact.gender, 'birthdate': this.contact.birthDate, 'country': this.contact.countryId, 'offerid': this.contact.offerId, 'affiliateid': this.contact.affiliateId, 'subid': this.contact.subId, 'cbrid': cbrId } });
                  }
                  this.isDuplicate = true;
                });

        }
    }

    validateBirthdate(): boolean {
        const birthdateString: string = this.contact.birthDateMonth + '/' + this.contact.birthDateDay + '/' + this.contact.birthDateYear;
        if (this.isValidDate(birthdateString)) {
            this.invalidBirthdate = false;
            return true;
        }
        this.invalidBirthdate = true;
        return false;
    }

    private isValidDate(dateString: string) {
        // First check for the pattern
        if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString)) {
            return false;
        }

        // Parse the date parts to integers
        const parts = dateString.split('/');
        const day = parseInt(parts[1], 10);
        const month = parseInt(parts[0], 10);
        const year = parseInt(parts[2], 10);

        // Check the ranges of month and year
        if (year < 1000 || year > 3000 || month === 0 || month > 12) {
            return false;
        }

        const monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        // Adjust for leap years
        if (year % 400 === 0 || (year % 100 !== 0 && year % 4 === 0)) {
            monthLength[1] = 29;
        }

        // Check the range of the day
        return day > 0 && day <= monthLength[month - 1];
    }

    private setUpDateDropdowns(): void {
        const today: Date = new Date();
        // the min age is 14

        this.yearlist.push({ text: 'Year', value: '0' });
        const year: number = today.getFullYear() - 14;
        for (let i = year; i > year - 86; i--) {
            this.yearlist.push({ text: i.toString(), value: i.toString() });
        }


        this.daylist.push({ text: 'Day', value: '0' });
        for (let i = 1; i <= 31; i++) {
            this.daylist.push({ text: i.toString(), value: i.toString() });
        }

        this.monthlist.push({ text: 'Month', value: '0' });
        for (let i = 1; i <= 12; i++) {
            switch (i) {
                case 1:
                    this.monthlist.push({ text: 'Janauary', value: i.toString() });
                    break;
                case 2:
                    this.monthlist.push({ text: 'February', value: i.toString() });
                    break;
                case 3:
                    this.monthlist.push({ text: 'March', value: i.toString() });
                    break;
                case 4:
                    this.monthlist.push({ text: 'April', value: i.toString() });
                    break;
                case 5:
                    this.monthlist.push({ text: 'May', value: i.toString() });
                    break;
                case 6:
                    this.monthlist.push({ text: 'June', value: i.toString() });
                    break;
                case 7:
                    this.monthlist.push({ text: 'July', value: i.toString() });
                    break;
                case 8:
                    this.monthlist.push({ text: 'August', value: i.toString() });
                    break;
                case 9:
                    this.monthlist.push({ text: 'September', value: i.toString() });
                    break;
                case 10:
                    this.monthlist.push({ text: 'October', value: i.toString() });
                    break;
                case 11:
                    this.monthlist.push({ text: 'November', value: i.toString() });
                    break;
                case 12:
                    this.monthlist.push({ text: 'December', value: i.toString() });
                    break;
                default:
            }
        }
    }

    private writeCookie() {
        const d: Date = new Date();
        const year: number = d.getFullYear();
        d.setFullYear(year + 1);
        this._cookieService.put('rev1signup_transid', 'key', { expires: d });
    }
    private getCookie(key: string) {
        return this._cookieService.get(key);
    }
}
