import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CustomPropertyDescriptor, Customer, CustomerAdd } from '../_models/customer';
import { CustomerService } from '../_services/customer.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CustomerProductList, CustomerProductListAdd } from '../_models/customerProductList';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../_services/product.service';
import { Router } from '@angular/router';
import { Console } from 'console';

@Component({
  selector: 'app-cutomer-add',
  templateUrl: './customer-add.component.html',
  styleUrl: './customer-add.component.css'
})
export class CustomerAddComponent implements OnInit {

  @Output() addItemEvent = new EventEmitter<Customer>();

  //////

  customerAdd: CustomerAdd = {} as CustomerAdd;
  customerAddFormInitialised: boolean = false;
  
  customerAddForm: FormGroup = new FormGroup({});
  customerAddFormErrors: string[] = [];

  //////

  customerName: string = "";
  customerJsonData: string = "";
  customerId: number = 0;

  ////// 

  customProperties: {[key: string]: string} = {};
  customPropertyDescriptors: CustomPropertyDescriptor[] = [];

  existingCustomerProductLists: CustomerProductList[] = []; //edit mode

  customerProductListsToAdd: CustomerProductListAdd[] = []; //adding -new //comes from customer-product-list-add

  IdsToDel: number[] = []; //del

  constructor(private customerService: CustomerService, private toastrService: ToastrService, private route: ActivatedRoute,
    private productService: ProductService, private router: Router) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      //parse from string to number
      let queryId = +params['customerId'] || 0;
      this.customerId = queryId; //if not 0, edit mode
      if (this.customerId > 0) {
        this.advancedMode = true;
        //get a Customer by id
        //populate the cumstomeradd from customer
        //populate existing customerProductLists from cutomer.customerProductLists

        this.customerService.getCustomer(this.customerId).subscribe({
          next: response => {
            this.customerName = response.companyName;
            this.customerJsonData = response.jsonData;
            if (response.customerProductListDtos) {
              this.existingCustomerProductLists = response.customerProductListDtos;
            }

            //map customer to customerAdd

            this.customerAdd.companyName = response.companyName;
            this.customerAdd.addressPhone = response.addressPhone;
            this.customerAdd.fax = response.fax;
            this.customerAdd.taxOffice = response.taxOffice;
            this.customerAdd.taxNumber = response.taxNumber;
            this.customerAdd.customerEmail = response.customerEmail;
            this.customerAdd.jsonData = response.jsonData;

            this.customerService.getCustomPropertyDescriptors().subscribe(customPropertyDescriptors => {
              this.customPropertyDescriptors = customPropertyDescriptors;
              this.InitCustomerAddForm();
            });
          },
          error: err => {
            console.log(err);
            this.toastrService.error(err.error);
          }
        });
      } else {
        this.customerService.getCustomPropertyDescriptors().subscribe(customPropertyDescriptors => {
          this.customPropertyDescriptors = customPropertyDescriptors;
          this.InitCustomerAddForm();
        });
      }
    });
  }

  onSubmit() {
    // this.trySubmitCustomerAddForm();
  }

  resetCustomerAddForm() {
    if (this.customerId > 0) {
      this.customerAddForm.reset(this.customerAdd);
    } else {
      this.customerAddForm.reset();
    }
  }

  trySubmitCustomerAddForm() {

    this.customerAddFormErrors = [];
    if (!this.customerAddForm.valid) {
      this.customerAddFormErrors.push("Zorunlu alanları doldurunuz.");
      return;
    }

    const values = {...this.customerAddForm.value};

    //deserialise custom properties
    this.customPropertyDescriptors.forEach(descriptor => {
      this.customProperties[descriptor.label] = values[descriptor.identifier];
      delete values[descriptor.identifier];
    });

    //serialise customerAdd
    this.customerAdd = {...this.customerAdd, ...values};
    
    //stringify custom properties
    this.customerAdd.jsonData = JSON.stringify(this.customProperties);

    //add customerProductLists to customerAdd
    this.customerAdd.customerProductListDtos = this.customerProductListsToAdd;

    if (this.customerId == 0) {
      this.customerService.addCustomer(this.customerAdd).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Müşteri başarıyla eklendi.");
            this.addItemEvent.emit(response);
            this.router.navigate(['/customer/edit'], { queryParams: { customerId: response.id } });
          } else {
            this.toastrService.error("Müşteri eklenirken hata oluştu.");
          }
          this.resetCustomerAddForm();
        },
        error: err => {
          console.log(err);
          this.toastrService.error(err.error);
          this.customerAddFormErrors.push(err.error);
        }
      });
    } else {
      this.customerService.updateCustomer(this.customerAdd, this.customerId, this.IdsToDel).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Müşteri başarıyla güncellendi.");
            this.router.navigate(['/customer/edit'], { queryParams: { customerId: response.id } });
          } else {
            this.toastrService.error("Müşteri güncellenirken hata oluştu.");
          }
          this.resetCustomerAddForm();
        },
        error: error => {
          console.log(error);
          this.toastrService.error(error.error);
          this.customerAddFormErrors.push(error.error);
        }
      });
    }
  }

  InitCustomerAddForm() {

    this.createIdentifiers();
  
    this.customerAddForm = new FormGroup({
      companyName: new FormControl(this.customerAdd.companyName, Validators.required),
      addressPhone: new FormControl(this.customerAdd.addressPhone, Validators.required),
      fax: new FormControl(this.customerAdd.fax, Validators.required),
      taxOffice: new FormControl(this.customerAdd.taxOffice, Validators.required),
      taxNumber: new FormControl(this.customerAdd.taxNumber, Validators.required),
      customerEmail: new FormControl(this.customerAdd.customerEmail, Validators.required),
    })
    
    if (this.customerId > 0) {
      //populate custom property descriptors' .existingValue, with this.customerJsonData
      var jsonValues = JSON.parse(this.customerJsonData);
      this.customPropertyDescriptors.forEach(descriptor => {
        descriptor.existingValue = jsonValues[descriptor.label] ? jsonValues[descriptor.label] : "";
      });
      this.customPropertyDescriptors.forEach(descriptor => {
        this.customerAddForm.addControl(descriptor.identifier, new FormControl(descriptor.existingValue, descriptor.isRequired ? Validators.required : null));
      });
    } else {
      this.customPropertyDescriptors.forEach(descriptor => {
        this.customerAddForm.addControl(descriptor.identifier, new FormControl(descriptor.defaultValue, descriptor.isRequired ? Validators.required : null));
      });
    }

    this.customerAddForm.valueChanges.subscribe(() => {
      this.customerAddFormErrors = [];
    });

    this.customerAddFormInitialised = true;
  }

  createIdentifiers() {
    this.customPropertyDescriptors.forEach(descriptor => {
      descriptor.identifier = descriptor.label.replace(/\s/g, '');
    });
  }

  log(any: any) {
    console.log(any);
  }

  advancedMode: boolean = false;
}
