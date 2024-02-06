import { Component, OnInit, ViewChild } from '@angular/core';
import { FileInputComponent } from '../file-input/file-input.component';
import { TicketNodeAdd } from '../_models/ticketNode';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TicketService } from '../_services/ticket.service';
import { HtmlEditorComponent } from '../html-editor/html-editor.component';

@Component({
  selector: 'app-ticket-node-add',
  templateUrl: './ticket-node-add.component.html',
  styleUrl: './ticket-node-add.component.css'
})
export class TicketNodeAddComponent implements OnInit {
  @ViewChild(FileInputComponent) fileInputComponent: FileInputComponent = {} as FileInputComponent;
  @ViewChild(HtmlEditorComponent) htmlEditorComponent: HtmlEditorComponent = {} as HtmlEditorComponent;

  //////

  ticketNodeAdd: TicketNodeAdd = {} as TicketNodeAdd;

  constructor(private ticketService: TicketService, private toastrService: ToastrService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      let queryId = +params['ticketId'];
      if (queryId) {
        this.ticketNodeAdd.ticketId = queryId;
      } else {
        this.router.navigate(['/ticket']);
      }
    });
  }

  SubmitTicketNode() {
    this.ticketNodeAdd.node = this.htmlEditorComponent.valueContent;
    this.ticketService.addTicketNode(this.ticketNodeAdd).subscribe({
      next: response => {
        this.toastrService.success("Cevap eklendi");
        this.fileInputComponent.uploadFiles("ticketNode", response.id, this.ticketNodeAdd.ticketId);
      },
      error: err => {
        console.log(err);
        this.toastrService.error("Cevap eklenirken hata oluştu.");
        this.toastrService.error(err.error);
      }
    })
  }
}
