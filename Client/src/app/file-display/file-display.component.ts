import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { environment } from '../environment';
import { FileDownload } from '../_models/FileDownload';
import { response } from 'express';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-file-display',
  templateUrl: './file-display.component.html',
  styleUrl: './file-display.component.css'
})
export class FileDisplayComponent implements OnInit {

  baseUrl = environment.apiUrl + 'file';

  @Input() forTicketNodeId: number = 0;
  @Input() forTicketId: number = 0;

  //////


  fileDownloads: FileDownload[] = [];

  constructor(private httpClient: HttpClient, private toastr: ToastrService) { }

  ngOnInit() {
    this.loadFiles();
  }

  loadFiles() {
    
    if (this.forTicketNodeId > 0) {
      this.httpClient.get<FileDownload[] | boolean>(this.baseUrl + '/ticketnode/' + this.forTicketNodeId).subscribe(files => {
        if (files && files instanceof Array) {
          this.fileDownloads = files;
        } else {
          this.fileDownloads = [];
        }
      });
    } else if (this.forTicketId > 0) {
      this.httpClient.get<FileDownload[] | boolean>(this.baseUrl + '/ticket/' + this.forTicketId).subscribe(files => {
        if (files && files instanceof Array) {
          this.fileDownloads = files;
        } else {
          this.fileDownloads = [];
        }
      });
    }
  }

  downloadFile(fileId: number) {
    this.httpClient.get(this.baseUrl + '/download/' + fileId,
      {observe: 'response', responseType: 'blob'}
    ).subscribe(response => {
      let fileName = response.headers.get('content-disposition')!.split(';')[1].split('=')[1] || 'file';
      let blob: Blob = response.body as Blob;
      let a = document.createElement('a');
      a.download = fileName;
      a.href = URL.createObjectURL(blob);
      a.click();
    });
  }

  deleteFile(fileId: number) {
    if (!confirm('Dosyayı silmek istediğinize emin misiniz?')) {
      return;
    }
    this.httpClient.delete(this.baseUrl + '/' + fileId).subscribe(response => {
      if (response) {
        this.toastr.success('Dosya silindi.');
      }
      this.loadFiles();
    });
  }
}
