import { Component } from '@angular/core';
import { TabConfig } from '../_services/html-editor.service';
import { HtmlEditorService } from '../_services/html-editor.service';

@Component({
  selector: 'app-html-editor',
  templateUrl: './html-editor.component.html',
  styleUrl: './html-editor.component.css'
})
export class HtmlEditorComponent {
  isMultiline: boolean | null | undefined = true;

  valueContent: string;

  tabs: TabConfig[];

  currentTab: string[];

  constructor(private service: HtmlEditorService) {
    this.valueContent = service.getMarkup();
    this.tabs = service.getTabsData();
    this.currentTab = this.tabs[2].value;
  }

  logContent() {
    if (!this.valueContent) {
      console.log("The content is empty");
    }
    console.log(this.valueContent);
  }
}

