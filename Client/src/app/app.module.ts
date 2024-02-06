import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
import { TestComponent } from './test/test.component';
import { ToastrModule } from 'ngx-toastr';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';
import { ErrorInterceptor } from './error.interceptor';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { RouterModule, provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { NavComponent } from './nav/nav.component';
import { TicketPropertyComponent } from './ticket-property/ticket-property.component';
import { CustomerComponent } from './customer/customer.component';
import { ProductComponent } from './product/product.component';
import { TeamComponent } from './team/team.component';
import { HomeComponent } from './home/home.component';
import { CustomerAddComponent } from './cutomer-add/customer-add.component';
import { TextInputComponent } from './text-input/text-input.component';
import { UserComponent } from './user/user.component';
import { UserAddComponent } from './user-add/user-add.component';
import { SpacePipe } from "./pipes/space.pipe";
import { TeamAddComponent } from './team-add/team-add.component';
import { TicketTypeAddComponent } from './ticket-type-add/ticket-type-add.component';
import { PriorityAddComponent } from './priority-add/priority-add.component';
import { ProductAddComponent } from './product-add/product-add.component';
import { CustomerProductListAddComponent } from './customer-product-list-add/customer-product-list-add.component';
import { DateInputComponent } from './date-input/date-input.component';
import { NamePipe } from './pipes/name.pipe';
import { PermsPipe } from './pipes/perms.pipe';
import { DisplayDatePipe } from './pipes/display-date.pipe';
import { CustomDataPipe } from './pipes/custom-data.pipe';
import { JwtInterceptor } from './jwt.interceptor';
import { TicketComponent } from './ticket/ticket.component';
import { TicketAddComponent } from './ticket-add/ticket-add.component';
import { FileInputComponent } from './file-input/file-input.component';
import { ProgressbarModule } from 'ngx-bootstrap/progressbar';
import {NgxFilesizeModule} from 'ngx-filesize';
import { TicketEditComponent } from './ticket-edit/ticket-edit.component';
import { DxHtmlEditorModule, DxCheckBoxModule, DxSelectBoxModule } from 'devextreme-angular';
import { HtmlEditorComponent } from './html-editor/html-editor.component';
import { TicketNodeAddComponent } from './ticket-node-add/ticket-node-add.component';
import { FileDisplayComponent } from './file-display/file-display.component';
import { MemberNamesPipe } from './pipes/member-names.pipe';
@NgModule({
    declarations: [
        TicketNodeAddComponent,
        FileDisplayComponent,
        HtmlEditorComponent,
        TicketEditComponent,
        FileInputComponent,
        TicketAddComponent,
        TicketComponent,
        ProductAddComponent,
        CustomerProductListAddComponent,
        PriorityAddComponent,
        TicketTypeAddComponent,
        TeamComponent,
        TeamAddComponent,
        UserComponent,
        UserAddComponent,
        TextInputComponent,
        DateInputComponent,
        CustomerAddComponent,
        AppComponent,
        TestComponent,
        NavComponent,
        TicketPropertyComponent,
        CustomerComponent,
        ProductComponent,
        TeamComponent,
        HomeComponent
    ],
    providers: [
        provideRouter(routes), provideAnimations(),
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
    ],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        DxHtmlEditorModule,
        DxCheckBoxModule,
        DxSelectBoxModule,
        CommonModule,
        RouterModule.forRoot(routes),
        BsDropdownModule.forRoot(),
        BsDatepickerModule.forRoot(),
        ModalModule.forRoot(),
        ToastrModule.forRoot({
            positionClass: 'toast-bottom-right',
            preventDuplicates: true
        }),
        HttpClientModule,
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        SpacePipe,
        NamePipe,
        PermsPipe,
        DisplayDatePipe,
        CustomDataPipe,
        MemberNamesPipe,
        ProgressbarModule.forRoot(),
        NgxFilesizeModule
    ]
})
export class AppModule { }
