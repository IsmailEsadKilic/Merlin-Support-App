import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketNodeAddComponent } from './ticket-node-add.component';

describe('TicketNodeAddComponent', () => {
  let component: TicketNodeAddComponent;
  let fixture: ComponentFixture<TicketNodeAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketNodeAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TicketNodeAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
