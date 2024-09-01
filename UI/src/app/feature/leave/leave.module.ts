import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LeaveRoutingModule } from './leave-routing.module';
import { LeaveComponent } from './leave/leave.component';
import { CalenderComponent } from './leave/calender/calender.component';
import { TypeOfLeaveComponent } from './leave/type-of-leave/type-of-leave.component';
import { PendingForApprovalComponent } from './leave/pending-for-approval/pending-for-approval.component';
import { LeaveHistoryComponent } from './leave/leave-history/leave-history.component';
import { SharedModule } from '../../shared/shared.module';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
import { FullCalendarModule } from '@fullcalendar/angular';
export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}

@NgModule({
  declarations: [
    LeaveComponent,
    CalenderComponent,
    TypeOfLeaveComponent,
    PendingForApprovalComponent,
    LeaveHistoryComponent,
  ],
  imports: [
    CommonModule,
    LeaveRoutingModule,
    SharedModule,
    FormsModule,
    FullCalendarModule,
    TranslateModule.forRoot(
      {
      loader:{
        provide:TranslateLoader,
        useFactory:HttpLoaderFactory,
        deps:[HttpClient]
      }
    }
    )
  ]
})
export class LeaveModule { }
