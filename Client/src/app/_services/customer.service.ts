import { Injectable } from '@angular/core';
import { environment } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Customer, CustomerAdd, CustomPropertyDescriptor } from '../_models/customer';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  baseUrl = environment.apiUrl + 'customer';
  constructor(private http: HttpClient) { }

  getCustomers() {
    return this.http.get<Customer[]>(this.baseUrl)
  }

  getCustomer(id: number) {
    return this.http.get<Customer>(this.baseUrl + '/' + id);
  }
  
  addCustomer(customerAdd: CustomerAdd) {
    return this.http.put<Customer>(this.baseUrl + '/add', customerAdd);
  }
  
  updateCustomer(customerAdd: CustomerAdd, customerId: number, customerProductListIdsToDel: number[]) {
    //assume, customer
    return this.http.post<Customer>(this.baseUrl + '/update/' + customerId, {"customerDto": customerAdd, "customerProductListIdsToDel": customerProductListIdsToDel});
  }

  deleteCustomer(customerId: number) {
    return this.http.delete(this.baseUrl + '/' + customerId);
  }

  getCustomPropertyDescriptors() {
    return this.http.get<CustomPropertyDescriptor[]>(this.baseUrl + '/customPropertyDescriptors');
  }
}
