export interface AuthenticateRequest
{
    login: string;
    password: string;
}

export interface User
{
    id: string;
    name: string;
    lastName: string;
    token: string;
    isAdmin: boolean;
}

export interface ChangePassword
{
    oldPassword: string;
    newPassword: string;
}

export interface AddDoctorRequest
{
    name: string;
    lastName: string;
    login: string;
    password: string;
    isAdmin: boolean;
}