export interface Examination
{
    date: Date;
    pregnancies: number;
    glucose: number;
    bloodPreasure: number;
    skinThickness: number;
    insulin: number;
    weight: number;
    height: number;
    diabetesPedigreeFunction: number;
    outcome: number
}

export interface AddExaminationRequest
{
    doctorId: string,
    patientId: number,
    pregnancies: string;
    glucose: string;
    bloodPreasure: string;
    skinThickness: string;
    insulin: string;
    weight: string;
    height: string;
    diabetesPedigreeFunction: string;
}