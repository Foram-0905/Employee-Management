import { permission } from './../../../shared/models/permission';
import { Component, ComponentFactoryResolver, ElementRef, OnInit, Type, ViewChild, ViewContainerRef } from '@angular/core';
import { Router } from '@angular/router';
import { LeaveService } from '../../../shared/services/leave.service';
import { first, take } from 'rxjs';
import { LoginService } from '../../../core/services/login.service';
import { annualLeaveOfficemangement, ApprovOrRejectLeave, typesOFLeaves } from '../../../shared/models/employee-leave';
import { leaveTypeService } from '../../../shared/services/leaveType.service';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { leaveAccordingDate, leavesAccordingLogin, ManageLeave } from '../../../shared/models/manage-leave';
import { ToastrService } from 'ngx-toastr';
import { ActionEnum } from '../../../shared/constant/enum.const';
import { ManageLeaveService } from '../../../shared/services/manage-leave.service';
import { EmployeeService } from '../../../shared/services/employee.service';
import { RequestWithFilterAndSort, sortModel } from '../../../shared/models/FilterRequset';
import { totalPageArray } from '../../../shared/helpers/common.helpers';
import { CalenderComponent } from './calender/calender.component';
import { NgxSpinnerService } from "ngx-spinner";
import { PermissionService } from '../../../shared/services/permission.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-leave',
  templateUrl: './leave.component.html',
  styleUrl: './leave.component.scss'
})
export class LeaveComponent implements OnInit {
  activeTab: any = "employeeLeaveBalance";
  showApprovButton: boolean = true;
  showRejectButton: boolean = true;
  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, interactionPlugin],
    dateClick: (args: any) => this.onDateClick(args),
    initialView: 'dayGridMonth',
    weekends: true,
  };

  // annualLeave = [
  //   { headerName: 'Annual leave available for current year', field: 'available', sortable: false, filter: true },
  //   { headerName: 'Already taken', field: 'taken', sortable: false, filter: true },
  //   { headerName: 'Remaining Annual Leave Days', field: 'remainingDays', sortable: false, filter: true },
  //   { headerName: 'Pending Leave Days', field: 'pendingDays', sortable: false, filter: true },
  // ];

  coluDefForCalenderClick = [
    { headerName: 'Employee Name', field: 'employeeName', sortable: false, filter: false },
    { headerName: 'Type of Leave', field: 'leavetypeName', sortable: false, filter: false },
    { headerName: 'Start Date', field: 'startDate', sortable: false, filter: false },
    { headerName: 'End Date', field: 'endDate', sortable: false, filter: false },
    { headerName: 'Applied on', field: 'appliedDate', sortable: false, filter: false }
  ]

  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addBtn') addBtn!: ElementRef;
  employeePendingLeaves: any;
  employeeLeavesHistory: any;
  LoginUser: any;
  allLeaveType: any[] = [];
  employeeLeaves: any;
  typesOFLeaves = {} as typesOFLeaves;
  displayLeaveType: any[] = [];
  leaves: any[] = [];
  annualLeaves: any;
  employeeannualLeave: any[] = [];
  penddingDay: number = 0;
  remainingDays: number = 0;
  taken: number = 0;
  available: number = 0;
  currentDate: string;
  employeeName: any;
  manageLeave = {} as ManageLeave;
  leavesAccordingLogin = {} as leavesAccordingLogin;
  leaveAccordingDate = {} as leaveAccordingDate;
  editLeaveId: any;
  showDeleteModal: boolean = false;
  deleteLeave: any;
  sortModel = {} as sortModel;
  requestWithFilterAndSort = {} as RequestWithFilterAndSort;
  employeeList: any;
  totalPages: number[] = [];
  totalItems: any;
  totalPagesHistory: number[] = [];
  totalItemsHistory: any;
  totalDay: any = 0;
  allLeave: any;
  showEmployeeLeave: boolean = false;
  selectedDate: any;
  // filterModelByComponet={} as filterModelByComponet;
  constructor(private leaveService: LeaveService, private permission: PermissionService, private spinner: NgxSpinnerService, private vcr: ViewContainerRef, private resolver: ComponentFactoryResolver, 
    private translateService:TranslateService,
    private totalPageArray: totalPageArray, private employeservice: EmployeeService, private manageLeaveService: ManageLeaveService, private toastr: ToastrService, private loginService: LoginService, private leaveTypeService: leaveTypeService) {
    this.currentDate = new Date().toISOString().split('T')[0];
    this.requestWithFilterAndSort.pageNumber = 1;
    this.requestWithFilterAndSort.pageSize = 5;
    this.sortModel.colId = "appliedDate";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndSort.sortModel = this.sortModel;
    this.requestWithFilterAndSort.filterModel = {};
    this.leavesAccordingLogin.filterRequset = this.requestWithFilterAndSort;
    this.leaveAccordingDate.filterRequset = this.requestWithFilterAndSort;
  }
  ngOnInit(): void {
    this.activeTab = "employeeLeaveBalance";
    this.getLoginUser();
    this.getLeaveType();
    this.manageLeave.leave_End = "Full";
    this.manageLeave.leave_Start_From = "Full";
    this.manageLeave.leave_duration = "Full"
  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  toggleActive(tab: any) {
    this.activeTab = tab;
    this.leavesAccordingLogin.pageType = this.activeTab;
    delete this.requestWithFilterAndSort.filterModel["employeeName"]
    if (this.activeTab == 'officeManagementBalance') {
      this.getEmployeeByLeader();
      this.leavesAccordingLogin.pageType = this.activeTab;
      this.leavesAccordingLogin.id = this.LoginUser.name;
      this.leaveAccordingDate.id = this.LoginUser.name;
    }
    this.GetEmployeeHistoryLeaves(this.leavesAccordingLogin);
    this.GetEmployeePendingLeaves(this.leavesAccordingLogin);
  }

  async getLoginUser() {

    this.LoginUser = this.loginService.currentUserValue;
    if (this.activeTab == 'employeeLeaveBalance') {
      this.leavesAccordingLogin.pageType = this.activeTab;
      this.leavesAccordingLogin.id = this.LoginUser.name;
    }
    else {
      this.leavesAccordingLogin.pageType = this.activeTab;
      this.leavesAccordingLogin.id = this.LoginUser.name;
      this.leaveAccordingDate.id = this.LoginUser.name;
      this.getEmployeeByLeader();
    }
    // this.getEmployeeById(this.employeeId.name);
    this.GetEmployeeHistoryLeaves(this.leavesAccordingLogin);
    this.GetEmployeePendingLeaves(this.leavesAccordingLogin);


  }

  getEmployeeById(id: any) {
    this.employeservice.getEmployeeById(id).pipe(first())
      .subscribe((resp) => {
        this.available = resp.httpResponse.annualLeaveEntitlement;
      });
  }
 
  // annualLeaveData() {
  //   this.employeeannualLeave=[];
  //   if (this.employeeLeaves) {
  //     this.annualLeaves = this.employeeLeaves.filter((item: any) => item.leavetypeId.typeName == 'Annual Leave' && (new Date(item.startDate)).getFullYear() == (new Date()).getFullYear());
  //     this.annualLeaves.forEach((element: any) => {
  //       if (!element.isApproved && !element.isRejected) {
  //         this.penddingDay = this.penddingDay + Number(element.leaveDay);
  //       }
  //       if (element.isApproved) {
  //         this.taken = this.taken + Number(element.leaveDay);
  //       }
  //     });
  //     this.remainingDays = this.available - this.taken;
  //     const annualLeave = {
  //       available: 10,
  //       taken: this.taken,
  //       remainingDays: this.remainingDays,
  //       pendingDays: this.penddingDay
  //     }
  //     this.employeeannualLeave.push(annualLeave);
  //   }
  // }
  

  GetAllLeaves() {
    this.leaveService.GetAllLeaves().pipe(first())
      .subscribe({
        next: (resp: any) => {

        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  onRejectLeave(params: any) {
    var leave = {} as ApprovOrRejectLeave
    if (params.approvedbyTeamlead) {
      leave = {
        id: params.id,
        approvedBy: "TeamLead",
        approvedByName: this.leavesAccordingLogin.id,
        approvedOrreject: 'Rejecte',
      }
    }
    if (params.approvedbyOfficeManagement) {
      leave = {
        id: params.id,
        approvedBy: "OfficeMangement",
        approvedByName: this.leavesAccordingLogin.id,
        approvedOrreject: 'Rejecte',
      }
    }
    this.spinner.show();
    this.leaveService
      .ApprovOrRejectLeave(leave)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.spinner.hide();
          this.toastr.success(resp.message);
          this.GetEmployeeHistoryLeaves(this.leavesAccordingLogin);
          this.GetEmployeePendingLeaves(this.leavesAccordingLogin);

        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  onApproveLeave(params: any) {
    var leave = {} as ApprovOrRejectLeave
    if (params.approvedbyTeamlead) {
      leave = {
        id: params.id,
        approvedBy: "TeamLead",
        approvedByName: this.leavesAccordingLogin.id,
        approvedOrreject: 'Approve',
      }
    }
    if (params.approvedbyOfficeManagement) {
      leave = {
        id: params.id,
        approvedBy: "OfficeMangement",
        approvedByName: this.leavesAccordingLogin.id,
        approvedOrreject: 'Approve',
      }
    }
    this.spinner.show();
    this.leaveService
      .ApprovOrRejectLeave(leave)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {

          this.toastr.success(resp.message);
          this.GetEmployeeHistoryLeaves(this.leavesAccordingLogin);
          this.GetEmployeePendingLeaves(this.leavesAccordingLogin);
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  GetEmployeeHistoryLeaves(leavesAccordingLogin: any) {
    this.leaveService
      .GetEmployeeHistoryLeaves(leavesAccordingLogin)
      .pipe(first())
      .subscribe((resp) => {
        this.employeeLeavesHistory = resp.httpResponse.leaves1;
        this.totalItemsHistory = resp.httpResponse.totalRecord;
        this.totalPagesHistory = this.totalPageArray.GetTotalPageArray(this.totalItems, this.leavesAccordingLogin.filterRequset?.pageSize);
        console.log("aall", this.totalPagesHistory, this.totalItemsHistory);

        this.countTakenLeave();
        // this.setLeaveInCalendar();
      });
  }

  GetEmployeePendingLeaves(leavesAccordingLogin: any) {
    this.leaveService
      .GetEmployeePendingLeaves(leavesAccordingLogin)
      .pipe(first())
      .subscribe((resp) => {
        this.employeePendingLeaves = resp.httpResponse.leaves1;
        console.log(this.employeePendingLeaves);
        this.totalItems = resp.httpResponse.totalRecord;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.leavesAccordingLogin.filterRequset?.pageSize);
        this.countTakenLeave();
        this.setLeaveInCalendar();
        // this.setLeaveInCalendar();
      });
  }

  getLeaveType() {
    this.leaveTypeService
      .getLeaves().pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.allLeaveType = resp.httpResponse;
          this.countTakenLeave();
        }
      });
  }

  countTakenLeave() {
    this.displayLeaveType = [];
    if (this.allLeaveType) {
      this.allLeaveType.forEach(element => {
        let taken = 0
        if (this.employeePendingLeaves) {
          this.employeePendingLeaves.filter((item: any) => item.leavetypeName == element.typeName).forEach((element1: any) => {
            taken = taken + Number(element1.leaveDay);
          });
        }
        if (this.employeeLeavesHistory) {

          this.employeeLeavesHistory.filter((item: any) => item.leavetypeName == element.typeName).forEach((element1: any) => {
            taken = taken + Number(element1.leaveDay);
          });

          const typesOFLeaves = {
            leaveType: element.typeName,
            takenLeave: taken,
          };
          this.displayLeaveType.push(typesOFLeaves);
        }
      });
    }

  }

  setLeaveInCalendar() {
    this.calendarOptions.events = "";
    this.leaves = [];

    if (this.employeeLeavesHistory != null) {
      if (this.employeeLeavesHistory.length != 0) {
        this.employeeLeavesHistory.forEach((element: any) => {
          let color: string;
          switch (element.leavetypeName) {
            case 'Sick Leave':
              color = "Red";
              break;
            case 'Annual Leave':
              color = "Green";
              break;
            case 'Special Leave':
              color = "#6b33e0";
              break;
            default:
              color = '#52575d';
              break;
          }
          let endDate = new Date(element.endDate);
          let nextDayTimestamp = endDate.setDate(endDate.getDate() + 1);
          const leave = {
            title: '',
            start: element.startDate,
            end: new Date(new Date(element.endDate).setDate(new Date(element.endDate).getDate() + 1)).toISOString().split('T')[0],
            color: color
          };

          this.leaves.push(leave);

        });
      }
    }

    if (this.employeePendingLeaves.length != 0) {
      this.employeePendingLeaves.forEach((element: any) => {
        let color: string;
        switch (element.leavetypeName) {
          case 'Sick Leave':
            color = "Red";
            break;
          case 'Annual Leave':
            color = "Green";
            break;
          case 'Special Leave':
            color = "#6b33e0";
            break;
          default:
            color = '#52575d';
            break;
        }
        let endDate = new Date(element.endDate);
        let nextDayTimestamp = endDate.setDate(endDate.getDate() + 1);
        const leave = {
          title: '',
          start: element.startDate,
          end: new Date(new Date(element.endDate).setDate(new Date(element.endDate).getDate() + 1)).toISOString().split('T')[0],
          color: color
        };

        this.leaves.push(leave);
      });
    }

    this.calendarOptions.events = this.leaves;
    this.loadComponent(CalenderComponent);
  }

  async loadComponent(componentType: Type<any>): Promise<void> {
    this.vcr.clear();
    if (!componentType) {
      console.error('Component type is undefined');
      return;
    }
    const componentFactory = this.resolver.resolveComponentFactory(componentType);
  }


  closeModal() {
    this.manageLeave = {} as ManageLeave;
    this.totalDay = 0;
    this.manageLeave.leave_End = "Full";
    this.manageLeave.leave_Start_From = "Full";
    this.manageLeave.leave_duration = "Full"
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
    }
  }

  openModel() {
    // Check if the close button reference exists

    if (this.addBtn) {

      this.manageLeave.endDate = ""; // Clear end date
      // Access the native element and trigger a click event
      this.addBtn.nativeElement.click();
    }

  }

  ValidateModalData(): boolean {

    if (!this.manageLeave.employeeId) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageLeaveDetails.EmployeeIdRequired",(this.spinner.hide())));
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

  getEmployeeByLeader() {
    // debugger
    if (this.LoginUser.role == "HR Manager") {
      this.employeservice.getEmployeeByHr(this.LoginUser.name).subscribe({
        next: (data: any) => {
          this.employeeList = data.httpResponse;
          console.log("empHR", this.employeeList);
        },
        error: (error: any) => {
          this.toastr.error("Mange Leave Get Employee", error.message);
        }
      });
    }
    else {
      this.employeservice.getEmployeeByLeader(this.leavesAccordingLogin.id).subscribe({
        next: (data: any) => {
          this.employeeList = data.httpResponse;
          console.log("emp", this.employeeList);
        },
        error: (error: any) => {
          this.toastr.error("Mange Leave Get Employee", error.message);
        }
      });
    }
  }

  applyLeave() {
    this.spinner.show();
    if (!this.manageLeave.id) {
      this.manageLeave.action = ActionEnum.Insert;
      this.manageLeave.employeeId = this.leavesAccordingLogin.id;
    } else {
      this.manageLeave.action = ActionEnum.Update;
    }
    // this.manageLeave.employeeId = this.employeeId;
    console.log("apply", this.manageLeave);

    if (this.ValidateModalData()) {
      this.leaveService
        .applyLeave(this.manageLeave)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.closeModal();
            this.GetEmployeeHistoryLeaves(this.leavesAccordingLogin);
            this.GetEmployeePendingLeaves(this.leavesAccordingLogin);
            this.manageLeave = {} as ManageLeave;
            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error("Manage Leave", err.message);
            this.spinner.hide();
          },
        });
    }
  }

  onDateClick(arg: any) {
    if (this.activeTab == "employeeLeaveBalance") {
      this.manageLeave.startDate = arg.dateStr;
      this.openModel();
    }
    else {
      this.openEmployeLeave(arg.dateStr)
    }
  }

  openEmployeLeave(date: any) {
    this.getCalenderClickLeave(date)
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

  onEditClick(currentRowData: any): void {
    this.manageLeave = currentRowData;
    if (this.manageLeave) {
      this.totalDay = this.manageLeave.leaveDay;
      this.openModel();
    }
  }

  onDeleteClick(currentRowData: any) {
    this.editLeaveId = currentRowData.id;
    this.showDeleteModal = true;
    this.deleteLeave = currentRowData.startDate + " TO " + currentRowData.endDate + "'s Leave";
  }

  cancelDelete() {
    this.showDeleteModal = false;
    this.showEmployeeLeave = false;
  }

  deleteItem() {
    this.spinner.show();
    this.manageLeaveService
      .deleteLeave(this.editLeaveId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.GetEmployeeHistoryLeaves(this.leavesAccordingLogin);
          this.GetEmployeePendingLeaves(this.leavesAccordingLogin);
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        },
      });
  }

  onSelectEmployee(employee: any) {
    // debugger
    if (employee.filter == "All") {
      delete this.requestWithFilterAndSort.filterModel["employeeName"]
    }
    else {
      this.requestWithFilterAndSort.filterModel["employeeName"] = employee;
    }

    this.GetEmployeeHistoryLeaves(this.leavesAccordingLogin);
    this.GetEmployeePendingLeaves(this.leavesAccordingLogin);

  }

  getHistoryDataRowsourse(event: RequestWithFilterAndSort) {
    if (!event.sortModel) {
      event.sortModel = this.sortModel;
    }
    if (!event.filterModel) {
      event.filterModel = this.leavesAccordingLogin.filterRequset?.filterModel;
      event.filterConditionAndOr = this.leavesAccordingLogin.filterRequset?.filterConditionAndOr;
    }
    this.leavesAccordingLogin.filterRequset = event;
    this.GetEmployeeHistoryLeaves(this.leavesAccordingLogin);
  }

  getPendingDataRowsourse(event: RequestWithFilterAndSort) {
    if (!event.sortModel) {
      event.sortModel = this.sortModel;
    }
    if (!event.filterModel) {
      event.filterModel = this.leavesAccordingLogin.filterRequset?.filterModel;
      event.filterConditionAndOr = this.leavesAccordingLogin.filterRequset?.filterConditionAndOr;
    }
    this.leavesAccordingLogin.filterRequset = event;
    this.GetEmployeePendingLeaves(this.leavesAccordingLogin);
  }

  getCalenderClickLeave(date: any) {
    this.leaveAccordingDate.date = date;
    // this.employeeLeaves.push(this.employeeLeavesHistory.filter((x:any)=>x.startDate>=date && x.endDate<=date));
    // console.log("sasasa",this.employeePendingLeaves);
    this.selectedDate = date;
    console.log(this.leaveAccordingDate);

    this.leaveService
      .GetLeaveByDate(this.leaveAccordingDate)
      .pipe(first())
      .subscribe((resp) => {
        this.employeeLeaves = resp.httpResponse.leaves1;
        this.totalItems = resp.httpResponse.totalRecord;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.leavesAccordingLogin.filterRequset?.pageSize);
        this.showEmployeeLeave = true;
        // this.setLeaveInCalendar();
      });
  }
}
