import { Routes } from '@angular/router';
import { TicketPropertyComponent } from './ticket-property/ticket-property.component';
import { CustomerComponent } from './customer/customer.component';
import { ProductComponent } from './product/product.component';
import { TeamComponent } from './team/team.component';
import { HomeComponent } from './home/home.component';
import { UserComponent } from './user/user.component';
import { CustomerAddComponent } from './cutomer-add/customer-add.component';
import { ProductAddComponent } from './product-add/product-add.component';
import { CustomerProductListAddComponent } from './customer-product-list-add/customer-product-list-add.component';
import { TeamAddComponent } from './team-add/team-add.component';
import { UserAddComponent } from './user-add/user-add.component';
import { TicketTypeAddComponent } from './ticket-type-add/ticket-type-add.component';
import { PriorityAddComponent } from './priority-add/priority-add.component';
import { TicketComponent } from './ticket/ticket.component';
import { TicketAddComponent } from './ticket-add/ticket-add.component';
import { TicketEditComponent } from './ticket-edit/ticket-edit.component';
import { TicketNodeAddComponent } from './ticket-node-add/ticket-node-add.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: '',
      children: [
        {path: 'customer', component: CustomerComponent},
        {path: 'customer/add', component: CustomerAddComponent},
        {path: 'customer/edit', component: CustomerAddComponent},

        {path: 'product', component: ProductComponent},

        {path: 'product/add', component: ProductAddComponent},
        {path: 'product/edit', component: ProductAddComponent},

        {path: 'product/customerProductList/add', component: CustomerProductListAddComponent},
        {path: 'product/customerProductList/edit', component: CustomerProductListAddComponent},

        {path: 'team', component: TeamComponent},
        {path: 'team/add', component: TeamAddComponent},
        {path: 'team/edit', component: TeamAddComponent},

        {path: 'ticket', component: TicketComponent},
        {path: 'ticket/add', component: TicketAddComponent},
        {path: 'ticket/edit', component: TicketEditComponent},

        {path: 'ticketNode/add', component: TicketNodeAddComponent},

        {path: 'ticketProperty', component: TicketPropertyComponent},

        {path: 'ticketProperty/ticketType/add', component: TicketTypeAddComponent},
        {path: 'ticketProperty/ticketType/edit', component: TicketTypeAddComponent},

        {path: 'ticketProperty/priority/add', component: PriorityAddComponent},
        {path: 'ticketProperty/priority/edit', component: PriorityAddComponent},

        {path: 'user', component: UserComponent},
        {path: 'user/add', component: UserAddComponent},
        {path: 'user/edit', component: UserAddComponent}
      ]
    },
    {path: '**', component: HomeComponent, pathMatch: 'full'}
  ];
