import { Component, ElementRef,OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable, first } from 'rxjs';
import { EmployeeService } from '../../../../../../../../shared/services/employee.service';
import { SalaryTypeService } from '../../../../../../../../shared/services/salarytypes.service';
import { BonusService } from '../../../../../../../../shared/services/bonus.service';
import { ActionEnum, DefaultEmployee } from '../../../../../../../../shared/constant/enum.const';
import { Bonus,SaveBonus } from '../../../../../../../../shared/models/bonus';
import { employee } from '../../../../../../../../shared/models/employee';
import { SalaryType } from '../../../../../../../../shared/models/salarytype';
import { sortModel, RequestWithFilterAndSort, filterModel } from '../../../../../../../../shared/models/FilterRequset';
import { GridApi } from 'ag-grid-community';
import { totalPageArray } from '../../../../../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../../../../../shared/helpers/common.helpers';
import { PermissionService } from '../../../../../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFlagService } from '../../../../../../../../shared/services/EmployeFlag.service';
@Component({
  selector: 'app-bonus1',
  templateUrl: './bonus.component.html',
  styleUrl: './bonus.component.scss',

})
export class BonusComponent1 implements OnInit {
  PageSize!: number;
  PageNumber!: number;
  dataRowSource!: any[];
  totalPages: number[] = [];
  totalItems: any;
  columnDefs:any[]=[];
  // columnDefs = [
  //   { headerName: 'Bonus', field: 'entitlement', sortable: false, filter: true },
  //   { headerName: 'Starting Date', field: 'startingDate', sortable: false, filter: true },
  //   { headerName: 'Ending Date', field: 'endingDate', sortable: false, filter: true },
  //   { headerName: 'Amount', field: 'bonusamount', sortable: false, filter: true },
  //   { headerName: 'Salary-Type', field: 'salarytype.salaryTypeName', sortable: false, filter: true },
  // ];
  employees: employee[] = [];
  bonus:Bonus[]=[];
  savebonus = {} as SaveBonus;
  sortModel = {} as sortModel;
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  private helper = new commonHelper();
  private gridApi!: GridApi;
  editId: any;
  showEdit: boolean = true;
  showDelete: boolean = true;
  showCheckbox: boolean = true;
  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  showDeleteModal: boolean = false;
  searchText: any;
  DeleteBonus: any;
  bonusdata: any;
  salarytypedata:any;
  bonusResponse: any;
  public bonusEntitlement: boolean = false;
  SelectedEmployee: any;




  constructor(
    private router: Router,
    private toastr: ToastrService,
    private bonusService: BonusService,
    private employeeService: EmployeeService,
    private permission:PermissionService,
    private salarytypeService: SalaryTypeService,
    private translateService: TranslateService,
    private totalPageArray: totalPageArray,
    private spinner:NgxSpinnerService,
    private EmployeFlage: EmployeeFlagService,
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "bonusamount";  
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
  }

  ngOnInit(): void {

    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
      if (employee) {
        this.SelectedEmployee = employee;
        this.getFilterBonus();
      }
    });

    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.employeeProfile.finance-details.bonusDetails.bonusentitlement'), field: 'entitlement', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.employeeProfile.finance-details.bonusDetails.startingDate'), field: 'startingDate', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.employeeProfile.finance-details.bonusDetails.endingDate'), field: 'endingDate', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.employeeProfile.finance-details.bonusDetails.bonusAmount'), field: 'bonusamount', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.employeeProfile.finance-details.bonusDetails.salaryType'), field: 'salarytype.salaryTypeName', sortable: false, filter: true },
      ];
    }, 50);
    // this.getBonus();
    this.getSalaryType()
    this.getFilterBonus();
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  getBonus(): void {
    this.bonusService
      .getBonus()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.bonusdata = resp;
        },
        error: (err: any) => {
          this.toastr.error("Bonus", err.message);
        },
      });
  }
  getSalaryType(): void {
    this.salarytypeService.getSalaryType()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.salarytypedata = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  getFilterBonus() {
    this.bonusService
      .getFilterBonus(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {

        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.bonus.filter((bonus: any) => bonus.employeeId == this.SelectedEmployee);
        ;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
        // console.log("Bonus",this.dataRowSource);
      });
  }

  SaveBonus(): void {
    this.spinner.show();
    if (!this.savebonus.id) {
      this.savebonus.action = ActionEnum.Insert;
      this.savebonus.employeeId = this.SelectedEmployee;
    } else {
      this.savebonus.action = ActionEnum.Update;
    }

    if (!this.savebonus.entitlement) {
      this.savebonus.entitlement= false;
      this.savebonus.startingDate = null;
      this.savebonus.endingDate = null;
      this.savebonus.bonusamount = '';
      this.savebonus.salaryType = '';
    }
    // console.log('Bonus data:', this.savebonus);

    if (this.ValidateModalData()) {
      this.bonusService.saveBonus(this.savebonus)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getFilterBonus();
            this.getBonus();
            this.savebonus = {} as SaveBonus;
            this.spinner.hide();
          },
          error: (err: any) => {
            this.spinner.hide();
            this.toastr.error(err);
          },
        });
    }
  }
  onEditClick(currentRowData: any): void {
    // console.log("edit click")
    this.editId = currentRowData.id;
    this.bonusService
      .getBonusById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.bonusResponse = resp;
        this.savebonus = this.bonusResponse.httpResponse;
        this.getFilterBonus();
        this.getBonus();
        /// // debugger
        if (this.savebonus) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }
  // toggleOptions() {
  //   if (!this.savebonus.entitlement) {
  //     this.savebonus.entitlement= false;
  //     this.savebonus.startingDate = null;
  //     this.savebonus.endingDate = null;
  //     this.savebonus.bonusamount = '';
  //     this.savebonus.salaryType = '';
  //   }
  // }



  onSalarytypeSelect(value: any) {
    this.savebonus.salaryType = value.target.value;
  }

  onAddClick() {
    this.router.navigate([`/SaveBonus`]);
  }
  cancelDelete() {
    this.showDeleteModal = false;
  }
  onCheckboxChange(event: any) {
    this.savebonus.entitlement = event.target.checked;
}


 
ValidateModalData(): boolean {
  // Skip validation for "Entitlement" when it's false

  if (!this.savebonus.startingDate) {
    this.toastr.error(this.translateService.instant("i18n.employeeProfile.finance-details.bonusDetails.startingDateRequired",(this.spinner.hide())));
    return false;
  }  
  if (!this.savebonus.endingDate) {
    this.toastr.error(this.translateService.instant("i18n.employeeProfile.finance-details.bonusDetails.endingDateRequired",(this.spinner.hide())));
    return false;
  }  
  if (!this.savebonus.bonusamount) {
    this.toastr.error(this.translateService.instant("i18n.employeeProfile.finance-details.bonusDetails.BonusamountRequired",(this.spinner.hide())));
    return false;
  } 
  if (!this.savebonus.salaryType) {
    this.toastr.error(this.translateService.instant("i18n.employeeProfile.finance-details.bonusDetails.SalaryTyperequired",(this.spinner.hide())));
    return false;
  }
  return true;
}

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteBonus = currentRowData.savebonus; // You can change this to any property you want to display
  }
  deleteItem() {
    this.bonusService
      .deleteBonus(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getFilterBonus();
          this.getBonus();
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  onPageSizeChange(pageSize: number) {
    this.PageSize = pageSize;
  }

  onPageNumberChange(pageNumber: number) {
    this.PageNumber = pageNumber;
  }
  gridReady(params: any) {
    this.gridApi = params.api;
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

    this.getSalaryType();
  }
}
