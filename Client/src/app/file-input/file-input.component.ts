import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Subscription, retry } from 'rxjs';
import { environment } from '../environment';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';


@Component({
  selector: 'app-file-input',
  templateUrl: './file-input.component.html',
  styleUrl: './file-input.component.css'
})
export class FileInputComponent {

  baseUrl = environment.apiUrl + 'file';

  @Output() fileUploaded = new EventEmitter<File>();

  // accept attribute of file input
  @Input() requiredFileType: string = 'image/*, video/*, audio/*, .pdf, .doc, .docx, .txt, .xls, .xlsx, .ppt, .pptx, .zip, .rar';

  //////

  @Input() forTicketId: number = 0;
  @Input() forTicketNodeId: number = 0;

  //////

  formDataUploads: FormDataUpload[] = [];
  files: File[] = [];
  completedFiles: File[] = [];

  uploadSubscriptions: Subscription[] = [];

  //////

  uploading: boolean = false;

  constructor(private http: HttpClient, private router: Router) {}

  onFileChange(event: any) {
    const files: FileList = event.target.files;
    if (files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        //no duplicate file
        if (!this.files.some(x => x.name == file.name)) {
          this.files.push(file);
        }
      }
    }
  }

  uploadFiles(forClass?: string, forId?: number, forTicketId?: number) {
    
    if (forClass && forId) {
      if (forClass == 'ticket') {
        this.forTicketId = forId;
      }
      else if (forClass == 'ticketNode') {
        this.forTicketNodeId = forId;
        if (forTicketId) {
          this.forTicketId = forTicketId;
        } else {
          console.log('forTicketId not set');
          return;
        
        }
      }
    }

    if (this.uploading) {
      console.log('already uploading');
      return;
    }

    if (this.forTicketId <= 0 && this.forTicketNodeId <= 0) {
      console.log('forTicketId or forTicketNodeId not set');
      return;
    }

    if (this.files.length > 0) {
      this.uploading = true;
      this.files.forEach(file => {
        const formData = new FormData();
        formData.append("fileName", file.name);
        formData.append("file", file);
        formData.append("forTicketId", this.forTicketId.toString());
        formData.append("forTicketNodeId", this.forTicketNodeId.toString());
        const formDataUpload: FormDataUpload = formData as FormDataUpload;
        formDataUpload.fileName = file.name;
        formDataUpload.progress = 0;
        formDataUpload.formData = formData;
        this.formDataUploads.push(formDataUpload);
      });

      this.formDataUploads.forEach(formDataUpload => {

        const upload$ = this.http.post(this.baseUrl + "/upload", formDataUpload.formData, {
          reportProgress: true,
          observe: 'events'
        }).pipe(
          finalize(() => {
            this.uploadSubscriptions.splice(this.uploadSubscriptions.indexOf(uploadSubscription), 1);
            this.formDataUploads.splice(this.formDataUploads.indexOf(formDataUpload), 1);

            //remove file from files, add to completedFiles
            this.files.splice(this.files.indexOf(formDataUpload.formData.get('file') as File), 1);
            this.completedFiles.push(formDataUpload.formData.get('file') as File);

            if (this.uploadSubscriptions.length == 0) {
              this.uploading = false;
              if (this.forTicketNodeId > 0) {
                this.router.navigate(['/ticket/edit'], { queryParams: { ticketId: this.forTicketId } });
              }
            }
          })
        );

        const uploadSubscription = upload$.subscribe(event => {
          if (event.type == 1) {
            formDataUpload.progress = Math.round(100 * (event.loaded / (event.total || 100)));
          }
          else if (event.type == 4) {
            this.fileUploaded.emit(formDataUpload.formData.get('file') as File);
          }
        });

      });
    }
  }

  cancelUpload(formDataUpload: FormDataUpload) {

    ///

    const uploadSubscription = this.uploadSubscriptions.find(x => x.closed == false);
    if (uploadSubscription) {
      uploadSubscription.unsubscribe();
      this.uploadSubscriptions.splice(this.uploadSubscriptions.indexOf(uploadSubscription), 1);
      this.formDataUploads.splice(this.formDataUploads.indexOf(formDataUpload), 1);
      if (this.uploadSubscriptions.length == 0) {
        this.uploading = false;
      }
    }

    ///
  }

  removeFile(file: File) {
    this.files.splice(this.files.indexOf(file), 1);
  }
}

export interface FormDataUpload extends FormData {
  fileName: string;
  formData: FormData;
  progress: number;
}



// import { HttpClient, HttpEventType } from '@angular/common/http';
// import { Component, Input } from '@angular/core';
// import { Subscription, finalize } from 'rxjs';

// @Component({
//   selector: 'app-file-input',
//   templateUrl: './file-input.component.html',
//   styleUrl: './file-input.component.css'
// })
// export class FileInputComponent {

//   @Input()requiredFileType: string;

//   fileName = '';
//   uploadProgress: number;
//   uploadSub: Subscription;

//   constructor(private http: HttpClient) {}

//   onFileSelected(event: any) {
//       const file:File = event.target.files[0];
    
//       if (file) {
//           this.fileName = file.name;
//           const formData = new FormData();
//           formData.append("thumbnail", file);

//           const upload$ = this.http.post("/api/thumbnail-upload", formData, {
//               reportProgress: true,
//               observe: 'events'
//           })
//           .pipe(
//               finalize(() => this.reset())
//           );
        
//           this.uploadSub = upload$.subscribe(event => {
//             if (event.type == HttpEventType.UploadProgress) {
//               this.uploadProgress = Math.round(100 * (event.loaded / (event.total || 100)));
//             }
//           })
//       }
//   }

//   cancelUpload() {
//     this.uploadSub.unsubscribe();
//     this.reset();
//   }

//   reset() {
//     this.uploadProgress = null;
//     this.uploadSub = null;
//   }
// }