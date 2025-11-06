import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from './login/login';
import { Register } from './register/register';
import { Account } from './account';

const routes: Routes = [
  { 
    path: '', component: Account, 
    children: [
      { path: 'login', component: Login },         // Login route, account/login
      { path: 'register', component: Register }    // Register route, account/register
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
