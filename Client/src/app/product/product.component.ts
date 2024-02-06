import { Component, OnInit, ViewChild } from '@angular/core';
import { Product } from '../_models/product';
import { ProductService } from '../_services/product.service';
import { CustomerProductList } from '../_models/customerProductList';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit {

  constructor(private productService: ProductService, private toastrService: ToastrService) { }

  ngOnInit() {
    this.productService.getProducts().subscribe(products => {
      this.products = products;
    });

    this.productService.getCustomerProductLists().subscribe(customerProductLists => {
      this.customerProductLists = customerProductLists;
    });
  }

  //product related

  @ViewChild('addProductModal') addProductModal: any;
  productAddModalInitialised: boolean = false;
  products: Product[] = [];

  hideAddProductModal() {
    this.productAddModalInitialised = false;
    this.addProductModal.hide();
  }

  showAddProductModal() {
    this.productAddModalInitialised = true;
    this.addProductModal.show();
  }

  removeProduct(product: Product) {
    if (!confirm('Bu ürünü silmek istediğinize emin misiniz?')) {
      return;
    }
    this.productService.deleteProduct(product.id).subscribe({
      next: () => {
        this.products = this.products.filter(c => c.id !== product.id);
      },
      error: (error) => {
        this.toastrService.error('Ürün silinirken hata oluştu');
        console.log(error);
      }
    });
  }

  //customer product list related

  @ViewChild('addCustomerProductListModal') addCustomerProductListModal: any;
  customerProductListAddModalInitialised: boolean = false;
  customerProductLists: CustomerProductList[] = [];

  hideAddCustomerProductListModal() {
    this.customerProductListAddModalInitialised = false;
    this.addCustomerProductListModal.hide();
  }

  showAddCustomerProductListModal() {
    this.customerProductListAddModalInitialised = true;
    this.addCustomerProductListModal.show();
  }

  removeCustomerProductList(customerProductList: CustomerProductList) {
    if (!confirm('Are you sure you want to delete this customer product list?')) {
      return;
    }
    this.productService.deleteCustomerProductList(customerProductList.id).subscribe({
      next: () => {
        this.customerProductLists = this.customerProductLists.filter(c => c.id !== customerProductList.id);
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
  legacy: boolean = false;
}
