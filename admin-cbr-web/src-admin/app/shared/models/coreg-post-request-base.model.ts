import { CoregCampaignType } from '../enums/coreg-campaign-type.enum';

export class CoregPostRequestBase {
  public email: string;
  public cbrLeadId: number;
  public subIdTag: string;
  public campaignCode: string;
  public zip: string;
  public campaignCodeId: CoregCampaignType;
}
