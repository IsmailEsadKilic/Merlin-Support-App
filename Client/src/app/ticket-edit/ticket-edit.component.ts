import { Component, OnInit, ViewChild } from '@angular/core';
import { FileInputComponent } from '../file-input/file-input.component';
import { Ticket, TicketAdd, TicketType } from '../_models/ticket';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Priority } from '../_models/priority';
import { CustomerService } from '../_services/customer.service';
import { ProductService } from '../_services/product.service';
import { TeamService } from '../_services/team.service';
import { TicketService } from '../_services/ticket.service';
import { forkJoin } from 'rxjs';
import { TicketNode } from '../_models/ticketNode';

@Component({
  selector: 'app-ticket-edit',
  templateUrl: './ticket-edit.component.html',
  styleUrl: './ticket-edit.component.css'
})
export class TicketEditComponent implements OnInit {

  @ViewChild(FileInputComponent) fileInputComponent: FileInputComponent = {} as FileInputComponent;
  
  //////

  ticketAdd = {} as TicketAdd;
  ticketEditFormInitialised = false;

  ticketEditForm: FormGroup = new FormGroup({});
  ticketEditFormErrors: string[] = [];

  //////

  // subject, status, priority , type, should be changeable

  priorityLookup: Priority[] = [];
  ticketTypeLookup: TicketType[] = [];
  ticketStateLookup: any[] = [];

  //////

  ticket = {} as Ticket;
  ticketId = 0;
  productName: string = "";

  //////

  ticketNodes: TicketNode[] = [];

  //   export interface TicketNode {
  //     id: number;
  //     rowGuid: string;
  //     node: string;
  //     recordUserId: number;
  //     recordUserName: string;
  //     ticketId: number;
  //     recordDate: Date;
  //     isDeleted: boolean;
  // }

  //////

  constructor(private ticketService: TicketService, private toastrService: ToastrService, private route: ActivatedRoute,
    private router: Router, private customerService: CustomerService, private productService: ProductService,
    private teamService: TeamService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      let queryId = +params['ticketId'] || 0;
      this.ticketId = queryId;
      if (this.ticketId > 0) {
        this.ticketService.getTicket(this.ticketId).subscribe({
          next: ticket => {
            this.ticket = ticket;
            this.productName = ticket.productName;
            this.ticketAdd = {
              subject: ticket.subject,
              priorityId: ticket.priorityId,
              ticketTypeId: ticket.ticketTypeId,
              ticketStateId: ticket.ticketStateId,

              productId: ticket.productId,
              teamId: ticket.teamId,
              customerId: ticket.customerId,
              description: ticket.description,
              teamMemberDtos: ticket.teamMemberDtos,
            };
            const lookups = forkJoin([
              this.ticketService.getPriorities(),
              this.ticketService.getTicketTypes(),
              this.ticketService.getTicketStatesDict()
            ]);

            lookups.subscribe({
              next: ([priorities, ticketTypes, ticketStates]) => {
                this.priorityLookup = priorities;
                this.ticketTypeLookup = ticketTypes;

                this.ticketStateLookup = Object.entries(ticketStates).map(([key, value]) => {
                  return { id: +key, name: value };
                });

                //remove id 0

                this.ticketStateLookup = this.ticketStateLookup.filter(x => x.id !== 0);
                
                this.initTicketEditForm();
              },
              error: err => this.toastrService.error(err.error || "error" , "Error")
            });
          },
          error: err => this.toastrService.error(err.error || "error" , "Error")
        })

        //get ticket nodes

        this.ticketService.getTicketNodesByTicketId(this.ticketId).subscribe({
          next: ticketNodes => {
            this.ticketNodes = ticketNodes;
          },
          error: err => this.toastrService.error(err.error || "error" , "Error")
        });
      }
    });
  }

  onSubmit() {
    this.trySubmitTicketEditForm();
  }

  resetTicketEditForm() {
    this.ticketEditForm.reset(this.ticketAdd);
  }

  trySubmitTicketEditForm() {
    this.ticketEditFormErrors = [];
    if (!this.ticketEditForm.valid) {
      console.log(this.ticketEditForm.controls);
      this.ticketEditFormErrors.push("Lütfen formu doğru şekilde doldurun.");
      return;
    }

    const values = {...this.ticketEditForm.value};
    
    this.ticketAdd = {...this.ticketAdd, ...values};

    this.ticketService.updateTicket(this.ticketAdd, this.ticketId).subscribe({
      next: response => {
        if (response) {
          this.toastrService.success("Ticket güncellendi.", "Başarılı");
          this.router.navigate(['/ticket/edit'], { queryParams: { ticketId: response.id } });
        } else {
          this.toastrService.error("Ticket güncellenirken hata oluştu.", "Hata");
        }
        this.resetTicketEditForm();
      },
      error: err => {
        this.toastrService.error(err.error || "error" , "Error");
      }
    });

  }

  initTicketEditForm() {
    this.ticketEditForm = new FormGroup({
      subject: new FormControl(this.ticketAdd.subject, [Validators.required, Validators.maxLength(100)]),
      priorityId: new FormControl(this.ticketAdd.priorityId, [Validators.required, this.isZero()]),
      ticketTypeId: new FormControl(this.ticketAdd.ticketTypeId, [Validators.required, this.isZero()]),
      ticketStateId: new FormControl(this.ticketAdd.ticketStateId, [Validators.required, this.isZero()]),
    });

    this.ticketEditFormErrors = [];

    this.ticketEditFormInitialised = true;
  }

  isZero(): ValidatorFn {
    return (control: AbstractControl) => {
      if (+control?.value === 0 || control?.value === "0") {
        return { isZero: true };
      }
      return null;
      // return control?.value === 0 ? { isZero: true } : control?.value === "0" ? { isZero: true } : {isZero : false};
    };
  }

  deleteTicketNode(ticketId: number) {
    if (!confirm("Ticket mesajını silmek istediğinize emin misiniz?")) {
      return;
    }

    this.ticketService.deleteTicketNode(ticketId).subscribe({
      next: response => {
        if (response) {
          this.toastrService.success("Ticket mesajı silindi.", "Başarılı");
          this.ticketNodes = this.ticketNodes.filter(x => x.id !== ticketId);
        } else {
          this.toastrService.error("Ticket mesajı silinirken hata oluştu.", "Hata");
        }
      },
      error: err => {
        this.toastrService.error(err.error || "error" , "Error");
      }
    });
  }
  
}

// export interface Ticket {
//   id: number;
//   rowGuid: string;
//   customerTicketId: number;
//   recordDate: string;
//   closedDate: string;
//   firstResponseTime: string;
//   subject: string;
//   description: string;
//   productId: number;
//   productName: string;
//   teamId: number;
//   teamName: string;

//   teamMemberDtos: TeamMember[];

//   priorityId: number;
//   priorityName: string;
  
//   recordUserId: number;

//   customerId: number;
//   customerName: string;

//   ticketStateId: number;
//   ticketStateName: string;

//   ticketTypeId: number;
//   ticketTypeName: string;
// }
