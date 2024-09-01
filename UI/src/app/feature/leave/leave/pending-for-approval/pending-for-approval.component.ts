import { HttpParams } from '@angular/common/http';
import { Component, EventEmitter, input, Input, OnInit, Output, output } from '@angular/core';
import { ManageLeave } from '../../../../shared/models/manage-leave';
import { LeaveService } from '../../../../shared/services/leave.service';
import { first } from 'rxjs';
import { GridApi } from 'ag-grid-community';
import { TranslateService } from '@ngx-translate/core';
import { ApprovOrRejectLeave } from '../../../../shared/models/employee-leave';
import { ToastrService } from 'ngx-toastr';
import { ManageLeaveService } from '../../../../shared/services/manage-leave.service';
import { RequestWithFilterAndSort } from '../../../../shared/models/FilterRequset';
import { LoginService } from '../../../../core/services/login.service';
import { PermissionService } from '../../../../shared/services/permission.service';
@Component({
  selector: 'app-pending-for-approval',
  templateUrl: './pending-for-approval.component.html',
  styleUrl: './pending-for-approval.component.scss'
})
export class PendingForApprovalComponent implements OnInit {
  // employeePendingLeaves =[ { TypeName: 'seek', StartDate: '2024-04-23',EndDate:'2024-04-23' ,AppliedDate:'2024-04-23' ,ApprovedbyTeamlead:true, ApprovedbyOfficeManagement:false}];

  @Input() showApprovButton: boolean = false;
  @Input() showRejectButton: boolean = false;
  @Input() activeTab: any;
  @Input() employeePendingLeaves: any;
  @Input() employeeId: any;

  showEdit: boolean = true;
  showDelete: boolean = true;
  teamLead: any;
  LoginUser: any;
  private gridApi!: GridApi;

  @Input() totalPages: number[] = [];
  @Input() totalItems: any;

  @Output() onApproveLeave = new EventEmitter<any>();
  @Output() onRejectLeave = new EventEmitter<any>();
  @Output() editLeave = new EventEmitter<any>();
  @Output() deleteLeave = new EventEmitter<any>();
  @Output() filterModel = new EventEmitter<any>();


  columnDefsforEmployee = [
    { headerName: 'Type of Leave', field: 'leavetypeName', sortable: true, filter: true, filterParams: { maxNumConditions: 1 } },
    { headerName: 'Start Date', field: 'startDate', sortable: true, filter: true, filterParams: { maxNumConditions: 1 } },
    { headerName: 'End Date', field: 'endDate', sortable: true, filter: true, filterParams: { maxNumConditions: 1 } },
    { headerName: 'Applied on', field: 'appliedDate', sortable: true, filter: true, filterParams: { maxNumConditions: 1 } },
    { headerName: 'Teamlead', field: 'teamleadName', filterParams: { maxNumConditions: 1 } },
    { headerName: 'Office Management', field: 'officeManagementName', filterParams: { maxNumConditions: 1 } },
  ];

  columnDefsforOffice = [
    { headerName: 'Employee Name', field: 'employeeName', sortable: true, filter: true, filterParams: { maxNumConditions: 1 } },
    { headerName: 'Type of Leave', field: 'leavetypeName', sortable: true, filter: true, filterParams: { maxNumConditions: 1 } },
    { headerName: 'Start Date', field: 'startDate', sortable: true, filter: true, filterParams: { maxNumConditions: 1 } },
    { headerName: 'End Date', field: 'endDate', sortable: true, filter: true, filterParams: { maxNumConditions: 1 } },
    { headerName: 'Applied on', field: 'appliedDate', sortable: true, filter: true, filterParams: { maxNumConditions: 1 } },
    { headerName: 'Team Lead', field: 'approvedbyTeamlead', cellRenderer: (params: any) => this.createCheckboxRendererTeamLead(params) },
    { headerName: 'Office Management', field: 'approvedbyOfficeManagement', cellRenderer: (params: any) => this.createCheckboxRendererOfficemanagement(params) },
    // {
    //   headerName: 'Approved by Team Lead', field: 'approvedbyTeamlead', cellRenderer: function (params: any) {
    //     var input = document.createElement('input');
    //     input.type = "checkbox";
    //     input.checked = params.value;
    //     input.classList.add('check-input');
    //     input.disabled = activeTeamdear();
    //     input.addEventListener('click', function (event) {
    //       params.value = !params.value;
    //       params.node.data.approvedbyTeamlead = params.value;
    //     });
    //     return input;
    //   }
    // },
    // {
    //   headerName: 'Approved by Office Management', field: 'approvedbyOfficeManagement', cellRenderer: function (params: any) {
    //     var input = document.createElement('input');
    //     input.type = "checkbox";
    //     input.classList.add('check-input');
    //     input.checked = params.value;
    //     input.addEventListener('click', function (event) {
    //       params.value = !params.value;
    //       params.node.data.approvedbyOfficeManagement = params.value;
    //     });
    //     return input;
    //   }
    // },
    // { headerName: 'Rejected Reason', field: 'rejectedReason' }
  ];


