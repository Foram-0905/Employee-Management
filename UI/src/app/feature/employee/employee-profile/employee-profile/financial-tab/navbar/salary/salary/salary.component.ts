// import { ChangeDetectionStrategy, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
// import { GridApi } from 'ag-grid-community';

// import { sortModel } from '../../../../../../../../shared/models/FilterRequset';
// import { Observable, first } from 'rxjs';
// import { TranslateService } from '@ngx-translate/core';
// import { RequestWithFilterAndSort, filterModel } from '../../../../../../../../shared/models/FilterRequset';
// import { Router } from '@angular/router';
// import { ActionEnum } from '../../../../../../../../shared/constant/enum.const';
// import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
// import { ToastrService } from 'ngx-toastr';
// import { totalPageArray } from '../../../../../../../../shared/helpers/common.helpers';
// import { commonHelper } from '../../../../../../../../shared/helpers/common.helpers';
// import { Salary } from '../../../../../../../../shared/models/Salary';
// import { SalaryService } from '../../../../../../../../shared/services/salary.service';
// import { SalaryTypeService } from '../../../../../../../../shared/services/salaryType.service';
// import { CurrencyService } from '../../../../../../../../shared/services/currency.service';

// @Component({
//   selector: 'app-salary',
//   templateUrl: './salary.component.html',
//   styleUrl: './salary.component.scss'
// })
// export class SalaryComponent implements OnInit{
//   allSalary: any;
//   filter: any;
//   dataRowSource!: any[];
//   searchText: any;
//   newFilterModel = {} as filterModel;
//   salary = {} as Salary;
//   // SalaryType: any;
//   FCurrency: any;
//   SalaryTypes: any;
//   requestWithFilterAndPage = {} as RequestWithFilterAndSort;
//   sortModel = {} as sortModel;
//   editId: string = '';
//   totalPages: number[] = [];
//   totalItems: any;
//   showEdit: boolean = true;
//   showDelete: boolean = true;
//   currency: any;


//   @ViewChild('addEditModal') addEditModal!: any;
//   @ViewChild('addBtn') addBtn!: ElementRef;
//   @ViewChild('closeButton') closeButton!: ElementRef;
//   showDeleteModal: boolean = false;
//   DeleteDesignation: any;
//   private helper = new commonHelper();
//   private gridApi!: GridApi;

//   columnDefs = [
//     {
//       headerName: 'salaryType',
//       field: 'salaryTypeName.salaryTypeName',
//       filter: true,
//       filterParams: { maxNumConditions: 1 },
//     },
//     {
//       headerName: 'Amount',
//       field: 'amount',
//       filter: true,
//       filterParams: { maxNumConditions: 1 },
//     },
//     {
//       headerName: 'currency',
//       field: 'currenccy.shortWord',
//       filter: true,
//       filterParams: { maxNumConditions: 1 },
//     },
//     {
//       headerName: 'salaryStartDate',
//       field: 'salaryStartDate',
//       filter: true,
//       filterParams: { maxNumConditions: 1 },
//     },
//     {
//       headerName: 'salaryEndDate',
//       field: 'salaryEndDate',
//       filter: true,
//       filterParams: { maxNumConditions: 1 },
//     }
//   ];

//   constructor(
//     private salaryservice: SalaryService,
//     private salaryTypeService: SalaryTypeService,
//     private currencyService:CurrencyService,
//     private translateService: TranslateService,
//     private router: Router,
//     private modalService: NgbModal,
//     private toastr: ToastrService,
//     private totalPageArray: totalPageArray

//   )
//   {
//     this.requestWithFilterAndPage.pageNumber = 1;
//     this.requestWithFilterAndPage.pageSize = 5;
//     this.sortModel.colId = "Amount";
//     this.sortModel.sortOrder = "asc";
//     this.requestWithFilterAndPage.sortModel = this.sortModel;
//     this.requestWithFilterAndPage.filterModel = {};
//     this.translateService.setDefaultLang('en');
//     this.translateService.use(localStorage.getItem('lang') || 'en');
//   }
// ngOnInit(): void {
//     this.getSalary();
//     this.getSalaryType();
//     this.getCurrency();
// }
//   SaveSalary() {
//     if (!this.salary.id) {
//       this.salary.action = ActionEnum.Insert;
//       this.salary.employeeId="525c6192-13e5-4711-a28a-537557e48157";
//     } else {
//       this.salary.action = ActionEnum.Update;
//     }

//     if (this.ValidateModalData()) {
//       this.salaryservice
//         .saveSalary(this.salary)
//         .pipe(first())
//         .subscribe({
//           next: (resp: any) => {
//             this.toastr.success(resp.message);
//             this.getSalary();
//             // this.closeModal();
//             this.salary = {} as Salary;
//           },
//           error: (err: any) => {
//             this.toastr.error("Save Salary", err.message);
//           },
//         });
//     }
//   }

