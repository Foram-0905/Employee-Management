import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedRoutingModule } from './shared-routing.module';
import { PasswordStrengthComponent } from './component/password-strength/password-strength.component';
import { BeonGridComponent } from './component/beon-grid/beon-grid.component';
import{MatGridListModule} from '@angular/material/grid-list';
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatSortModule } from "@angular/material/sort";
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { HttpClientModule } from '@angular/common/http';
import { AgGridModule } from 'ag-grid-angular';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
     BeonGridComponent,
    // PasswordStrengthComponent
  ],
  imports: [
    CommonModule,
    SharedRoutingModule,HttpClientModule,
    MatGridListModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
    MatFormFieldModule,
    AgGridModule,
    FormsModule
  ],
  providers:[],
  exports: [
    BeonGridComponent,
    
  ]
})
export class SharedModule {

 }
