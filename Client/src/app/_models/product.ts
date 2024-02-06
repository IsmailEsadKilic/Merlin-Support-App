export interface Product {
    id: number;
    productName: string;
    rowGuiid: string;
    customerHasValidLicense: boolean;
}

export interface ProductAdd {
    productName: string;
    rowGuiid: string;
}

