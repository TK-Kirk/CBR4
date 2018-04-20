namespace xverify {
    export class AddressVerificationResponse {
        public get isValid(): boolean {
            if (this.address != null) {
                return this.address.status === 'valid';
            }
            return false;
        }
        public address: Address;
    }
    export class Address {
        public error: number;
        public status: string;
        public response_code: number;
        public message: string;
        public duration: string;
        public address1: string;
        public street: string;
        public zip: string;
        public reason_for_invalid: string;
        public city: string;
        public state: string;
    }

    export class EmailVerificationResponse {
        public get isValid(): boolean {
            if (this.email != null && this.email.responsecode != null) {
                return this.email.responsecode === 1;
            }
            return false;
        }
        public email: Email;
    }
    export class Email {
        public syntax: string;
        public handle: string;
        public domain: string;
        public catch_all: string;

        public error: number;
        public message: string;
        public responsecode: number;
        public address: string;
        public status: string;
        public duration: number;
    }

    export class IpVerificationResponse {
        public get isValid(): boolean {
            if (this.ipdata != null) {
                return this.ipdata.status === 'valid';
            }
            return false;
        }
        public ipdata: Ipdata;
    }
    export class Ipdata {
        public value: string;
        public proxy: string;
        public error: number;
        public status: string;
        public response_code: number;
        public message: string;
        public region: string;
        public city: string;
        public country: string;
        public duration: string;
        public latitude: string;
        public longitude: string;
    }
}
