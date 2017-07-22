import { Injectable } from '@angular/core';

@Injectable()
export class AppConfiguration {

    public authToken: string;
    public auditToken: string;
    public serviceApi: string;
    public accountInformationMartClientUrl: string;
    public relationshipClientUrl: string;
    public finalRiskRatingClientUrl: string;
    public customerAggregateLegalLiabilityClientUrl: string;
    public docuClickClientUrl: string;
    public agDirectAgrilyticClientUrl: string;
    public agrilyticClientUrl: string;
    public collateralHubClientUrl: string;
    public environment: string;
    public machineName: string;
    public traceLevel: number;
    public testUserAccounts: string[];

    constructor() {
        this.setConfigurationValues();
    }

    private setConfigurationValues() {

        //this.authToken = this.getConfigurationValueFromDOM('configAuthToken');
        //this.auditToken = this.getConfigurationValueFromDOM('configAuditToken');
        //this.serviceApi = this.getConfigurationValueFromDOM('configServiceApiUrl');
        //this.accountInformationMartClientUrl = this.getConfigurationValueFromDOM('configAccountInformationMartClientUrl');
        //this.relationshipClientUrl = this.getConfigurationValueFromDOM('configRelationshipClientUrl');
        //this.finalRiskRatingClientUrl = this.getConfigurationValueFromDOM('configFinalRiskRatingClientUrl');
        //this.customerAggregateLegalLiabilityClientUrl = this.getConfigurationValueFromDOM('configCustomerAggregateLegalLiabilityClientUrl');
        //this.docuClickClientUrl = this.getConfigurationValueFromDOM('configDocuClickClientUrl');
        //this.agDirectAgrilyticClientUrl = this.getConfigurationValueFromDOM('configAgDirectAgrilyticClientUrl');
        //this.agrilyticClientUrl = this.getConfigurationValueFromDOM('configAgrilyticClientUrl');
        //this.collateralHubClientUrl = this.getConfigurationValueFromDOM('collateralHubClientUrl');
        //this.environment = this.getConfigurationValueFromDOM('configEnvironment');
        //this.machineName = this.getConfigurationValueFromDOM('configMachineName');
        //this.traceLevel = Number(this.getConfigurationValueFromDOM('configTraceLevel'));
        //this.machineName = this.getConfigurationValueFromDOM('configMachineName');
        //this.testUserAccounts = this.getConfigurationValueFromDOM('configTestUserAccounts').split(',');
    }

    private getConfigurationValueFromDOM(elementId: string): string {

        let elementToFind: HTMLElement;
        let castedToHtmlInputElement: HTMLInputElement;

        elementToFind = document.getElementById(elementId);

        //Check if element exists in DOM
        if (!elementToFind) {
            throw new Error(`Element id:${elementId} not found in the DOM`);
        }

        //Validate that the found element is an input type
        if (elementToFind instanceof HTMLInputElement) {
            castedToHtmlInputElement = (elementToFind as HTMLInputElement);
        } else {
            throw new Error(`Element id:${elementId} is not an HTMLInputElement type.`);
        }

        //Check that the value of the input element has a value
        if (castedToHtmlInputElement.value.trim().length === 0) {
            throw new Error(`The value attribute for element id:${elementId} was
                            not provided or has an empty string or only
                            whitespace assigned to it.
                            Please provide a valid value`);
        }

        return castedToHtmlInputElement.value;
    }
}