//   ValidateModalData(): boolean {
//     if (!this.salary.amount) {
//       this.toastr.error('Amount required');
//       return false;
//     }
//     if (!this.salary.currency) {
//       this.toastr.error('Currency name required');
//       return false;
//     }
//     if (!this.salary.salaryEndDate) {
//       this.toastr.error('SalaryEndDate required');
//       return false;
//     }
//     if (!this.salary.salaryStartDate) {
//       this.toastr.error('SalaryStartDate required');
//       return false;
//     }
//     if (!this.salary.salaryType) {
//       this.toastr.error('SalaryType required');
//       return false;
//     }

//     return true;
//   }

//   getSalaryType(): void {
//     console.log('get country');
//     this.salaryTypeService.GetAllSalaryType()
//       .pipe(first())
//       .subscribe({
//         next: (resp: any) => {
//           // this.toastr.success(resp.message);
//           // var allSLGGroup = resp;
//           this.SalaryTypes = resp.httpResponse;
//           console.log("salaryType",this.SalaryTypes)
//         },
//         error: (err: any) => {
//           this.toastr.error(err);
//         },
//       });
//   }

//   getAllSalary() {
//     this.salaryservice
//       .GetAllSalary()
//       .pipe(first())
//       .subscribe({
//         next: (resp: any) => {
//           // this.toastr.success(resp.message);
//           this.allSalary = resp;
//           // this.dataRowSource = this.allDesignation.httpResponse;
//           console.log("Salary",resp);

//         },
//         error: (err: any) => {
//           this.toastr.error("Salary", err.message);
//         },
//       });
//   }
//   getCurrency() {
//     this.currencyService
//       .getCurrencies()
//       .pipe(first())
//       .subscribe({
//         next: (resp: any) => {
//           this.currency = resp.httpResponse;
//           console.log("Currency",this.currency);

//         },
//         error: (err: any) => {
//           this.toastr.error("salary currency", err.message);
//         },
//       });
//   }

//   getSalary() {
//     this.salaryservice
//       .GetFilterSalary(
//         this.requestWithFilterAndPage
//       )
//       .pipe(first())
//       .subscribe((resp) => {
//         this.totalItems = resp.httpResponse.totalRecord;
//         this.dataRowSource = resp.httpResponse.salary;
//         console.log("salary",resp);

//         this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
//       });
//   }
//   onEditClick(currentRowData: any): void {
//     this.editId = currentRowData.id;
//     this.salaryservice
//       .getSalaryById(this.editId)
//       .pipe(first())
//       .subscribe((resp) => {
//         this.allSalary = resp;
//         this.salary = this.allSalary.httpResponse;
//         /// // debugger
//         if (this.salary) {
//           if (this.addBtn) {
//             this.addBtn.nativeElement.click();
//           }
//         }
//       });
//   }
//   closeModal() {
//     this.salary = {} as Salary;
//     // Check if the close button reference exists
//     if (this.closeButton) {
//       // Access the native element and trigger a click event
//       this.closeButton.nativeElement.click();
//     }
//   }
//   onDeleteClick(currentRowData: any) {
//     this.editId = currentRowData.id;
//     this.showDeleteModal = true;
//     this.DeleteDesignation = currentRowData.salary;
//   }


//   onCategorySelect(value: any) {
//     this.salary.salaryType = value.target.value;
//   }
//   onCurrencySelect(value: any) {
//     this.salary.currency = value.target.value;
//   }
//   //****************** For Grid Filter ****************//

//   gridReady(params: any) {
//     this.gridApi = params.api;
//   }


//   commonSearchWithinGrid() {
//     this.gridApi.setFilterModel(null);
//     // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
//     const gridColumnsToFileter: string[] = ['salaryTypeName.salaryTypeName', 'amount','currenccy.shortWord','salaryStartDate','salaryEndDate']
//     this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
//     // this.getDataRowsourse(this.requestWithFilterAndPage);
//     this.getSalary();

//   }

//   getDataRowsourse(event: RequestWithFilterAndSort) {
//     // debugger
//     if (!event.sortModel) {
//       event.sortModel = this.sortModel;
//     }
//     if (!event.filterModel) {
//       event.filterModel = this.requestWithFilterAndPage.filterModel;
//       event.filterConditionAndOr = this.requestWithFilterAndPage.filterConditionAndOr;
//     }
//     else {
//       this.searchText = "";
//     }
//     this.requestWithFilterAndPage = event;

//     this.getSalary();
//   }
// }
