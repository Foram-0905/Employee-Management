import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployeeListRoutingModule } from './employee-list-routing.module';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { SharedModule } from "../../../shared/shared.module";
import { FormsModule } from '@angular/forms';


@NgModule({
    declarations: [
        EmployeeListComponent
    ],
    imports: [
        CommonModule,
        EmployeeListRoutingModule,
        SharedModule,
        FormsModule
    ]
})
export class EmployeeListModule { }
