import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Priority, PriorityAdd, TimeTypeList } from '../_models/priority';
import { TicketService } from '../_services/ticket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-priority-add',
  templateUrl: './priority-add.component.html',
  styleUrl: './priority-add.component.css'
})
export class PriorityAddComponent implements OnInit {
  @Output() addItemEvent = new EventEmitter<Priority>();

  //////

  priorityAdd: PriorityAdd = {} as PriorityAdd;
  priorityAddFormInitialised: boolean = false;

  priorityAddForm: FormGroup = new FormGroup({});
  priorityAddFormErrors: string[] = [];

  //////

  timeTypeList = TimeTypeList;

  priorityName: string = "";
  priorityId: number = 0;

    constructor(private ticketService: TicketService, private toastrService: ToastrService, private activatedRoute: ActivatedRoute, private router: Router) { }
  
    ngOnInit(): void {
      this.ticketService.getTimeTypesDict().subscribe({
        next: (timeTypesDict) => {
          const dict = timeTypesDict as { [key: number]: string; };
          let timeTypeList = [];
          for (let key in dict) {
            timeTypeList.push({value: Number(key), name: dict[key]});
          }
          this.timeTypeList = timeTypeList;
        },
        error: (error) => {
          console.log(error);
        }
      });

      this.activatedRoute.queryParams.subscribe(params => {
        //parse from string to number
        let queryId = +params['id'] || 0;
        this.priorityId = queryId; //if not 0, edit mode
        if (this.priorityId > 0) {
  
          //get a Priority by id
          //populate the priorityAdd from priority
  
          this.ticketService.getPriority(this.priorityId).subscribe({
            next: response => {
              this.priorityName = response.priorityName;
              this.priorityAdd = response;
              this.InitPriorityAddForm();
            },
            error: (err) => {
              console.log(err);
              this.toastrService.error(err.error);
            }
          });
        } else {
          this.InitPriorityAddForm();
        }
      });
    }

    onSubmit() {
      this.trySubmitPriorityAddForm();
    }

    resetPriorityAddForm() {
      if (this.priorityId > 0) {
        this.priorityAddForm.reset(this.priorityAdd);
      } else {
        this.priorityAddForm.reset();
      }
    }

    trySubmitPriorityAddForm() {
      this.priorityAddFormErrors = [];
      if (!this.priorityAddForm.valid) {
        this.priorityAddFormErrors.push("Zorunlu alanları doldurunuz.");
        return;
      }

      this.priorityAdd = {...this.priorityAddForm.value};
      this.priorityAdd.listIndex = Number(this.priorityAdd.listIndex);
      this.priorityAdd.time = Number(this.priorityAdd.time);
      this.priorityAdd.timeType = Number(this.priorityAdd.timeType);


      if (isNaN(this.priorityAdd.listIndex)) {
        this.priorityAddFormErrors.push("Liste sırası sayı olmalıdır.");
        return;
      }

      if (isNaN(this.priorityAdd.time)) {
        this.priorityAddFormErrors.push("Süre sayı olmalıdır.");
        return;
      }

      if (isNaN(this.priorityAdd.timeType)) {
        this.priorityAddFormErrors.push("Süre tipi sayı olmalıdır.");
        return;
      }

      if (this.priorityId == 0) {
        this.ticketService.addPriority(this.priorityAdd).subscribe({
          next: response => {
            if (response) {
              this.toastrService.success("Öncelik başarıyla eklendi.");
              this.addItemEvent.emit(response);
              this.router.navigate(['/ticketProperty/priority/edit'], { queryParams: { id: response.id } });
            } else {
              this.toastrService.error("Öncelik eklenemedi.");
            }
            this.resetPriorityAddForm();
          },
          error: err => {
            console.log(err);
            this.toastrService.error(err.error);
          }
        });
      } else {
        this.ticketService.updatePriority(this.priorityAdd, this.priorityId,).subscribe({
          next: response => {
            if (response) {
              this.toastrService.success("Öncelik başarıyla güncellendi.");
              this.router.navigate(['/ticketProperty/priority/edit'], { queryParams: { id: response.id } });
            } else {
              this.toastrService.error("Öncelik güncellenemedi.");
            }
            this.resetPriorityAddForm();
          },
          error: err => {
            console.log(err);
            this.toastrService.error(err.error);
          }
        });
      }
    }

    InitPriorityAddForm() {
      this.priorityAddForm = new FormGroup({
        priorityName: new FormControl(this.priorityAdd.priorityName, [Validators.required]),
        timeType: new FormControl(this.priorityAdd.timeType, [Validators.required]),
        time: new FormControl(this.priorityAdd.time, [Validators.required]),
        listIndex: new FormControl(this.priorityAdd.listIndex)
      });
      this.priorityAddFormInitialised = true;
    }
  legacy: boolean = false;
}