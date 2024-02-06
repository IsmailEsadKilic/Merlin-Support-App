import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketTypeAddComponent } from './ticket-type-add.component';

describe('TicketTypeAddComponent', () => {
  let component: TicketTypeAddComponent;
  let fixture: ComponentFixture<TicketTypeAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketTypeAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TicketTypeAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
