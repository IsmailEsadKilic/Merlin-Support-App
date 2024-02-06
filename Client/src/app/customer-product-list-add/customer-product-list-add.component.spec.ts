import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerProductListAddComponent } from './customer-product-list-add.component';

describe('CustomerProductListAddComponent', () => {
  let component: CustomerProductListAddComponent;
  let fixture: ComponentFixture<CustomerProductListAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerProductListAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CustomerProductListAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
