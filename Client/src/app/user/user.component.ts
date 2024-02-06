import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit{
  @ViewChild('addModal') addModal: any;
  users: User[] = [];

  constructor(private userService: UserService, private toastrService: ToastrService) { }

  ngOnInit() {
    this.userService.getUsers().subscribe(users => {
      this.users = users;
    });
  }

  showAddModal() {
    this.addModal.show();
  }

  hideAddModal() {
    this.addModal.hide();
  }

  remove(user: User) {
    if (!confirm('Bu kullanıcıyı silmek istediğinize emin misiniz?')) {
      return;
    }
    this.userService.deleteUser(user.id).subscribe({
      next: () => {
        this.users = this.users.filter(c => c.id !== user.id);
      },
      error: (error) => {
        this.toastrService.error('Kullanıcı silinirken hata oluştu');
        console.log(error);
      }
    });
  }
  legacy: boolean = false;
}
