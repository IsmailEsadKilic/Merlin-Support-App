import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Product, ProductAdd } from '../_models/product';
import { ProductService } from '../_services/product.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-product-add',
  templateUrl: './product-add.component.html',
  styleUrl: './product-add.component.css'
})
export class ProductAddComponent implements OnInit{
  @Output() addItemEvent = new EventEmitter<Product>();

  //////

  productAdd: ProductAdd = {} as ProductAdd;
  productAddFormInitialised: boolean = false;

  productAddForm: FormGroup = new FormGroup({});
  productAddFormErrors: string[] = [];

  //////

  productName: string = "";
  productId: number = 0;

  constructor(private productService: ProductService, private toastrService: ToastrService, private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      //parse from string to number
      let queryId = +params['id'] || 0;
      this.productId = queryId; //if not 0, edit mode
      if (this.productId > 0) {

        //get a Product by id
        //populate the productAdd from product

        this.productService.getProduct(this.productId).subscribe({
          next: response => {
            this.productName = response.productName;
            this.productAdd = response;
            this.InitProductAddForm();
          },
          error: (err) => {
            console.log(err);
            this.toastrService.error(err.error);
          }
        });
      } else {
        this.InitProductAddForm();
      }
    });
  }

  onSubmit() {
    this.trySubmitProductAddForm();
  }

  resetProductAddForm() {
    if (this.productId > 0) {
      this.productAddForm.reset(this.productAdd);
    } else {
      this.productAddForm.reset();
    }
  }

  trySubmitProductAddForm() {
    this.productAddFormErrors = [];
    if (!this.productAddForm.valid) {
      this.productAddFormErrors.push("Zorunlu alanları doldurunuz.");
      return;
    }

    this.productAdd = {...this.productAdd, ...this.productAddForm.value};

    if (this.productId == 0) {
      this.productService.addProduct(this.productAdd).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Ürün başarıyla eklendi.");
            this.addItemEvent.emit(response);
            this.router.navigate(['/product/edit'], { queryParams: { id: response.id } });
          } else {
            this.toastrService.error("Ürün eklenemedi.");
          }
          this.resetProductAddForm();
        },
        error: (err) => {
          console.log(err);
          this.toastrService.error(err.error);
          this.productAddFormErrors.push(err.error);
        }
      });
    } else {
      this.productService.updateProduct(this.productAdd, this.productId).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Ürün başarıyla güncellendi.");
            this.router.navigate(['/product/edit'], { queryParams: { id: response.id } });
          } else {
            this.toastrService.error("Ürün güncellenemedi.");
          }
          this.resetProductAddForm();
        },
        error: (err) => {
          console.log(err);
          this.toastrService.error(err.error);
          this.productAddFormErrors.push(err.error);
        }
      });
    }
  }

  InitProductAddForm() {
    this.productAddForm = new FormGroup({
      productName: new FormControl(this.productAdd.productName, [
        Validators.required,
      ])
    });
    this.productAddFormInitialised = true;
  }
  legacy: boolean = false;
}

