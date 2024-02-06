import { OnInit, Pipe, PipeTransform } from '@angular/core';
import { UserService } from '../_services/user.service';
import { firstValueFrom } from 'rxjs';
import e from 'express';

@Pipe({
  name: 'perms',
  standalone: true
})
export class PermsPipe implements PipeTransform {

  permdict: {[key: number]: string} = {}; // {101: "create user", 102: "create customer", ...}

  constructor(private userService: UserService) {
    this.permdict = this.userService.permDictCache;
  }


  transform(value: string , ...args: unknown[]): string {
    //value: "101|102|103"
    //args: [] 

    let permIds = value.split("|");
    let permNames: string[] = [];
    


    Object.entries(this.permdict).forEach(([key, value]) => {
      if (permIds.includes(key)) {
        permNames.push(value + "✓");
      } else {
        permNames.push(value + "✗");
      }
    });

    return permNames.join(", ");
  }

}
