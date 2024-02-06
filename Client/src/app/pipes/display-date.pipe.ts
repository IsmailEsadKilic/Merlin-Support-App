import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'displayDate',
  standalone: true
})
export class DisplayDatePipe implements PipeTransform {

  //2024-01-26T16:59:10.0523538 -> 26/01/2024

  transform(value: string | Date, ...args: unknown[]): string {
    if (value === null || value === undefined)
      return '';

    

    if (typeof value === 'string') {
      value = new Date(value);
    }
    // let date = new Date(value);
    // let day = date.getDate();
    // let month = date.getMonth() + 1;
    // let year = date.getFullYear();
    // let displayDate = `${day}/${month}/${year}`;

      //2024-01-26T16:59:10.0523538 -> 26/01/2024 hour:minute:second

    let date = new Date(value);
    let day = date.getDate();
    let month = date.getMonth() + 1;
    let year = date.getFullYear();

    let hour = date.getHours();
    let hourString = hour.toString();
    if (hour < 10)
      hourString = '0' + hourString;

    let minute = date.getMinutes();
    let minuteString = minute.toString();
    if (minute < 10)
      minuteString = '0' + minuteString;

    let second = date.getSeconds();
    let secondString = second.toString();
    if (second < 10)
      secondString = '0' + secondString;

    let displayDate = `${day}/${month}/${year} ${hourString}:${minuteString}:${secondString}`;

    if (displayDate === '1/1/1')
      return '';
    return displayDate;
  }

}
