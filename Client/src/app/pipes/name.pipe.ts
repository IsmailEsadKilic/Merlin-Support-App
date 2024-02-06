import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'name',
  standalone: true
})
export class NamePipe implements PipeTransform {

  transform(value: object[], ...args: unknown[]): string {
    return value.map((item: any) => {
      return item.name;
    }).join(', ');
  }

}
