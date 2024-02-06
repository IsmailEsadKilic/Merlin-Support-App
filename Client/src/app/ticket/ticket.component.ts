import { Component } from '@angular/core';
import { Ticket } from '../_models/ticket';
import { ToastrService } from 'ngx-toastr';
import { TicketService } from '../_services/ticket.service';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrl: './ticket.component.css'
})
export class TicketComponent {
  tickets: Ticket[] = [];

  constructor(private ticketService: TicketService, private toastrService: ToastrService) { }

  ngOnInit() {
    this.ticketService.getTickets().subscribe(tickets => {
      this.tickets = tickets;
    });
  }

  remove(ticket: Ticket) {
    if (!confirm('Bu ticketı silmek istediğinize emin misiniz?')) {
      return;
    }
    this.ticketService.deleteTicket(ticket.id).subscribe({
      next: () => {
        this.tickets = this.tickets.filter(c => c.id !== ticket.id);
      },
      error: (error) => {
        console.log(error);
        this.toastrService.error('Ticket silinirken hata oluştu');
      }
    });
  }
}
