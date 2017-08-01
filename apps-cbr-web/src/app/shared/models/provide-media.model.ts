import { CoregLead } from './coreg-lead.model';
import { CoregPostRequestBase } from '../models/coreg-post-request-base.model'

export class ProvideMediaRequest extends CoregPostRequestBase {
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

