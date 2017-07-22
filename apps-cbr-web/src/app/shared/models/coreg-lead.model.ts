import { Contact } from './Contact.model';

export class CoregLead extends Contact {
    public offerId: string;
    public affiliateId: string;
    public subId: string;
    public countryId: string;
    public ip: string;
    public device: string;
}

