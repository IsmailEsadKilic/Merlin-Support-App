import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionProfileAddComponent } from './permission-profile-add.component';

describe('PermissionProfileAddComponent', () => {
  let component: PermissionProfileAddComponent;
  let fixture: ComponentFixture<PermissionProfileAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PermissionProfileAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PermissionProfileAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
