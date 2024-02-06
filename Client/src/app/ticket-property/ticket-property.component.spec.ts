import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketPropertyComponent } from './ticket-property.component';

describe('TicketComponent', () => {
  let component: TicketPropertyComponent;
  let fixture: ComponentFixture<TicketPropertyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketPropertyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TicketPropertyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
