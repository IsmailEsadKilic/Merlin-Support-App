import { Component, Input, Self } from '@angular/core';
import { NgControl } from '@angular/forms';

@Component({
  selector: 'app-date-input',
  templateUrl: './date-input.component.html',
  styleUrl: './date-input.component.css'
})
export class DateInputComponent {
  @Input() label: string = '';
  @Input() type: string = 'date';
  @Input() defaultValue: string = '';
  @Input() isRequired: boolean = false;

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {
  }

  registerOnChange(fn: any): void {
  }

  registerOnTouched(fn: any): void {
  }

  get control(): any {
    return this.ngControl.control;
  }
}
