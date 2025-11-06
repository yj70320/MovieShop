import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Header } from './layout/header/header';
import { AccountRoutingModule } from "../account/account-routing-module";
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    Header
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    RouterModule
  ],
  exports: [
    Header
  ]
})
export class CoreModule { }
