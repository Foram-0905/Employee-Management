import { PermissionService } from './../../../../shared/services/permission.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '../../../../shared/services/employee.service';
import { ContactAddressService } from '../../../../shared/services/contactaddress.service';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { RequestWithFilterAndSort, sortModel } from '../../../../shared/models/FilterRequset';
import { first } from 'rxjs';
import { Router } from '@angular/router';
import {EmployeeFlagService} from '../../../../shared/services/EmployeFlag.service'

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit {
  PageSize!: number;
  PageNumber!: number;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  showDeleteModal: boolean = false;
  dataRowSource!: any[];
  totalPages: number[] = [];
  totalItems: any;
  private helper = new commonHelper();
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addBtn') addBtn!: ElementRef;
  showEdit: boolean = true;
 

  columnDefs = [
    { headerName: 'Employe Number(ID)', field: 'employeeNumber', sortable: false, filter: true },
    { headerName: 'Name', field: 'fullName', sortable: false, filter: true },
    { headerName: 'Type', field: 'employementTypename', sortable: false, filter: true },
    { headerName: 'Office Email', field: 'officeEmail', sortable: false, filter: true },
    { headerName: 'Work Location', field: 'workLocation', sortable: false, filter: true },
    { headerName: 'Start Work Date', field: 'contractualStartDate', sortable: false, filter: true },
    { headerName: 'Phone Number', field: 'phoneNumber', sortable: false, filter: true },
    { headerName: 'Employee Status', field: 'employeeStatusname', sortable: false, filter: true },
    { headerName: 'Role', field: 'rolename', sortable: false, filter: true },
  ];

  constructor(
    private toastr: ToastrService,
    private employeservice: EmployeeService,
    private totalPageArray: totalPageArray,
    private router: Router,
    private EmployeFlage:EmployeeFlagService,
    private permission: PermissionService,
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "officeEmail";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
  }

  ngOnInit(): void {
    
    this.getFilterEmployee();
  }

  getFilterEmployee() {
    this.employeservice
      .getFilterEmployee(this.requestWithFilterAndPage)
      .pipe(first())
      .subscribe((resp) => {
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.employee;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }
 
 AddEmployee() {
  //localStorage.removeItem('SelectedEmployeeForEdit');
  this.router.navigate(['/employee/employeeprofile'], { queryParams: { flag: true } });
}


  currentId!: string;

  onEditClick(currentRowData: any): void {
    this.currentId = currentRowData.id;
    this.router.navigate(['/employee/employeeprofile'], { queryParams: { id: this.currentId, Editflag: false } });
    this.onEditClicks(currentRowData);

  }

  onEditClicks(currentRowData: any): void {
    const currentId = currentRowData.id;
    const editFlag = false;

    // Set the service with the currentId and editFlag
    this.EmployeFlage.setEmployeeParams(currentId, editFlag);

   
  }

  searchText: any;
  sortModel = {} as sortModel;
  gridApi: any;

  gridReady(params: any) {
    this.gridApi = params.api;
  }

  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    const gridColumnsToFileter: string[] = ['employeeNumber', 'FullName', 'employementTypename', 'officeEmail', 'workLocation', 'rolename', 'employeeStatusname', 'phoneNumber'];
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    this.getFilterEmployee();
  }

  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length == 0 || inputValue.length >= 4) {
      this.commonSearchWithinGrid();
    }
  }

  getDataRowsourse(event: RequestWithFilterAndSort) {
    if (!event.sortModel) {
      event.sortModel = this.sortModel;
    }
    if (!event.filterModel) {
      event.filterModel = this.requestWithFilterAndPage.filterModel;
      event.filterConditionAndOr = this.requestWithFilterAndPage.filterConditionAndOr;
    } else {
      this.searchText = "";
    }
    this.requestWithFilterAndPage = event;
    this.getFilterEmployee();
  }
  hasPermission(permission: any) {
    // debugger
     return this.permission.hasPermission(permission);
   }
}
