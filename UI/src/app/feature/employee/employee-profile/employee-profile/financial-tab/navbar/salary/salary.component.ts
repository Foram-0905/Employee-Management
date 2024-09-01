import { ChangeDetectionStrategy, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { GridApi } from 'ag-grid-community';

import { sortModel } from '../../../../../../../shared/models/FilterRequset';
import { Observable, first } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { RequestWithFilterAndSort, filterModel } from '../../../../../../../shared/models/FilterRequset';
import { Router } from '@angular/router';
import { ActionEnum, DefaultEmployee } from '../../../../../../../shared/constant/enum.const';
import { CreditDebit } from '../../../../../../../shared/constant/enum.const';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { totalPageArray } from '../../../../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../../../../shared/helpers/common.helpers';
import { Salary } from '../../../../../../../shared/models/Salary';
import { Currency } from '../../../../../../../shared/models/currency';
import { SalaryService } from '../../../../../../../shared/services/salary.service';
import { SalaryTypeService } from '../../../../../../../shared/services/salaryType.service';
import { CurrencyService } from '../../../../../../../shared/services/currency.service';
import { TransactionType } from '../../../../../../../shared/models/TransactionType';
import { EmployeeService } from '../../../../../../../shared/services/employee.service';
import { SalaryType } from '../../../../../../../shared/models/Salary_Type';
import { PermissionService } from '../../../../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFlagService } from '../../../../../../../shared/services/EmployeFlag.service';
@Component({
  selector: 'app-salary',
  templateUrl: './salary.component.html',
  styleUrl: './salary.component.scss'
})
export class SalaryComponent implements OnInit {
  allSalary: any;
  filter: any;
  dataRowSource!: any;
  searchText: any;
  newFilterModel = {} as filterModel;
  salary = {} as Salary;
  currenccy = {} as Currency;
  transactionType = {} as TransactionType;
salaryType= {} as SalaryType;
  // SalaryType: any;
  // FCurrency: any;
  SalaryTypes : any;
  allEmployee: any;
  Transactions: any;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  editId: string = '';
  totalPages: number[] = [];
  totalItems: any;
  showEdit: boolean = true;
  showDelete: boolean = true;
  currency: any;
  deleteSalary: any;
  lastMonthTitle!: string;
  laterLastMonthTitle!: string;
  
  @Input() employeeId = new EventEmitter<string>();

  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  showDeleteModal: boolean = false;
  DeleteDesignation: any;
  private helper = new commonHelper();
  private gridApi!: GridApi;

  showCurrencyDropdown: boolean = true;
  LastMonthSalary:any;
  LaterLastMonthSalary:any;
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

  columnDefsLastMonth = [
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

  columnDefsLaterLM = [
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
  SelectedEmployee: any;

  constructor(
    private salaryservice: SalaryService,
    private salaryTypeService: SalaryTypeService,
    private permission: PermissionService,

    private currencyService: CurrencyService,
    private employeeService: EmployeeService,
    private translateService: TranslateService,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private EmployeFlage: EmployeeFlagService,
    private spinner : NgxSpinnerService,

    private totalPageArray: totalPageArray

  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "Amount";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
 
  ngOnInit(): void {

    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
      if (employee) {
        this.SelectedEmployee = employee;
        // this.salary = {} as Salary;  // Comment by AM
        this.getSalaryByEmployee( this.SelectedEmployee);
        this.getLastMonth( this.SelectedEmployee);
        this.getLaterLastMonth( this.SelectedEmployee);
        // this.salary.salaryType='-1';
      }
    });

    this.getSalaryType();
    this.getCurrency();
    this.getTransaction();
   //this.getAllEmployee();
    
    this.employeeId.emit(this.selectedEmployeeId);
    this.updateTitles();

  }
  dataRowSourceLastMonth!:any;
  getLastMonth(employeeId: string) {
    this.salaryservice
      .GetLastMonthSalary(employeeId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.LastMonthSalary = resp.httpResponse;
          if (!this.LastMonthSalary) {
            this.dataRowSourceLastMonth = [];
          } else {
            this.dataRowSourceLastMonth = this.LastMonthSalary;
          }
        },
        error: (err: any) => {
          this.toastr.error('Salary', err.message);
        },
      });
  }
  dataRowSourceLaterLM!:any;
  getLaterLastMonth(employeeId: string) {
    // debugger
    this.salaryservice
      .GetTwoMonthsAgoSalary(employeeId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // debugger
          this.LaterLastMonthSalary = resp.httpResponse;
          if (!this.LaterLastMonthSalary) {
            this.dataRowSourceLaterLM = [];
          } else {
            this.dataRowSourceLaterLM = this.LaterLastMonthSalary;
            console.warn('LaterLastMonthSalary',this.dataRowSourceLaterLM);
            
          }
          // this.toastr.success(resp.message);
          // this.LaterLastMonthSalary = resp;
          // this.dataRowSourceL = this.LaterLastMonthSalary.httpResponse;
          // console.log("Later Last Month",this.dataRowSourceL);
          
        },
        error: (err: any) => {
          // debugger
          this.toastr.error("Salary", err.message);
        },
      });
  }

  SaveSalary() {
    this.spinner.show();
    if (!this.salary.id) {
        this.salary.action = ActionEnum.Insert;
        // Use the selected employee ID for saving
        this.salary.employeeId =  this.SelectedEmployee;
    } else {
        this.salary.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
        if (this.salary.transactionType === CreditDebit.Debit) {
            this.salary.amount = -1 * this.salary.amount;
        }

        // Ensure transactionType is correctly set
        this.salary.transactionType = this.salary.transactionType;

        this.salaryservice.saveSalary(this.salary)
            .pipe(first())
            .subscribe({
                next: (resp: any) => {
                    this.toastr.success(resp.message);
                    this.salary = {} as Salary;
                    this.getSalaryByEmployee(this.SelectedEmployee); // Fetch updated data for the selected employee
                    this.selectedEmployeeId = '';
                    this.spinner.hide();
                },
                error: (err: any) => {
                    this.toastr.error("Save Salary", err.message);
                    this.spinner.hide();
                },
            });
    } else {
        this.spinner.hide();
    }
}

  ValidateModalData(): boolean {

    if (!this.salary.salaryType) {
      
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.finance-details.salary-details.SalaryTyperequired"));
      return false;
    }
  
    if (!this.salary.amount) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.finance-details.salary-details.Amountrequired",(this.spinner.hide())));
      return false;
    }
    if (!this.salary.currency) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.finance-details.salary-details.Currencynamerequired",(this.spinner.hide())));
      return false;
    }
    if (!this.salary.salaryEndDate) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.finance-details.salary-details.SalaryEndDaterequired",(this.spinner.hide())));
      return false;
    }
    if (!this.salary.salaryStartDate) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.finance-details.salary-details.SalaryStartDaterequired",(this.spinner.hide())));
      return false;
    }
   

    return true;
  }

  updateTitles() {
    const currentDate = new Date();

    // Get last month
    const lastMonthDate = new Date(currentDate.setMonth(currentDate.getMonth() - 1));
    const lastMonth = lastMonthDate.toLocaleString('default', { month: 'long' });

    // Get the month before the last month
    const laterLastMonthDate = new Date(currentDate.setMonth(currentDate.getMonth() - 1));
    const laterLastMonth = laterLastMonthDate.toLocaleString('default', { month: 'long' });

    this.lastMonthTitle = `Payment of ${lastMonth}`;
    this.laterLastMonthTitle = `Payment of ${laterLastMonth}`;
    console.log("running");
  }
  

  cancelDelete() {
    this.showDeleteModal = false;
  }

  deleteItem() {
    this.spinner.show();
    this.salaryservice
      .deleteSalary(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getSalaryByEmployee(this.SelectedEmployee);
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        },
      });
  }

  getCurrentDate(): string {
    const currentDate = new Date();
    return currentDate.toISOString().substring(0, 10); // Format as YYYY-MM-DD
  }
  selectedEmployeeId: any;
  onEmployeeSelect(event: any) {
   
    this.selectedEmployeeId = event.target.value || this.allEmployee[0]?.id ;

    this.getLaterLastMonth(this.selectedEmployeeId);

    this.getSalaryByEmployee(this.selectedEmployeeId);
    this.getLastMonth(this.selectedEmployeeId);
    // this.employeeId.emit(this.selectedEmployeeId);
    console.log("Id",this.selectedEmployeeId);
   

  }

  getSalaryType(): void {

    this.salaryTypeService.GetAllSalaryType()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.SalaryTypes = resp.httpResponse;
          
          // this.SalaryTypes.Add
          // this.salary.salaryType = this.SalaryTypes[0]?.id; // default value

        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  getTransaction(): void {
    this.salaryservice.getTransaction()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.Transactions = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  amount!: string;
  transaction!: string;

  getSalaryByEmployee(employeeId: string) {
    this.salaryservice
      .getSalaryByEmployee(employeeId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // console.warn(resp.httpResponse.id);
          // // debugger
          this.allSalary = resp.httpResponse;
          if (!this.allSalary) {
            this.dataRowSource = [];
          }
          else {
            this.dataRowSource = this.allSalary;
          }
          if (resp.httpResponse) {

            this.salaryType.salaryTypeName="Payable Salary";
            // this.currency=this.currenccy.shortWord;
            // this.dataRowSource[this.dataRowSource.length - 1].salaryTypeName =null;
            this.dataRowSource[this.dataRowSource.length - 1].salaryTypeName=  this.salaryType
         //  this.dataRowSource[this.dataRowSource.length - 1].currency  = this.currency.shortword
            this.dataRowSource[this.dataRowSource.length - 1].salaryStartDate = null
            this.dataRowSource[this.dataRowSource.length - 1].salaryEndDate = null
          }
         
          console.log("empSalary", this.allSalary)

          if (resp.httpResponse) {
            // If there are existing entries, hide the currency dropdown
            this.showCurrencyDropdown = false;
            // Populate the currency value for the read-only text box
            this.salary.currency = resp.httpResponse[0].currency; // Assuming currency is stored in the first entry
          } else {
            // If there are no existing entries, show the currency dropdown
            this.showCurrencyDropdown = true;
            this.salary.currency = ''; // Clear currency input
          }

        },
        error: (err: any) => {
          this.toastr.error("Salary", err.message);

          this.showCurrencyDropdown = true;
          this.salary.currency = ''; // Clear currency input
          this.dataRowSource = [];
        },
      });
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
  
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
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
          this.getSalaryByEmployee(initialEmployeeId);
          this.getLastMonth(initialEmployeeId);
          this.getLaterLastMonth(initialEmployeeId);
          console.log("allEmployee",this.allEmployee)
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.salaryservice
      .getSalaryById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.allSalary = resp;
        this.salary = this.allSalary.httpResponse;
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
    this.deleteSalary = currentRowData.salary;
  }


  onCategorySelect(value: any) {
    // // // // // // // console.log("🚀 ~ SalaryComponent ~ onCategorySelect ~ e:")
    this.salary.salaryType = value.target.value;

  }
  onTransactionSelect(value: any) {
    // // // // // // // console.log("🚀 ~ SalaryComponent ~ onTransactionSelect ~ onTransactionSelect:")

    this.salary.transactionType = value.target.value;
  }
  currencytype:any;
  onCurrencySelect(value: any) {
    // // // // // // // console.log("🚀 ~ SalaryComponent ~ onCurrencySelect ~ onCurrencySelect:")
    this.salary.currency = value.target.value;
    // this.salary.currency = this.currencytype;
   

  }


  //****************** For Grid Filter ****************//

  gridReady(params: any) {
    this.gridApi = params.api;
  }


  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['salaryTypeName.salaryTypeName', 'currenccy.shortWord', 'salaryStartDate', 'salaryEndDate']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    // this.getSalary();

  }

  getDataRowsourse(event: RequestWithFilterAndSort) {
    // // debugger
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

    // this.getSalary();
  }
}

