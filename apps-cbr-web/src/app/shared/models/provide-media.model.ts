import { CoregLead } from './coreg-lead.model';

export class ProvideMediaRequest {
    // public contact: CoregLead;
    public cbrLeadId: number;
    public subIdTag: string;
    public campaignCode: string;
    public trustedForm: string;
}

export class ProvideMediaResponse {
    public success: boolean;
    public invalidPhone: boolean;
    public invalidZip: boolean;
    public invalidAddress: boolean;
    public other: string;
    public message: string;

}
export class ProvideMediaUpdateRequest {
  public retryRequest: ProvideMediaRequest;
  public address: string;
  public phone: string;
  public zip: string;
}

