import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'customData',
  standalone: true
})
export class CustomDataPipe implements PipeTransform {

  transform(value: string, ...args: unknown[]): string {
    if (value == null || value == "") {
      return "";
    }
    let obj = JSON.parse(value);
    let result = "";
    Object.keys(obj).forEach(key => {
      if (obj[key] == null || obj[key] == "") {
        return;
      }
      result += key + ": " + obj[key] + ", ";
    });
    return result;
  }

}

// {"Satral Model":"","Software Versiyon":"","CPU ID":"","Pbx IP Adres Main":"","Pbx IP Adres CPU 1":"","Pbx IP Adres CPU 2":"","GD IP Adres 1":"","GD IP Adres 2":"","INTIP IP Adress 1":"","INTIP IP Adress 2":"","Node Number":"","MTCL Password":"","ROOT Password":"","SWINST Password":"","RMA Password":"","Modem No 1":"","Modem No 2":"","Uzak Erişim Bilgileri":"","Voice Mail":"","Digital User":"","IP User":"","SIP User":"","Analog User":"","Digital Dış Hat PRI":"","Analog Dış Hat":"","SIP Trunk":"","BRI":"","GAP Dect Dahili Konv":"","Wi-fi Dect User":"","Dect Baz İstasonu":"","4760 veya 8770 Versiyon":"","4760 veya 8770 Password":"","4760 veya 8770 PC Password":"","Automatic Att.":"","Hotel":"","CCS Version":"","Agent Sayısı":"","Networking":"","SPS Süresi":""}
