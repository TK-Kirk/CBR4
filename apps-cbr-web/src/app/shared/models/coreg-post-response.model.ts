export class CoregPostResponse {
  public success: boolean;
  public invalidPhone: boolean;
  public invalidZip: boolean;
  public invalidAddress: boolean;
  public zipIpVerificationFailed: boolean;
  public ipIsIrReputable: boolean;
  public other: string;
  public message: string;
}
