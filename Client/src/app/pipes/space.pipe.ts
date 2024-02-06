import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'space',
  standalone: true
})
export class SpacePipe implements PipeTransform {

  transform(value: string, ...args: unknown[]): string {
    //replace all '|' with ' '
    return value.replace(/\|/g, ' ');
  }

}
