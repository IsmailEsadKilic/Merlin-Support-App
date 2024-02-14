import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TicketType, TicketTypeAdd } from '../_models/ticket';
import { TicketService } from '../_services/ticket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ticket-type-add',
  templateUrl: './ticket-type-add.component.html',
  styleUrl: './ticket-type-add.component.css'
})
export class TicketTypeAddComponent implements OnInit{
  @Output() addItemEvent = new EventEmitter<TicketType>();

  //////

  ticketTypeAdd: TicketTypeAdd = {} as TicketTypeAdd;
  ticketTypeAddFormInitialised: boolean = false;

  ticketTypeAddForm: FormGroup = new FormGroup({});
  ticketTypeAddFormErrors: string[] = [];

  //////

  ticketTypeName: string = "";
  ticketTypeId: number = 0;

  constructor(private ticketService: TicketService, private toastrService: ToastrService, private activatedRoute: ActivatedRoute, private router: Router) { }
  
  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      //parse from string to number
      let queryId = +params['id'] || 0;
      this.ticketTypeId = queryId; //if not 0, edit mode
      if (this.ticketTypeId > 0) {

        //get a TicketType by id
        //populate the ticketTypeAdd from ticketType

        this.ticketService.getTicketType(this.ticketTypeId).subscribe({
          next: response => {
            this.ticketTypeName = response.name;
            this.ticketTypeAdd = response;
            this.InitTicketTypeAddForm();
          },
          error: (err) => {
            console.log(err);
            this.toastrService.error(err.error || "Ticket Tipi bulunamadı.");
          }
        });
      } else {
        this.InitTicketTypeAddForm();
      }
    });
  }

  onSubmit() {
    this.trySubmitTicketTypeAddForm();
  }

  resetTicketTypeAddForm() {
    if (this.ticketTypeId > 0) {
      this.ticketTypeAddForm.reset(this.ticketTypeAdd);
    } else {
      this.ticketTypeAddForm.reset();
    }
  }

  trySubmitTicketTypeAddForm() {
    this.ticketTypeAddFormErrors = [];
    if (!this.ticketTypeAddForm.valid) {
      this.ticketTypeAddFormErrors.push("Zorunlu alanları doldurunuz.");
      return;
    }

    this.ticketTypeAdd = {...this.ticketTypeAddForm.value};
    this.ticketTypeAdd.listIndex = Number(this.ticketTypeAdd.listIndex);

    if (isNaN(this.ticketTypeAdd.listIndex)) {
      this.ticketTypeAddFormErrors.push("Liste sırası sayı olmalıdır.");
      return;
    }

    if (this.ticketTypeId == 0) {
      this.ticketService.addTicketType(this.ticketTypeAdd).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Ticket Tipi başarıyla eklendi.");
            this.addItemEvent.emit(response);
            this.router.navigate(['/ticketProperty/ticketType/edit'], { queryParams: { id: response.id } });
          } else {
            this.toastrService.error("Ticket Tipi eklenemedi.");
          }
          this.resetTicketTypeAddForm();
        },
        error: err => {
          console.log(err);
          this.toastrService.error(err.error || "Ticket Tipi eklenemedi.");
          this.ticketTypeAddFormErrors.push(err.error);
        }
      });
    } else {
      this.ticketService.updateTicketType(this.ticketTypeAdd, this.ticketTypeId).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Ticket Tipi başarıyla güncellendi.");
            this.router.navigate(['/ticketProperty/ticketType/edit'], { queryParams: { id: response.id } });
          } else {
            this.toastrService.error("Ticket Tipi güncellenemedi.");
          }
          this.resetTicketTypeAddForm();
        },
        error: err => {
          console.log(err);
          this.toastrService.error(err.error || "Ticket Tipi güncellenemedi.");
          this.ticketTypeAddFormErrors.push(err.error);
        }
      });
    }
  }

  InitTicketTypeAddForm() {
    this.ticketTypeAddForm = new FormGroup({
      name: new FormControl(this.ticketTypeAdd.name, [Validators.required]),
      listIndex: new FormControl(this.ticketTypeAdd.listIndex, [Validators.required])
    });
    this.ticketTypeAddFormInitialised = true;
  }
  legacy: boolean = false;
}