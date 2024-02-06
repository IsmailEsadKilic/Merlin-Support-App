import { Component, OnInit, ViewChild } from '@angular/core';
import { Customer } from '../_models/customer';
import { CustomerService } from '../_services/customer.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})
export class CustomerComponent implements OnInit {
  @ViewChild('addModal') addModal: any;
  customers: Customer[] = [];

  constructor(private customerService: CustomerService, private toastrService: ToastrService) { }

  ngOnInit() {
    this.customerService.getCustomers().subscribe(customers => {
      this.customers = customers;
    });
  }

  showAddModal() {
    this.addModal.show();
  }

  hideAddModal() {
    this.addModal.hide();
  }

  remove(customer: Customer) {
    if (!confirm('Bu müşteriyi silmek istediğinize emin misiniz?')) {
      return;
    }
    this.customerService.deleteCustomer(customer.id).subscribe({
      next: () => {
        this.customers = this.customers.filter(c => c.id !== customer.id);
      },
      error: (error) => {
        console.log(error);
        this.toastrService.error('Müşteri silinirken hata oluştu');
      }
    });
  }
  legacy: boolean = false;
}
