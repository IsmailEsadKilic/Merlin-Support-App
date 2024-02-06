export interface User {
    id: number;
    rowGuiid: string;
    nameSurname: string;
    userName: string;
    password: string;
    email: string;
    gsml: string;
    permission: string;
    token: string;
    roles: string[];
}

export interface UserAdd {
    nameSurname: string;
    userName: string;
    password: string;
    email: string;
    gsml: string;
    permission: string;
}

export interface permissionDescription {
    identifier: string;
    label: string;
    value: boolean;
}