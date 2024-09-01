import { GridApi } from 'ag-grid-community';
import { getLeaveType } from './../../../../shared/constant/api.const';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { ManageLeave } from '../../../../shared/models/manage-leave';
import { RequestWithFilterAndSort, sortModel } from '../../../../shared/models/FilterRequset';
import { ManageLeaveService } from '../../../../shared/services/manage-leave.service';
import { first } from 'rxjs';
import { EmployeeService } from '../../../../shared/services/employee.service';
import { leaveTypeService } from '../../../../shared/services/leaveType.service';
import { DatePipe } from '@angular/common';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { NgxSpinnerService } from 'ngx-spinner';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NotificationService } from '../../../../shared/services/notification.service';


@Component({
  selector: 'app-manage-leave',
  templateUrl: './manage-leave.component.html',
  styleUrl: './manage-leave.component.scss'
})
export class ManageLeaveComponent implements OnInit {
  columnDefs:any[]=[];
  // columnDefs = [
  //   {
  //     headerName: 'Name of Employee',
  //     filter: true,
  //     field: 'employeeName',
  //     filterParams: { maxNumConditions: 1 },
  //   },
  //   {
  //     headerName: 'Leave Type',
  //     field: 'leavetypeName',
  //     filter: true,
  //     filterParams: { maxNumConditions: 1 },
  //   },
  //   {
  //     headerName: 'Start Date',
  //     field: 'startDate',
  //     filter: true,
  //     filterParams: { maxNumConditions: 1 },
  //   },
  //   {
  //     headerName: 'End Date',
  //     field: 'endDate',
  //     filter: true,
  //     filterParams: { maxNumConditions: 1 },
  //   },
  //   {
  //     headerName: 'Days of Requested Leave',
  //     field: 'leaveDay',
  //     filter: true,
  //     filterParams: { maxNumConditions: 1 },
  //   },
  // ];

  combineNames(params: any) {
    return `${params.data.employee.firstName} ${params.data.employee.middleName ? params.data.employee.middleName + ' ' : ''}${params.data.employee.lastName}`;
  }
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addBtn') addBtn!: ElementRef;
  showEdit: boolean = true;
  showDelete: boolean = true;
  slgGroup: any;
  dataRowSource!: any[];
  manageLeave = {} as ManageLeave;
  editId: any;
  showDeleteModal: boolean = false;
  totalPages: number[] = [];
  totalDay: any=0;
  totalItems: any;
  searchText: any;
  allLeaveType: any;
  employees: any;
  currentDate: string;
  startDate: Date | undefined;
  sortModel = {} as sortModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  private helper = new commonHelper();
  private gridApi!: GridApi;
  deleteLeave: any;
  SelectedEmployee!: string;

  constructor(private translateService: TranslateService,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private manageLeaveService: ManageLeaveService,
    private employeeService: EmployeeService,
    private leaveTypeService: leaveTypeService,
    private totalPageArray: totalPageArray,
    private spinner: NgxSpinnerService,
    private permission: PermissionService,
    private notificationservice:NotificationService
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "employeeName";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
    this.currentDate = new Date().toISOString().split('T')[0];
    this.manageLeave.leaveDay = "0";
    this.manageLeave.leavetype = " "; 
    this.manageLeave.employeeId = " "; 


  }

