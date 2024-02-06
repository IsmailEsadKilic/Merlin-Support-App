import { Pipe, PipeTransform } from '@angular/core';
import { TeamMember } from '../_models/team';

@Pipe({
  name: 'memberNames',
  standalone: true
})
export class MemberNamesPipe implements PipeTransform {

  transform(value: TeamMember[], ...args: unknown[]): string {
    return value.map(x => x.name).join(', ');
  }

}
