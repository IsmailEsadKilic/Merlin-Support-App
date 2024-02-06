import { Injectable } from '@angular/core';
import { environment } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Product, ProductAdd } from '../_models/product';
import { CustomerProductList, CustomerProductListAdd } from '../_models/customerProductList';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseUrl = environment.apiUrl + 'product'
  constructor(private http: HttpClient) { }

  getProducts() {
    return this.http.get<Product[]>(this.baseUrl);
  }

  getProductsByCustomerId(customerId: number) {
    return this.http.get<Product[]>(this.baseUrl + '/customer/' + customerId);
  }

  getProduct(id: number) {
    return this.http.get<Product>(this.baseUrl + '/' + id);
  }

  addProduct(productAdd: ProductAdd) {
    return this.http.put<Product>(this.baseUrl + '/add', productAdd);
  }

  updateProduct(productAdd: ProductAdd, productId: number) {
    return this.http.post<Product>(this.baseUrl + '/update/' + productId, productAdd);
  }

  deleteProduct(productId: number) {
    return this.http.delete(this.baseUrl + '/' + productId);
  }

  /////////////////////////////

  getCustomerProductLists() {
    return this.http.get<CustomerProductList[]>(this.baseUrl + '/customerProductList');
  }

  getCustomerProductList(id: number) {
    return this.http.get<CustomerProductList>(this.baseUrl + '/customerProductList/' + id);
  }

  addCustomerProductList(customerProductListAdd: CustomerProductListAdd) {
    return this.http.put<CustomerProductList>(this.baseUrl + '/customerProductList/add', customerProductListAdd);
  }

  updateCustomerProductList(customerProductListAdd: CustomerProductListAdd, customerProductListId: number) {
    return this.http.post<CustomerProductList>(this.baseUrl + '/customerProductList/update/' + customerProductListId, customerProductListAdd);
  }

  deleteCustomerProductList(customerProductListId: number) {
    return this.http.delete(this.baseUrl + '/customerProductList/' + customerProductListId);
  }
}
