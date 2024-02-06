import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PriorityAddComponent } from './priority-add.component';

describe('PriorityAddComponent', () => {
  let component: PriorityAddComponent;
  let fixture: ComponentFixture<PriorityAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PriorityAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PriorityAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
