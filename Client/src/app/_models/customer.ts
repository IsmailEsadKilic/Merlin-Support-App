import { CustomerProductList, CustomerProductListAdd } from "./customerProductList";

export interface Customer {
    id: number;
    rowGuiid: string;
    companyName: string;
    addressPhone: string;
    fax: string;
    taxOffice: string;
    taxNumber: string;
    customerEmail: string;
    jsonData: string;
    customerProductListDtos: CustomerProductList[];
}

export interface CustomerAdd {
    companyName: string;
    addressPhone: string;
    fax: string;
    taxOffice: string;
    taxNumber: string;
    customerEmail: string;
    jsonData: string;
    customerProductListDtos: CustomerProductListAdd[];
}

export interface CustomPropertyDescriptor {
    existingValue: string;
    identifier: string;
    id: number;
    label: string;
    defaultValue: string;
    isRequired: boolean;
}