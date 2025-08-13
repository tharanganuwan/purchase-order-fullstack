import { Routes } from '@angular/router';
import { OrderListComponent } from './purchases/order-list/order-list.component';
import { OrderFormComponent } from './purchases/order-form/order-form.component';


export const routes: Routes = [
  { path: '', redirectTo: 'purchases', pathMatch: 'full' },
  { path: 'purchases', component: OrderListComponent },
  { path: 'purchases/new', component: OrderFormComponent },
  { path: 'purchases/:id', component: OrderFormComponent },
  { path: '**', redirectTo: 'purchases' }
];

