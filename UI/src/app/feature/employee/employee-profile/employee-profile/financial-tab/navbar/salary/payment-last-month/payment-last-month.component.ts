import { ChangeDetectionStrategy, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { GridApi } from 'ag-grid-community';

import { sortModel } from '../../../../../../../../shared/models/FilterRequset';
import { Observable, first } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { RequestWithFilterAndSort, filterModel } from '../../../../../../../../shared/models/FilterRequset';
import { Router } from '@angular/router';
import { ActionEnum } from '../../../../../../../../shared/constant/enum.const';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { totalPageArray } from '../../../../../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../../../../../shared/helpers/common.helpers';
import { Salary } from '../../../../../../../../shared/models/Salary';
import { SalaryService } from '../../../../../../../../shared/services/salary.service';
import { SalaryTypeService } from '../../../../../../../../shared/services/salaryType.service';
import { CurrencyService } from '../../../../../../../../shared/services/currency.service';
import { EmployeeService } from '../../../../../../../../shared/services/employee.service';

@Component({
  selector: 'app-payment-last-month',
  templateUrl: './payment-last-month.component.html',
  styleUrl: './payment-last-month.component.scss'
})
export class PaymentLastMonthComponent implements OnInit{
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  editId: string = '';
  totalPages: number[] = [];
  totalItems: any;
  showEdit: boolean = false;
  showDelete: boolean = false;
  currency: any;
  dataRowSource!: any[];
  LastMonthSalary:any;
  salary = {} as Salary;

  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  showDeleteModal: boolean = false;
  DeleteLastMonth: any;
  private helper = new commonHelper();
  private gridApi!: GridApi;

  columnDefs = [
    {
      headerName: 'salaryType',
      field: 'salaryTypeName.salaryTypeName',
      filter: true,
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: 'Amount',
      field: 'amount',
      filter: true,
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: 'currency',
      field: 'currenccy.shortWord',
      filter: true,
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: 'salaryStartDate',
      field: 'salaryStartDate',
      filter: true,
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: 'salaryEndDate',
      field: 'salaryEndDate',
      filter: true,
      filterParams: { maxNumConditions: 1 },
    }
  ];
  selectedEmployeeId!:'';
  allEmployee: any;
  ngOnInit(): void {
      this.getLastMonth(this.selectedEmployeeId);
      this.getCurrency();
      this.getAllEmployee();
  }
  logEmployeeId(employeeId: string | undefined) {
    console.log("Received Employee ID:", employeeId); // Log received employee ID to console
  }
  constructor(
    private salaryservice: SalaryService,
    private currencyService:CurrencyService,
    private employeeService: EmployeeService,
    private translateService: TranslateService,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private totalPageArray: totalPageArray

  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
   
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  getLastMonth(employeeId:string) {
    this.salaryservice
      .GetLastMonthSalary(employeeId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          this.LastMonthSalary = resp;
          this.dataRowSource = this.LastMonthSalary.httpResponse;
          console.log("Last Month",this.dataRowSource);
          
        },
        error: (err: any) => {
          this.toastr.error("Salary", err.message);
        },
      });
  }
  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.salaryservice
      .getSalaryById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.LastMonthSalary = resp;
        this.salary = this.LastMonthSalary.httpResponse;
        /// // debugger
        if (this.salary) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }
  closeModal() {
    this.salary = {} as Salary;
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
    }
  }
  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteLastMonth = currentRowData.salary;
  }

  getCurrency() {
    this.currencyService
      .getCurrencies()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.currency = resp.httpResponse;
          console.log("Currency",this.currency);
          
        },
        error: (err: any) => {
          this.toastr.error("salary currency", err.message);
        },
      });
  }

  getAllEmployee(): void {

    this.employeeService.getEmployee()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          // var allSLGGroup = resp;
          this.allEmployee = resp.httpResponse;
          const initialEmployeeId = this.allEmployee[0].id; // Assuming allEmployee is populated with employee objects having 'id' property
          this.getLastMonth(initialEmployeeId);
          console.log("LastallEmployee",this.allEmployee)
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

}
