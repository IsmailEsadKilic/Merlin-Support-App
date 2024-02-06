import { Component, OnInit, ViewChild } from '@angular/core';
import { TicketType } from '../_models/ticket';
import { TicketService } from '../_services/ticket.service';
import { Priority } from '../_models/priority';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-ticket-property',
  templateUrl: './ticket-property.component.html',
  styleUrl: './ticket-property.component.css'
})
export class TicketPropertyComponent implements OnInit {

  constructor(private ticketService: TicketService, private toastrService: ToastrService) {}

  ngOnInit() {
    this.ticketService.getTicketTypes().subscribe((ticketTypes: TicketType[]) => {
      this.ticketTypes = ticketTypes;
    });
    this.ticketService.getPriorities().subscribe((priorities: Priority[]) => {
      this.priorities = priorities;
      this.ticketService.getTimeTypesDict().subscribe({
        next: (timeTypesDict) => {
          // this.timeTypesDict = timeTypesDict;
          this.timeTypesDict = timeTypesDict as { [key: number]: string; };
        },
        error: (error) => {
          console.log(error);
        }
      });
    });
  }

  //ticket type related

  @ViewChild('ticketTypeAddModal') ticketTypeAddModal: any;
  ticketTypeAddModalInitialised = false;
  ticketTypes: TicketType[] = [];

  hideTicketTypeAddModal() {
    this.ticketTypeAddModalInitialised = false;
    this.ticketTypeAddModal.hide();
  }

  showTicketTypeAddModal() {
    this.ticketTypeAddModalInitialised = true;
    this.ticketTypeAddModal.show();
  }

  removeTicketType(ticketType: TicketType) {
    if (!confirm('Are you sure you want to delete this ticket type?')) {
      return;
    }
    this.ticketService.deleteTicketType(ticketType.id).subscribe({
      next: () => {
        this.ticketTypes = this.ticketTypes.filter(c => c.id !== ticketType.id);
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  //priority related

  @ViewChild('priorityAddModal') priorityAddModal: any;
  priorityAddModalInitialised = false;
  priorities: Priority[] = [];


  hidePriorityAddModal() {
    this.priorityAddModalInitialised = false;
    this.priorityAddModal.hide();
  }

  showPriorityAddModal() {
    this.priorityAddModalInitialised = true;
    this.priorityAddModal.show();
  }

  //////

  timeTypesDict: { [key: number]: string; } = {};

  timeTypeDescription(timeTypeId: number) {
    return this.timeTypesDict[timeTypeId];

  }

  removePriority(priority: Priority) {
    if (!confirm('Bu önceliği silmek istediğinize emin misiniz?')) {
      return;
    }
    this.ticketService.deletePriority(priority.id).subscribe({
      next: () => {
        this.priorities = this.priorities.filter(c => c.id !== priority.id);
      },
      error: (error) => {
        this.toastrService.error('Öncelik silinirken hata oluştu');
        console.log(error);
      }
    });
  }
  legacy: boolean = false;
}
