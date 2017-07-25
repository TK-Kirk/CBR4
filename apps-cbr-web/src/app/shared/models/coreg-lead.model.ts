import { Contact } from './Contact.model';

export class CoregLead extends Contact {
    public cbrLeadId: number;
    public offerId: string;
    public affiliateId: string;
    public subId: string;
    public countryId: string;
    public ip: string;
    public device: string;
}

