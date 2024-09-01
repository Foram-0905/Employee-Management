import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserActiveLogRoutingModule } from './user-active-log-routing.module';
import { UserActiveLogComponent } from './user-active-log/user-active-log.component';


@NgModule({
  declarations: [
    UserActiveLogComponent
  ],
  imports: [
    CommonModule,
    UserActiveLogRoutingModule
  ]
})
export class UserActiveLogModule { }