  constructor(private leaveService: LeaveService, private permission: PermissionService, private loginService: LoginService, private translateService: TranslateService, private toastr: ToastrService, private manageLeaveService: ManageLeaveService) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');


  }
  ngOnInit(): void {
    // this.GetAllLeaves()
  }

  createCheckboxRendererTeamLead(params: any) {
    var input = document.createElement('input');
    input.type = "checkbox";
    input.classList.add('check-input');
    input.checked = params.value;
    input.disabled = !this.hasPermission("Leave.approveleavebyTeamlead.Approve");  // Disable based on login status

    input.addEventListener('click', (event) => {
      params.value = !params.value;
      params.node.data.approvedbyTeamlead = params.value;
      params.api.refreshCells({ force: true, rowNodes: [params.node] });
    });

    return input;
  }
  createCheckboxRendererOfficemanagement(params: any) {
    var input = document.createElement('input');
    input.type = "checkbox";
    input.classList.add('check-input');
    input.checked = params.value;
    input.disabled = !this.hasPermission("Leave.approveleavebyOfficemanagement.Approve");  // Disable based on login status

    input.addEventListener('click', (event) => {
      params.value = !params.value;
      params.node.data.approvedbyOfficeManagement = params.value;
      params.api.refreshCells({ force: true, rowNodes: [params.node] });
    });

    return input;
  }

  // activeTeamdear(): boolean{
  //   this.LoginUser = this.loginService.currentUserValue;
  //   if (this.LoginUser.role == 'HR Manager') {
  //     return false
  //   }
  //   return true
  // }


  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }


  // activeOfficeMangement(): boolean{
  //   this.LoginUser = this.loginService.currentUserValue;
  //   if (this.LoginUser.role != 'HR Manager') {
  //     return false
  //   }
  //   return true
  // }
  // GetAllLeaves() {
  //   this.leaveService
  //     .GetEmployeeLeaves(this.employeeId)
  //     .pipe(first())
  //     .subscribe((resp) => {
  //       this.employeePendingLeaves = resp.httpResponse.pendingLeaves;
  //     });
  // }

  teamLeadcheckBox(params: any) {

    if (params.data.approvedbyTeamlead) {

      // return `<input type="checkbox" style="color: red;" [(ngModel)]="approvedbyTeamlead" name="approvedbyTeamlead" id="approvedbyTeamlead" checked>`;
      return `<div class="check_box_group permission_grp"><div class="form-group"><input type="checkbox" [(ngModel)]="approvedbyTeamlead" name="approvedbyTeamlead" id="approvedbyTeamlead" checked/></div></div>`
    }
    else {
      return `<input type="checkbox" name="approvedbyTeamlead" [(ngModel)]="approvedbyTeamlead" id="approvedbyTeamlead">`;

    }
  }
  onApprove(params: any) {
    this.onApproveLeave.emit(params);
  }

  onReject(params: any) {
    this.onRejectLeave.emit(params);

  }
  onEditClick(event: any) {
    this.editLeave.emit(event);
  }
  onDeleteClick(event: any) {
    this.deleteLeave.emit(event);
  }

  getDataRowsourse(event: RequestWithFilterAndSort) {

    this.filterModel.emit(event);
  }
  gridReady(params: any) {
    this.gridApi = params.api;
  }
}

