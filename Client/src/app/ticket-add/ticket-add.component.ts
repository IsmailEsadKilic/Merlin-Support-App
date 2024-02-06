import { Component, Injectable, OnInit, ViewChild } from '@angular/core';
import { TicketAdd, TicketType } from '../_models/ticket';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TicketService } from '../_services/ticket.service';
import { Customer } from '../_models/customer';
import { Product } from '../_models/product';
import { Priority } from '../_models/priority';
import { Team, TeamMember, TeamMemberAdd } from '../_models/team';
import { forkJoin } from 'rxjs';
import { CustomerService } from '../_services/customer.service';
import { ProductService } from '../_services/product.service';
import { TeamService } from '../_services/team.service';
import { FileInputComponent } from '../file-input/file-input.component';
import { HtmlEditorComponent } from '../html-editor/html-editor.component';

@Component({
  selector: 'app-ticket-add',
  templateUrl: './ticket-add.component.html',
  styleUrl: './ticket-add.component.css'
})
export class TicketAddComponent implements OnInit {
  @ViewChild(FileInputComponent) fileInputComponent: FileInputComponent = {} as FileInputComponent;
  @ViewChild(HtmlEditorComponent) htmlEditorComponent: HtmlEditorComponent = {} as HtmlEditorComponent;

  //////

  ticketAdd: TicketAdd = {} as TicketAdd;
  ticketAddFormInitialised: boolean = false;

  ticketAddForm: FormGroup = new FormGroup({});
  ticketAddFormErrors: string[] = [];

  //////

  //ticketSubject: string = "";
  //ticketId: number = 0;

  //////

  customerLookup: Customer[] = [];
  customerId: number = 0;

  productLookup: Product[] = [];

  priorityLookup: Priority[] = [];

  teamLookup: Team[] = [];
  teamId: number = 0;
  teamName: string = "";
  teamMembers: TeamMember[] = [];
  teamMemberAddsToAdd: TeamMemberAdd[] = [];

  ticketTypeLookup: TicketType[] = [];
  
  //////

  all: boolean = false;

  constructor(private ticketService: TicketService, private toastrService: ToastrService, private route: ActivatedRoute,
    private router: Router, private customerService: CustomerService, private productService: ProductService,
    private teamService: TeamService) { }    

  ngOnInit(): void {

    const lookups$ = forkJoin([
      this.customerService.getCustomers(),
      this.ticketService.getPriorities(),
      this.teamService.getTeams(),
      this.ticketService.getTicketTypes()
    ]);

    // Subscribe to the result

    lookups$.subscribe({
      next: ([customers, priorities, teams, ticketTypes]) => {
        // Assign the retrieved data to the corresponding properties
        this.customerLookup = customers;
        this.priorityLookup = priorities;
        this.teamLookup = teams;
        this.ticketTypeLookup = ticketTypes;

        this.InitTicketAddForm();
      },
      error: (err) => {
        console.log(err);
        this.toastrService.error(err.error);
      }
    })
  }

  //   this.route.queryParams.subscribe(params => {
  //     //parse from string to number
  //     let queryId = +params['ticketId'] || 0;
  //     this.ticketId = queryId; //if not 0, edit mode
  //     if (this.ticketId > 0) {
  //     } else {
  //       //get all lookups
  //       const lookups$ = forkJoin([
  //         this.customerService.getCustomers(),
  //         this.productService.getProducts(),
  //         this.ticketService.getPriorities(),
  //         this.teamService.getTeams(),
  //         this.ticketService.getTicketTypes()
  //       ]);

  //       // Subscribe to the result

  //       lookups$.subscribe({
  //         next: ([customers, products, priorities, teams, ticketTypes]) => {
  //           // Assign the retrieved data to the corresponding properties
  //           this.customerLookup = customers;
  //           this.productLookup = products;
  //           this.priorityLookup = priorities;
  //           this.teamLookup = teams;
  //           this.ticketTypeLookup = ticketTypes;

  //           this.InitTicketAddForm();
  //         },
  //         error: (err) => {
  //           console.log(err);
  //           this.toastrService.error(err.error);
  //         }
  //       })

