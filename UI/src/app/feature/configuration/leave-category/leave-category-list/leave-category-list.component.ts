import { Component, ViewChild, ElementRef } from '@angular/core';
import { Observable, first } from 'rxjs';
import { LeaveCategory, SaveLeaveCategory } from '../../../../shared/models/leave-category';
import { sortModel } from './../../../../shared/models/FilterRequset';
import { TranslateService } from '@ngx-translate/core';
import { PermissionModule } from '../../permission/permission.module';
import { HttpClient } from '@angular/common/http';
import { RequestWithFilterAndSort } from '../../../../shared/models/FilterRequset';
import { LeaveCategoryService } from '../../../../shared/services/leave-category.service';
import { Router } from '@angular/router';
import { ColDef } from 'ag-grid-community';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import {  totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { GridApi } from 'ag-grid-community';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-leave-category-list',
  templateUrl: './leave-category-list.component.html',
  styleUrl: './leave-category-list.component.scss'
})

export class LeaveCategoryListComponent {
  PageSize !: number;
  PageNumber!: number;
  dataRowSource!: any[];
  searchText: any;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  editId: string = '';
  allLeaveCategory: any;
  leavecategory = {} as LeaveCategory;
  DeleteLeaveCategory: any;
  showEdit: boolean = true;
  showDelete: boolean = true;
  showDeleteModal: boolean = false;
  private helper = new commonHelper();
  columnDefs: ColDef[] =[]
  // columnDefs: ColDef[] = [
  //   { headerName: 'LeaveCategory', field: 'name' },];

  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addEditModal') addEditModal!: any;

  totalPages: number[] = [];
  totalItems: any;
  leaveCategories: any;
  selectedLeaveCategory: any = null; // Variable to store the currently selected leave category for editing/deleting
  private gridApi!: GridApi;
  lang!: string;
  constructor(
    private leaveCategoryService: LeaveCategoryService,
    private translateService: TranslateService,
    private permission : PermissionService,
    private spinner : NgxSpinnerService,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private totalPageArray: totalPageArray) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "name";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
  ngOnInit(): void {
    // this.getLeaveCategory();
    this.lang = localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.configuration.leaveCategoryDetails.leaveCategory'), field: 'name' },
      ];
    }, 50);
    this.getLeave();
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  getLeaveCategory() {
    this.leaveCategoryService
      .getLeaveCategory()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.allLeaveCategory = resp.httpResponse;
          // this.dataRowSource = this.allLeaveCategory

        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  getLeave() {
    this.leaveCategoryService
      .GetFilterLeaveCategory(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.leaveCategory;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }

  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.leaveCategoryService
      .getLeaveCategoryById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.allLeaveCategory = resp;
        this.leavecategory = this.allLeaveCategory.httpResponse;
        this.getLeave();
        if (this.leavecategory) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }
  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteLeaveCategory = currentRowData.name;
  }
  cancelDelete() {
    this.showDeleteModal = false;
  }

  ValidateModalData(): boolean {
    if (!this.leavecategory.name) {
      this.toastr.error(this.translateService.instant("i18n.configuration.leaveCategoryDetails.leaveCategoryrequired",(this.spinner.hide())));
      return false;
    }
    return true;
  }
  SaveleaveCategory() {
    this.spinner.show();
    if (!this.leavecategory.id) {
      this.leavecategory.action = ActionEnum.Insert;
    } else {
      this.leavecategory.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.leaveCategoryService
        .saveleaveCategory(this.leavecategory)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getLeaveCategory();
            this.getLeave();
            this.closeModal();
            this.leavecategory = {} as LeaveCategory;
            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error(err);
            this.spinner.hide();
          },
        });
    }
  }
  deleteItem() {
    this.spinner.show();
    this.leaveCategoryService
      .deleteLeaveCategory(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          // this.getLeaveCategory();
          this.getLeave();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        },
      });
  }
  closeModal() {

    if (this.closeButton) {
      this.closeButton.nativeElement.click();
      this.leavecategory = {} as LeaveCategory;
    }
  }
  gridReady(params: any) {
    this.gridApi = params.api;
  }

  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    const gridColumnsToFileter: string[] = ['name']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getLeave();

  }
  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length==0 ||inputValue.length >= 4) {
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
    }
    else {
      this.searchText = "";
    }
    this.requestWithFilterAndPage = event;

    this.getLeave();
  }
}