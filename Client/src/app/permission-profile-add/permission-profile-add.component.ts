import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { permissionDescription } from '../_models/user';
import { UserService } from '../_services/user.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-permission-profile-add',
  templateUrl: './permission-profile-add.component.html',
  styleUrl: './permission-profile-add.component.css'
})
export class PermissionProfileAddComponent {
  PermissionProfileName: string = "";
  permission: string = ""

  formInitialised: boolean = false;
  form: FormGroup = new FormGroup({});
  formErrors: string[] = [];

  permdict: {[key: number]: string} = {}; // {101: "create user", 102: "create customer", ...}
  permissionDescriptions: permissionDescription[] = [];
  all: boolean = false;

  constructor(private userService: UserService, private toastrService: ToastrService,
    private router: Router, private location: Location) { }

  ngOnInit(): void {
    this.userService.getPermDict().subscribe({
      next: response => {
        this.permdict = Object.fromEntries(Object.entries(response));
        this.initForm();
      },
      error: error => {
        this.toastrService.error("Error: " + error);
      }
    });
  }

  onSubmit() {
    this.submit();
  }

  submit() {
    this.formErrors = [];
    if (!this.form.valid || !(this.form.get('PermissionProfileName')?.value)) {
      this.formErrors.push("Zorunlu alanları doldurunuz.");
      return;
    }


    this.PermissionProfileName = this.form.get('PermissionProfileName')?.value!;
    
    this.permission = this.permissionDescriptions
    .filter(x => x.value)
    .map(x => x.identifier)
    .join('|');

    console.log(this.permissionDescriptions)
    console.log(this.permission);
    console.log(this.PermissionProfileName);

    this.userService.addPermissionProfile(this.PermissionProfileName, this.permission).subscribe({
      next: response => {
        if (response) {
          this.toastrService.success("İşlem başarılı.");
          this.resetForm();
        } else {
          this.toastrService.error("İşlem başarısız.");
        }
      },
      error: error => {
        console.log(error);
        this.toastrService.error("Error: " + error);
        this.formErrors.push("Error: " + error);
      }
    });

  }

  resetForm() {
    this.PermissionProfileName = "";
    this.permission = "";
    this.form.reset();
  }

  initForm() {
    this.form.addControl('PermissionProfileName', new FormControl('', [Validators.required]));

    this.permissionDescriptions = Object.entries(this.permdict).map(
      ([key, value]) => ({identifier: key, label: value, value: false})    
    );

    this.formInitialised = true;
  }

  allFn() {
    this.permissionDescriptions.forEach(permissionDescription => {
      permissionDescription.value = this.all;
    });
  }

  goBackToPrevPage(): void {
    this.location.back();
  }

}