  //       // lookups$.subscribe(([customers, products, priorities, teams, ticketTypes]) => {
  //       //   // Assign the retrieved data to the corresponding properties
  //       //   this.customerLookup = customers;
  //       //   this.productLookup = products;
  //       //   this.priorityLookup = priorities;
  //       //   this.teamLookup = teams;
  //       //   this.ticketTypeLookup = ticketTypes;
  //       // });
  //     }
  //   });
  // }

  onSubmit() {
    //this.trySubmitTicketAddForm();
  }

  resetTicketAddForm() {
    if (this.ticketAddFormInitialised) {
      this.ticketAddForm.reset();
      this.all = false;
      this.teamId = 0;
      this.teamName = "";
      this.teamMembers = [];
      this.teamMemberAddsToAdd = [];
    } else {
      this.ticketAddForm.reset();
    }
  }

  trySubmitTicketAddForm() {

      this.ticketAddFormErrors = [];
      if (!this.ticketAddForm.valid) {
        console.log(this.ticketAddForm.controls);
        this.ticketAddFormErrors.push("Zorunlu alanları doldurunuz.");
        return;
      }
  
      // const values = { ...this.ticketAddForm.value };
      //add to ticketAdd object
      const values = { ...this.ticketAddForm.value };

      //populate team members
      //adding or not is determined by the value of the checkbox.    value: boolean; // for checkboxes in ticket-add
      
      this.teamMembers.forEach(teamMember => {
        if (teamMember.value) {
          this.teamMemberAddsToAdd.push({ teamId: this.teamId, userId: teamMember.userId });
        }
      });
      
      this.ticketAdd = { ...this.ticketAdd, ...values };
      
      this.ticketAdd.teamMemberDtos = this.teamMemberAddsToAdd;

      this.ticketAdd.description = this.htmlEditorComponent.valueContent;
  
      this.ticketService.addTicket(this.ticketAdd).subscribe({
        next: response => {
          this.toastrService.success("Ticket Başarıyla Eklendi.");
          this.fileInputComponent.uploadFiles("ticket", response.id);
        },
        error: err => {
          console.log(err);
          this.toastrService.error(err.error);
        }
      });
      // }
  }

  InitTicketAddForm() {
    this.ticketAddForm = new FormGroup({
      subject: new FormControl(this.ticketAdd.subject, [Validators.required, Validators.maxLength(100)]),
      // description: new FormControl(this.ticketAdd.description, [Validators.required, Validators.maxLength(1000)]),
      productId: new FormControl(this.ticketAdd.productId, [Validators.required, this.isZero()]),
      teamId: new FormControl(this.ticketAdd.teamId, [Validators.required]),
      priorityId: new FormControl(this.ticketAdd.priorityId, [Validators.required]),
      customerId: new FormControl(this.ticketAdd.customerId, [Validators.required]),
      ticketTypeId: new FormControl(this.ticketAdd.ticketTypeId, [Validators.required])
    });

    //subscribe to customer Id changes
    this.ticketAddForm.get('customerId')?.valueChanges.subscribe(customerId => {
      // populate product lookup
      this.productLookup = [];
      this.customerId = customerId;
      this.productService.getProductsByCustomerId(customerId).subscribe(products => {
        this.productLookup = products;
      });
    });

    //subscribe to team Id changes
    this.ticketAddForm.get('teamId')?.valueChanges.subscribe(teamId => {
      // populate team members
      this.teamId = teamId;
      this.teamMembers = [];
      this.teamService.getTeam(teamId).subscribe(team => {
        this.teamId = teamId;
        this.teamName = team.teamName;
        this.teamMembers = team.teamMemberDtos;
      });
    });
    
    // this.ticketAddForm.valueChanges.subscribe(() => {
    //   this.onTicketAddFormValuesChanged();
    // });

    this.ticketAddFormInitialised = true;
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

  allFn() {
    this.teamMembers.forEach(teamMember => {
      teamMember.value = this.all;
    });
  }

  advancedMode: boolean = false;
}
// export interface TicketAdd {
//   subject: string;
//   description: string;
//   productId: number;
//   teamId: number;
//   priorityId: number;
//   customerId: number;
//   ticketTypeId: number;
//   teamMembers: TeamMember[];
// }