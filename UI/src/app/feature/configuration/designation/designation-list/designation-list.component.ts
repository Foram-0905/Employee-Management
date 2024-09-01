import { GridApi } from 'ag-grid-community';
import { GetFilterDesignation } from './../../../../shared/constant/api.const';
import { sortModel } from './../../../../shared/models/FilterRequset';
import { ChangeDetectionStrategy, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DesignationService } from '../../../../shared/services/designation.service';
import { Observable, first } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { RequestWithFilterAndSort, filterModel } from '../../../../shared/models/FilterRequset';
import { Router } from '@angular/router';
import {ManageDesignation,Savedesignation} from '../../../../shared/models/manage-designation';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { SlgGroupService } from '../../../../shared/services/slg-group.service';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-designation-list',
  templateUrl: './designation-list.component.html',
  styleUrl: './designation-list.component.scss',
})
export class DesignationListComponent implements OnInit {
  allDesignation: any;
  pageSize!: number;
  pageNumber!: number;
  filter: any;
  dataRowSource!: any[];
  searchText: any;
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  editId: string = '';
  Designation = {} as Savedesignation;
  private gridApi!: GridApi;
  columnDefs:any[]=[];

  // columnDefs = [
  //   {
  //     headerName: 'Initial Status',
  //     field: 'slgGroup.initialStatus',
  //     filter: true,
  //     filterParams: { maxNumConditions: 1 },
  //   },
  //   {
  //     headerName: 'Designation',
  //     field: 'designation',
  //     filter: true,
  //     filterParams: { maxNumConditions: 1 },
  //   },
  //   {
  //     headerName: 'Display Sequence',
  //     field: 'displaySequence',
  //     filter: true,
  //     filterParams: { maxNumConditions: 1 },
  //   },
  //   {
  //     headerName: 'Short Word',
  //     field: 'shortWord',
  //     filter: true,
  //     filterParams: { maxNumConditions: 1 },
  //   },
  // ];
  showEdit: boolean = true;
  showDelete: boolean = true;
  slgGroup: any;

  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  showDeleteModal: boolean = false;
  DeleteDesignation: any;
  totalPages: number[] = [];
  totalItems: any;
  private helper = new commonHelper();
  lang!: string;


  constructor(
    private designtionService: DesignationService,
    private SLGService: SlgGroupService,
    private permission: PermissionService,
    private translateService: TranslateService,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private spinner : NgxSpinnerService,
    private totalPageArray: totalPageArray

  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "Designation";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        {
              headerName: this.translateEnumValue('i18n.configuration.designationDetails.InitialSLGStatus'),
              field: 'slgGroup.initialStatus',
              filter: true,
              filterParams: { maxNumConditions: 1 },
            },
            {
              headerName: this.translateEnumValue('i18n.configuration.designationDetails.designation'),
              field: 'designation',
              filter: true,
              filterParams: { maxNumConditions: 1 },
            },
            {
              headerName: this.translateEnumValue('i18n.configuration.designationDetails.DisplaySequence'),
              field: 'displaySequence',
              filter: true,
              filterParams: { maxNumConditions: 1 },
            },
            {
              headerName: this.translateEnumValue('i18n.configuration.designationDetails.DesignationShortWord'),
              field: 'shortWord',
              filter: true,
              filterParams: { maxNumConditions: 1 },
            },
      ];
    }, 50);
    this.getDesignation();
    this.getSLGGroup();

  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  getDesignation() {
    this.designtionService
      .GetFilterDesignation(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.designation;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }

  getSLGGroup() {
    this.SLGService
      .GetAllSlggroup()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.slgGroup = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error("Designation SlgGroup", err.message);
        },
      });
  }


  getAllDesignation() {
    this.designtionService
      .GetAllDesignation()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          this.allDesignation = resp;
          // this.dataRowSource = this.allDesignation.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error("Designation", err.message);
        },
      });
  }

  onAddClick() {
    this.Designation = {} as Savedesignation;
    this.router.navigate([`/addDesignation`]);
  }

  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.designtionService
      .getDesignationById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.allDesignation = resp;
        this.Designation = this.allDesignation.httpResponse;
        this.getAllDesignation();
        this.getDesignation();
        /// // debugger
        if (this.Designation) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteDesignation = currentRowData.designation;
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  ValidateModalData(): boolean {
    if (!this.Designation.initialStatus) {
      this.toastr.error(this.translateService.instant("i18n.configuration.designationDetails.InitialSLGstatusrequired",(this.spinner.hide())));
      return false;
    }
    if (!this.Designation.designation) {
      this.toastr.error(this.translateService.instant("i18n.configuration.designationDetails.Designationnamerequired",(this.spinner.hide())));
      return false;
    }
    if (!this.Designation.displaySequence) {
      this.toastr.error(this.translateService.instant("i18n.configuration.designationDetails.Displaysequencerequired",(this.spinner.hide())));
      return false;
    }
    if (!this.Designation.shortWord) {
      this.toastr.error(this.translateService.instant("i18n.configuration.designationDetails.ShortWordrequired",(this.spinner.hide())));
      return false;
    }
    return true;
  }

  closeModal() {
    this.Designation = {} as Savedesignation;
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
    }
  }

  SaveDesignation() {
    this.spinner.show();
    if (!this.Designation.id) {
      this.Designation.action = ActionEnum.Insert;
    } else {
      this.Designation.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.designtionService
        .saveDesignation(this.Designation)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getDesignation();
            this.getAllDesignation();
            this.closeModal();
            this.Designation = {} as Savedesignation;
            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error("Save Designation", err.message);
            this.spinner.hide();
          },
        });
    }
  }

  deleteItem() {
    this.designtionService
      .deleteDesignation(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getDesignation();
          this.getAllDesignation();
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  onSelect(value: any) {
    this.Designation.initialStatus = value.target.value;
  }

  //****************** For Grid Filter ****************//

  gridReady(params: any) {
    this.gridApi = params.api;
  }


  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['designation', 'shortWord', 'displaySequence','slgGroup.initialStatus']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getDesignation();

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

    this.getDesignation();
  }
}
