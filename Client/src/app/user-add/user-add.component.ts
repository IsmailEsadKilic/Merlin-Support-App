import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UserService } from '../_services/user.service';
import { UserAdd, permissionDescription } from '../_models/user';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-add',
  templateUrl: './user-add.component.html',
  styleUrl: './user-add.component.css'
})
export class UserAddComponent implements OnInit{
  @Output() addItemEvent = new EventEmitter<any>();
  
  //////

  userAdd: UserAdd = {} as UserAdd;
  userAddFormInitialised: boolean = false;
  
  userAddForm: FormGroup = new FormGroup({});
  userAddFormErrors: string[] = [];

  //////

  userName: string = "";
  userId: number = 0;
  userPermission: string = "";
  
  //////
  
  permdict: {[key: number]: string} = {}; // {101: "create user", 102: "create customer", ...}
  permissionDescriptions: permissionDescription[] = [];
  all: boolean = false;

  constructor(private userService: UserService, private toastrService: ToastrService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {
      //parse from string to number
      let queryId = +params['id'] || 0;
      this.userId = queryId; //if not 0, edit mode
      if (this.userId > 0) {

        //get a User by id
        //populate the userAdd from user

        this.userService.getUser(this.userId).subscribe({
          next: response => {
            this.userName = response.userName;
            this.userAdd = response;
            this.userPermission = response.permission
            this.userService.getPermDict().subscribe(permissions => {
              this.permdict = Object.fromEntries(Object.entries(permissions));
              this.InitUserAddForm();
            });
          },
          error: (err) => {
            console.log(err);
            this.toastrService.error(err.error);
          }
        });
      } else {
        this.userService.getPermDict().subscribe(permissions => {
          this.permdict = Object.fromEntries(Object.entries(permissions));
          this.InitUserAddForm();
        });
      }
    });


  }

  onSubmit() {
  }

  resetUserAddForm() {
    if (this.userId > 0) {
      this.userAddForm.reset(this.userAdd);
      
      //reset checkboxes

      var perms = this.userPermission.split("|");
      this.permissionDescriptions.forEach(permissionDescription => {
        permissionDescription.value = perms.includes(permissionDescription.identifier);
      });
    } else {
      this.userAddForm.reset();
      //reset checkboxes
      this.permissionDescriptions.forEach(permissionDescription => {
        permissionDescription.value = false;
      });
    }
  }

  trySubmitUserAddForm() {
    this.userAddFormErrors = [];
    if (!this.userAddForm.valid) {
      this.userAddFormErrors.push("Zorunlu alanları doldurunuz.");
      return;
    }

    this.userAdd = {...this.userAdd, ...this.userAddForm.value};
    this.userAdd.permission = this.permissionDescriptions
      .filter(permissionDescription => permissionDescription.value)
      .map(permissionDescription => permissionDescription.identifier)
      .join("|");

    if (this.userId == 0) {
      this.userService.addUser(this.userAdd).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Kullanıcı başarıyla eklendi.");
            this.router.navigate(['/user/edit'], { queryParams: { id: response.id } });
            this.addItemEvent.emit(response);
          } else {
            this.toastrService.error("Kullanıcı eklenemedi.");
          }
          this.resetUserAddForm();
        },
        error: err => {
          console.log(err);
          this.toastrService.error(err.error);
          this.userAddFormErrors.push(err.error);
        }
      });
    } else {
      this.userService.updateUser(this.userAdd, this.userId).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Kullanıcı başarıyla düzenlendi.");
            this.router.navigate(['/user/edit'], { queryParams: { id: response.id } });
          } else {
            this.toastrService.error("Kullanıcı düzenlenemedi.");
          }
          this.resetUserAddForm();
        },
        error: err => {
          console.log(err);
          this.toastrService.error(err.error);
          this.userAddFormErrors.push(err.error);
        }
      });
    }
  }

  InitUserAddForm() {
    this.userAddForm = new FormGroup({
      nameSurname: new FormControl(this.userAdd.nameSurname, Validators.required),
      userName: new FormControl(this.userAdd.userName, Validators.required),
      password: new FormControl(this.userAdd.password, Validators.required),
      email: new FormControl(this.userAdd.email, Validators.required),
      gsml: new FormControl(this.userAdd.gsml, Validators.required)
    });

    //perms are tickboxes

    if (this.userId > 0) {
      var perms = this.userPermission.split("|");
      this.permissionDescriptions = Object.entries(this.permdict).map(
        ([key, value]) => ({identifier: key, label: value, value: perms.includes(key)})
      );
    } else {
      this.permissionDescriptions = Object.entries(this.permdict).map(
        ([key, value]) => ({identifier: key, label: value, value: false})
      );
      this.userAddFormInitialised = true;
    }

    this.userAddFormInitialised = true;
  }

  allFn() {
    this.permissionDescriptions.forEach(permissionDescription => {
      permissionDescription.value = this.all;
    });
  }
  legacy: boolean = false;
}
