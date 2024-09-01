import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { LeaveType } from '../../../../shared/models/leave-type';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { leaveTypeService } from '../../../../shared/services/leaveType.service';
import { first } from 'rxjs';
import { ColDef } from 'ag-grid-community';
import { LeaveCategoryService } from '../../../../shared/services/leave-category.service';
import { RequestWithFilterAndSort, sortModel } from '../../../../shared/models/FilterRequset';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { GridApi } from 'ag-grid-community';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-leave-type-list',
  templateUrl: './leave-type-list.component.html',
  styleUrl: './leave-type-list.component.scss'
})
export class LeaveTypeListComponent implements OnInit {
  showEdit: boolean = true;
  showDelete: boolean = true;
  leavetype = {} as LeaveType;
  dataRowSource!: any[];
  allLeaveType: any;
  Leavetypes: any;
  editId: string = '';
  showDeleteModal: boolean = false;
  deleteLeaveType: any;
  totalPages: number[] = [];
  totalItems: any;
  private helper = new commonHelper();
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  private gridApi!: GridApi;
  searchText: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addEditModal') addEditModal!: any;
  columnDefs: ColDef[] = []
  lang!: string;
  // columnDefs: ColDef[] = [

  //   { headerName: 'Category', field: 'leaveCategory.name' },
  //   { headerName: 'Leave Type', field: 'typeName' },

  // ];

  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.configuration.leaveTypeDetails.Category'), field: 'leaveCategory.name' },
        { headerName: this.translateEnumValue('i18n.configuration.leaveTypeDetails.LeaveType'), field: 'typeName' },      ];
    }, 50);
    this.getLeaveType();
    this.getLeaveCategory();

  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  constructor(private translateService: TranslateService, private toastr: ToastrService, private leaveTypeService: leaveTypeService,
    private leaveCategoryService:LeaveCategoryService, private totalPageArray: totalPageArray,    private permission: PermissionService,private spinner : NgxSpinnerService,

  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "typeName";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  getAllLeaveType() {
    this.leaveTypeService
      .getLeaves()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          this.allLeaveType = resp.httpResponse;


        },
        error: (err: any) => {
          this.toastr.error(err.message);
        },
      });
  }
  getLeaveCategory() {
    this.leaveCategoryService
      .getLeaveCategory()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // var allSLGGroup = resp;
          this.Leavetypes = resp.httpResponse;


        },
        error: (err: any) => {
          this.toastr.error(err.message);
        },
      });
  }

  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }

  saveLeaveType() {
    this.spinner.show();
    if (!this.leavetype.id) {
      this.leavetype.action = ActionEnum.Insert;
    } else {
      this.leavetype.action = ActionEnum.Update;
    }

    if (this.validateModalData()) {
      this.leaveTypeService
        .saveLeaveType(this.leavetype)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getLeaveType();
            this.closeModal();
            this.leavetype = {} as LeaveType;
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
    this.leaveTypeService
      .deleteLeaveType(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getLeaveType();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        },
      });
  }
  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.leaveTypeService
      .getLeavesById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.allLeaveType = resp;
        this.leavetype = this.allLeaveType.httpResponse;
        this.getLeaveType();

        if (this.leavetype) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.deleteLeaveType = currentRowData.typeName;
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  onCategorySelect(value: any) {
    this.leavetype.categoryName = value.target.value;
  }

  closeModal() {
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
      this.leavetype = {} as LeaveType;
    }
  }

  validateModalData(): boolean {
    if (!this.leavetype.categoryName) {
      this.toastr.error(this.translateService.instant( "i18n.configuration.leaveTypeDetails.Categoryrequired",(this.spinner.hide())));
      return false;
    }
    if (!this.leavetype.typeName) {
      this.toastr.error(this.translateService.instant( "i18n.configuration.leaveTypeDetails.LeaveTyperequired",(this.spinner.hide())));
      return false;
    }

    return true;
  }
  //****************** For Grid Filter ****************//
  getLeaveType() {


    this.leaveTypeService
      .getFilterLeaveType(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {

        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.leaveType;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }
  gridReady(params: any) {
    this.gridApi = params.api;
  }


  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['leaveCategory.name', 'typeName']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getLeaveType();

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

    this.getLeaveType();
  }
}
