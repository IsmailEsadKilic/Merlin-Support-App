import { Component, OnInit, ViewChild } from '@angular/core';
import { AccountService, Login } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  userAny : any = {};

  changeMode: boolean = false;

  //////

  login: Login= {userName: '', password: ''};

  /////

  @ViewChild('staticModal') staticModal: any;

  changeForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  /////

  loginForm: FormGroup = new FormGroup({});
  loginFormErrors: string[] = [];

  constructor(public accountService: AccountService, private router: Router,
    private toastr: ToastrService, private fb: FormBuilder,) { }

  ngOnInit(): void {

    this.accountService.currentUser$.subscribe({
      next: (user) => {
        if (user) {
          this.userAny = user;
        } else {
          try {
            this.userAny = JSON.parse(localStorage.getItem('user')!);
          } catch (error) {
            console.log(error);
            this.userAny = null;
          }
        }
      }
    })

    this.InitializeChangeForm();
    this.InitializeLoginForm();
  }
  
  changePassword() {
    this.staticModal.show();
    this.changeMode = true;
  }

  clear() {
    this.changeForm.reset();
  }

  InitializeLoginForm() {
    this.loginForm = this.fb.group({
      userName: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });
  }

  InitializeChangeForm() {
    this.changeForm = this.fb.group({
      oldPassword : new FormControl('', [Validators.required]),
      password: new FormControl('', [
        Validators.required,
        // Validators.minLength(4),
        // Validators.maxLength(12),
        // this.requireUppercaseLowercaseNumber(),
      ]),
      confirmPassword: new FormControl('', [
        Validators.required,
        this.matchValues('password')
      ])
    });
    //below for matching password and confirm password. after user changes the password, it will check if the confirm password matches
    this.changeForm.controls['password'].valueChanges.subscribe({
      next: () => this.changeForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  matchValues(matchTo: string) : ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.get(matchTo)?.value ? null : {notMatching: true}
    }
  }

  requireUppercaseLowercaseNumber(): ValidatorFn {
    return (control: AbstractControl) => {
      const value = control.value;
      const hasUppercase = /[A-Z]/.test(value);
      const hasLowercase = /[a-z]/.test(value);
      const hasNumber = /[0-9]/.test(value);
  
      if (!hasUppercase || !hasLowercase || !hasNumber) {
        return { requireUppercaseLowercaseNumber: true };
      }
  
      return null;
    };
  }

  change() {
    var values = {
      currentPassword: this.changeForm.get('oldPassword')?.value,
      newPassword: this.changeForm.get('password')?.value
    }   
    this.accountService.changePassword(values).subscribe({
      next: (result) => {
        if (result) {
          this.toastr.success('Şifre değiştirildi.');
        }
        else {
          this.toastr.error('Şifre değiştirilemedi.');
        }
        this.clear();

      }
    }) 
  }

  Login() {

    this.login.userName = this.loginForm.get('userName')?.value;

    this.login.password = this.loginForm.get('password')?.value;

    this.accountService.login(this.login).subscribe({
      next: () => {
        this.toastr.success(this.login.userName + ' olarak giriş yapıldı.');
        this.login = {userName: '', password: ''};
        this.router.navigateByUrl('/home');
      }
    })

    // this.accountService.login(this.login).subscribe({
    //   next: () => {
    //     this.toastr.success(this.login.userName + ' olarak giriş yapıldı.');
    //     this.login = {userName: '', password: ''};
    //     this.router.navigateByUrl('/home');
    //   }
    // })
  }

  Logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
