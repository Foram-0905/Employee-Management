import { filterModel } from './../../../../shared/models/FilterRequset';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { LeaveService } from '../../../../shared/services/leave.service';
import { TranslateService } from '@ngx-translate/core';
import { first } from 'rxjs';
import { RequestWithFilterAndSort } from '../../../../shared/models/FilterRequset';
import { GridApi } from 'ag-grid-community';

@Component({
  selector: 'app-leave-history',
  templateUrl: './leave-history.component.html',
  styleUrl: './leave-history.component.scss'
})
export class LeaveHistoryComponent {
  // employeeLeavesHistory = [];
  // this.employeeLeavesHistory= [{ TypeName: 'seek', StartDate: '2024-04-23', EndDate: '2024-04-23',day:'4', AppliedDate: '2024-04-23',Rejected:false, ApprovedbyTeamlead: true, ApprovedbyOfficeManagement: false }];
@Input() activeTab:any;
@Input() employeeId:any;
@Input() employeeLeavesHistory:any;
@Input() totalPages: number[] = [];
@Input() totalItems: any;

@Output() filterModel = new EventEmitter<any>();

private gridApi!: GridApi;
  columnDefsEmployee = [

    { headerName: 'Type of Leave', field: 'leavetypeName', sortable: false, filter: true,filterParams: { maxNumConditions: 1 } },
    { headerName: 'Start Date', field: 'startDate', sortable: false, filter: true,filterParams: { maxNumConditions: 1 } },
    { headerName: 'End Date', field: 'endDate', sortable: false, filter: true,filterParams: { maxNumConditions: 1 } },
    { headerName: 'Days', field: 'leaveDay', sortable: false, filter: true,filterParams: { maxNumConditions: 1 } },
    { headerName: 'Applied on', field: 'appliedDate', sortable: false, filter: true ,filterParams: { maxNumConditions: 1 }},
    { headerName: 'Approved Or Rejected',cellRenderer: this.approvereject.bind(this)}
  ];

  columnDefsOffice = [
    { headerName: 'Employee Name', field: 'employeeName', sortable: true, filter: true,filterParams: { maxNumConditions: 1 } },
    { headerName: 'Type of Leave', field: 'leavetypeName', sortable: true, filter: true,filterParams: { maxNumConditions: 1 } },
    { headerName: 'Start Date', field: 'startDate', sortable: true, filter: true ,filterParams: { maxNumConditions: 1 }},
    { headerName: 'End Date', field: 'endDate', sortable: true, filter: true,filterParams: { maxNumConditions: 1 } },
    { headerName: 'Days', field: 'leaveDay', sortable: true, filter: true,filterParams: { maxNumConditions: 1 } },
    { headerName: 'Applied on', field: 'appliedDate', sortable: true, filter: true,filterParams: { maxNumConditions: 1 } },
    { headerName: 'Approved Or Rejected',cellRenderer: this.approvereject.bind(this)},
  ];
  constructor(private leaveService: LeaveService, private translateService: TranslateService) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
  ngOnInit(): void {
    // this.GetAllLeaves()
  }

  approvereject(params: any) {

    if (params.data.isApproved) {
      return `<p class="approveLeave text-success" style="font-weight: bolder !important;">Approved</i></p>`;
    }
     if(params.data.isRejected){
      return `<p class="rejectLeave text-danger" style="font-weight: bolder !important;">Rejected</p>`;
    }
      return '';


  }
  getDataRowsourse(event: RequestWithFilterAndSort) {

    this.filterModel.emit(event);
  }
  gridReady(params: any) {
    this.gridApi = params.api;
  }

  // GetAllLeaves() {
  //   this.leaveService
  //     .GetEmployeeLeaves(this.employeeId)
  //     .pipe(first())
  //     .subscribe((resp) => {
  //       this.employeeLeavesHistory = resp.httpResponse.leavesHistory;
  //     });
  // }
}
