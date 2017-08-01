import { CoregPostRequestBase } from "./coreg-post-request-base.model";

export class EngageIqRequest extends CoregPostRequestBase {
  public q1: string;
  public q2: string;
  public q3: string;
  public q4: string;
  public q5: string;
  public q6: string;
  public comments1: string;
}