  ngOnInit(): void {
    setTimeout(() => {
      this.columnDefs = [
    {
      headerName: this.translateEnumValue('i18n.configuration.manageLeaveDetails.employeeName'),
      filter: true,
      field: 'employeeName',
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: this.translateEnumValue('i18n.configuration.manageLeaveDetails.leaveType'),
      field: 'leavetypeName',
      filter: true,
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: this.translateEnumValue('i18n.configuration.manageLeaveDetails.startDate'),
      field: 'startDate',
      filter: true,
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: this.translateEnumValue('i18n.configuration.manageLeaveDetails.employeeName'),
      field: 'endDate',
      filter: true,
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: this.translateEnumValue('i18n.configuration.manageLeaveDetails.employeeName'),
      field: 'leaveDay',
      filter: true,
      filterParams: { maxNumConditions: 1 },
    },      ];
    }, 50);
    this.manageLeave.leave_End = "Full";
    this.manageLeave.leave_Start_From = "Full";
    this.manageLeave.leave_duration = "Full"
    this.getLeaves();
    this.getLeaveType();
  }
    translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  getLeaveType() {
    this.leaveTypeService
      .getLeaves().pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.allLeaveType = resp.httpResponse;
        },
        error: (error: any) => {
          this.toastr.error("Get Leave Type", error.message);
        },
      });
  }

  getEmployees() {
    this.employeeService.getEmployee().subscribe({
      next: (data: any) => {
        this.employees = data.httpResponse;
      },
      error: (error: any) => {
        this.toastr.error("Mange Leave Get Employee", error.message);
      }
    });
  }

  getLeaves() {
    this.manageLeaveService
      .getLeaves(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.leaves1;
        console.log("Leaves",this.dataRowSource);
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });

  }
  onAddClick() {
    this.manageLeave = {} as ManageLeave;
    this.router.navigate([`/addDesignation`]);
  }

  addLeave(){
    // debugger
    this.getLeaveType();
    this.getEmployees();
  }
  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.manageLeaveService
      .getLeavesById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.manageLeave = resp.httpResponse;
        if (this.manageLeave) {
          if (this.addBtn) {
            this.totalDay=this.manageLeave.leaveDay;
            this.addBtn.nativeElement.click();
          }
        }
      });
  }

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.deleteLeave = currentRowData.employee.firstName + " " + currentRowData.employee.middleName + " " + currentRowData.employee.lastName + "'s Leave";
  }

  updateTotalDays() {
    const startDate = new Date(this.manageLeave.startDate);
    const endDate = new Date(this.manageLeave.endDate);
    const timeDifference = Math.abs(endDate.getTime() - startDate.getTime());
    this.totalDay = Math.ceil(timeDifference / (1000 * 60 * 60 * 24) + 1);
    if (!Number.isNaN(this.totalDay)) {
      if (this.manageLeave.leave_duration != "Full" && this.manageLeave.leave_duration) {
        return this.manageLeave.leaveDay = (this.totalDay - 0.5).toString();
      }


      if ((this.manageLeave.leave_Start_From != "Full" || this.manageLeave.leave_End != "Full")) {
        if (this.manageLeave.leave_Start_From != "Full" && this.manageLeave.leave_End != "Full") {
          return this.manageLeave.leaveDay = (this.totalDay - 1).toString();
        }
        else {
          return this.manageLeave.leaveDay = (this.totalDay - 0.5).toString();
        }
      }

      return this.manageLeave.leaveDay = (this.totalDay).toString();

    }
  }

  ValidateModalData(): boolean {
    if (!this.manageLeave.employeeId) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageLeaveDetails.EmployeeNamerequired",(this.spinner.hide())));
      return false;
    }
    if (!this.manageLeave.leavetype) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageLeaveDetails.LeaveTypenamerequired",(this.spinner.hide())));
      return false;
    }
    if (!this.manageLeave.startDate) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageLeaveDetails.StartDaterequired",(this.spinner.hide())));
      return false;
    }
    if (!this.manageLeave.endDate) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageLeaveDetails.EndDaterequired",(this.spinner.hide())));
      return false;
    }
    return true;
  }


  applyLeave() {
    this.spinner.show();
    if (!this.manageLeave.id) {
      this.manageLeave.action = ActionEnum.Insert;
    } else {
      this.manageLeave.action = ActionEnum.Update;
    }
    if (this.ValidateModalData()) {
      this.manageLeaveService
        .applyLeave(this.manageLeave)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.closeModal();
            this.manageLeave = {} as ManageLeave;
            this.manageLeave.leave_End = "Full";
            this.manageLeave.leave_Start_From = "Full";
            this.manageLeave.leave_duration = "Full"
            this.spinner.hide();
            this.getLeaves();
          },
          error: (err: any) => {
            // // debugger
            this.toastr.error("Manage Leave", err.message);
            this.spinner.hide();
          },
        });
    }
  }
  closeModal() {
    this.manageLeave = {} as ManageLeave;
    this.totalDay=0;
    this.manageLeave.leave_End = "Full";
    this.manageLeave.leave_Start_From = "Full";
    this.manageLeave.leave_duration = "Full"
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event

      this.closeButton.nativeElement.click();
    }
  }
  gridReady(params: any) {
    this.gridApi = params.api;
  }


  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['employee.firstName', 'employee.middleName', 'employee.lastName', 'leavetypeId.typeName', 'leaveDay']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getLeaves();

  }

  getDataRowsourse(event: RequestWithFilterAndSort) {
    if (!event.sortModel) {
      event.sortModel = this.sortModel;
    }
    if (!event.filterModel) {
      event.filterModel = this.requestWithFilterAndPage.filterModel;
      event.filterConditionAndOr = this.requestWithFilterAndPage.filterConditionAndOr;
    }
    else {
      this.searchText = "";
    }
    this.requestWithFilterAndPage = event;
    this.getLeaves();
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  deleteItem() {
    this.spinner.show();
    this.manageLeaveService
      .deleteLeave(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getLeaves();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        },
      });
  }

// Notification
// GetNotificationByEmployeeId() {
//   this.notificationservice
//     .getNotificationByEmployeeId(this.SelectedEmployee)
//     .pipe(first())
//     .subscribe({
//       next: (resp: any) => {
//         debugger
//         if (resp.httpResponse) {
//           this.identity = resp.httpResponse;
//           console.log(resp.httpResponse)
        
//         }


//         //  
//       },
//       error: (err: any) => {
//         this.toastr.error(err.message);
//       },
//     });
// }




}
