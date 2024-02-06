import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CustomerProductList, CustomerProductListAdd } from '../_models/customerProductList';
import { ProductService } from '../_services/product.service';
import { Product } from '../_models/product';
import { Customer } from '../_models/customer';
import { CustomerService } from '../_services/customer.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-customer-product-list-add',
  templateUrl: './customer-product-list-add.component.html',
  styleUrl: './customer-product-list-add.component.css'
})



export class CustomerProductListAddComponent implements OnInit{
  @Output() addItemEvent = new EventEmitter<CustomerProductList>();
  @Output() emitWithoutSavingEvent = new EventEmitter<CustomerProductListAdd>();

  //////

  customerProductListAdd: CustomerProductListAdd = {} as CustomerProductListAdd;
  customerProductListAddFormInitialised: boolean = false;

  customerProductListAddForm: FormGroup = new FormGroup({});
  customerProductListAddFormErrors: string[] = [];

  //////

  productList: Product[] = [];
  customerList: Customer[] = [];

  //////

  @Input() forCustomerId: number = 0;

  //////
  
  @Input() customerProductListId: number = 0; 

  //////

  addFromModal: boolean = false;
  editFromList: boolean = false;
  addFromUserEdit: boolean = false;
  addWithUser: boolean = false;

  constructor(private productService: ProductService, private toastrService: ToastrService, private customerService: CustomerService,
    private route: ActivatedRoute, private router: Router) { }
  
  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {
      //parse from string to number
      let queryId = +params['customerProductListId'] || 0;
      if (this.customerProductListId != -1) {
        this.customerProductListId = queryId;
      }

      if (this.forCustomerId == 0 && this.customerProductListId < 0) {
        this.addFromModal = true;
      }

      if (this.forCustomerId == 0 && this.customerProductListId > 0) {
        this.editFromList = true;
      }

      if (this.forCustomerId > 0 && this.customerProductListId == 0) {
        this.addFromUserEdit = true;
      }

      if (this.forCustomerId == 0 && this.customerProductListId == 0) {
        this.addWithUser = true;
      }

      if (!this.legacy && this.addWithUser) {
        this.addWithUser = false;
        this.addFromModal = true;
      }

      if (this.customerProductListId > 0) {
        //get a CustomerProductList by id
        //populate the customerProductListAdd from customerProductList
        this.productService.getCustomerProductList(this.customerProductListId).subscribe({
          next: response => {
            if (response) {
              this.customerProductListAdd = response;
            }
            this.customerService.getCustomers().subscribe({
              next: response => {
                this.customerList = response;
                this.productService.getProducts().subscribe({
                  next: response => {
                    this.productList = response;
                    this.InitCustomerProductListAddForm();
                  },
                  error: error => {
                    console.log(error);
                    this.toastrService.error(error.error);
                  }
                });
              },
              error: error => {
                console.log(error);
                this.toastrService.error(error.error);
              }
            });        
          },
          error: error => {
            console.log(error);
            this.toastrService.error(error.error);
          }
        });
      } else {
        this.customerService.getCustomers().subscribe({
          next: response => {
            this.customerList = response;
            this.productService.getProducts().subscribe({
              next: response => {
                this.productList = response;
                this.InitCustomerProductListAddForm();
              },
              error: error => {
                console.log(error);
                this.toastrService.error(error.error);
              }
            });
          },
          error: error => {
            console.log(error);
            this.toastrService.error(error.error);
          }
        });    
      }
    });
  }

  onSubmit() {
    // this.trySubmitCustomerProductListAddForm();
  }

  resetCustomerProductListAddForm() {
    if (this.customerProductListId > 0) {
      this.customerProductListAddForm.reset(this.customerProductListAdd);
    } else {
      this.customerProductListAddForm.reset();
    }
  }

  trySubmitCustomerProductListAddForm() {
    this.customerProductListAddFormErrors = [];
    if (!this.customerProductListAddForm.valid) {
      this.customerProductListAddFormErrors.push("Zorunlu alanları doldurunuz.");
      return;
    }

    this.customerProductListAdd = {...this.customerProductListAdd, ...this.customerProductListAddForm.value};

    if (this.editFromList) {
      this.productService.updateCustomerProductList(this.customerProductListAdd, this.customerProductListId).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Müşteri lisans listesi güncellendi.");
            this.router.navigate(['/product/customerProductList/edit'], { queryParams: { customerProductListId: response.id } });
          } else {
            this.toastrService.error("Müşteri lisans listesi güncellenirken bir hata oluştu.");
          }
          this.resetCustomerProductListAddForm();
        },
        error: error => {
          console.log(error);
          this.toastrService.error(error.error)
          this.customerProductListAddFormErrors.push(error.error);
        }
      });
    } else if (this.addFromModal) {
      this.productService.addCustomerProductList(this.customerProductListAdd).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Müşteri lisans listesine eklendi.");
            this.addItemEvent.emit(response);
          } else {
            this.toastrService.error("Müşteri lisans listesine eklenirken bir hata oluştu.");
          }
          this.resetCustomerProductListAddForm();
        },
        error: error => {
          console.log(error);
          this.toastrService.error(error.error)
          this.customerProductListAddFormErrors.push(error.error);
        }
      });
    }
  }

  InitCustomerProductListAddForm() {
    this.customerProductListAddForm = new FormGroup({
      version: new FormControl(this.customerProductListAdd.version),
      description: new FormControl(this.customerProductListAdd.description),
      productId: new FormControl(this.customerProductListAdd.productId, Validators.required),
      firstDate: new FormControl(this.customerProductListAdd.firstDate, Validators.required),
      endDate: new FormControl(this.customerProductListAdd.endDate, Validators.required),
    });

    //include selecting the customer when?
    if (this.addFromModal || this.editFromList) {
      this.customerProductListAddForm.addControl('customerId', new FormControl(this.forCustomerId, Validators.required));
    }

    this.customerProductListAddFormInitialised = true;
  }

  emitWithoutSaving() {
    this.customerProductListAddFormErrors = [];
    if (!this.customerProductListAddForm.valid) {
      this.customerProductListAddFormErrors.push("Zorunlu alanları doldurunuz.");
      return;
    }

    this.customerProductListAdd = {...this.customerProductListAdd, ...this.customerProductListAddForm.value};

    this.emitWithoutSavingEvent.emit(this.customerProductListAdd);

    this.customerProductListAddForm.reset();
  }
  legacy: boolean = false;
  debug: boolean = false;
}
// export interface CustomerProductList {
//   id: number;
//   rowGuiid: string;
//   productId: number;
//   version: string;
//   firstDate: string;
//   endDate: string;
//   customerId: number;
//   description: string;
// }

// export interface CustomerProductListAdd {
//   rowGuiid: string;
//   productId: number; //select
//   version: string;
//   firstDate: string; //datepicker
//   endDate: string; //datepicker
//   customerId: number; //select
//   description: string;
// }
