export interface CustomerProductList {
    id: number;
    rowGuiid: string;
    productId: number;
    version: string;
    firstDate: string;
    endDate: string;
    customerId: number;
    customerName: string;
    description: string;
    productName: string;
}

export interface CustomerProductListAdd {
    productId: number;
    version: string;
    firstDate: string;
    endDate: string;
    customerId: number;
    description: string;
    productName: string;
}