import { CoregLead } from './coreg-lead.model';


export class CoregPostRequest {
    public listsToUpdate: ListrakLists[];
    public data: CoregLead;
    public pageName: string;
}

export class AdHocRequest {
    public fieldItems: ListrakFieldItem[];
    public listsToUpdate: ListrakLists[];
    public pageName: string;
}

export class ListrakFieldItem {
    name: string;
    value: string;
}

export enum ListrakLists {
    Clik_US = 1,
    CBR_US_Certified = 2,
    CBR_CA_Certified = 3,
    CBR_AU_Certified = 4,
    CBR_UK_Certified = 5,
    CBR_US_Non_Cert = 6,
    CBR_CA_Non_Cert = 7,
    CBR_AU_Non_Cert = 8,
    CBR_UK_Non_Cert = 9
}

export const ListrakEmailField: string = 'CBR.EmailAddress';
export const ListrakJoinSoiField: string = 'CBR.joinsoi';
