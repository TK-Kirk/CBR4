export class CoregDisplay {
  public showUpdate: boolean;
  public showConsent: boolean;
  public showQuestions: boolean;
  public consentChecked: boolean;
  public answer: string;
  public campaignCode: string;
  public zipMatchAttempts: number;
  public validationOn: boolean;
  constructor() {
    this.zipMatchAttempts = 0;
  }
}
