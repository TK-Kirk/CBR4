export class Contact {
    public email: string;
    public firstname: string;
    public lastname: string;
    public address: string;
    public city: string;
    public state: string;
    public zip: string;
    public phone: string;
    public birthDate: Date;
    public gender: string;
    public ethnicity: string;
    public birthDateYear: number;
    public birthDateMonth: number;
    public birthDateDay: number;

    public getBirthDateYear(): number {
        if (!this.birthDate) {
            return null;
        }
        return this.birthDate.getFullYear();
    }
    public getBirthDateDay(): number {
        if (!this.birthDate) {
            return null;
        }
        return this.birthDate.getDate() + 1;
    }
    public getBirthDateMonth(): number {
        if (!this.birthDate) {
            return null;
        }
        return this.birthDate.getMonth() + 1;
    }

}