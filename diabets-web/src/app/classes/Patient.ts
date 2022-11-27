export interface Patient
{
    id: number;
    name: string;
    lastName: string;
    pesel: string;
    birthDate: Date;
}

export interface AddPatientRequest
{
    name: string;
    lastName: string;
    pesel: string;
    birthDate: Date;
}