import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { first } from 'rxjs';
import { Router } from '@angular/router';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { ToastrService } from 'ngx-toastr';
import { EducationLevelService } from '../../../../shared/services/education-level.service';
import { EducationLevel } from '../../../../shared/models/education-level';
import { RequestWithFilterAndSort, sortModel } from '../../../../shared/models/FilterRequset';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { Title } from '@angular/platform-browser';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-education-level-list',
  templateUrl: './education-level-list.component.html',
  styleUrl: './education-level-list.component.scss'
})
export class EducationLevelListComponent {
  //***********  Configure AG-Grid  ****************/
  PageSize !: number;
  PageNumber!: number;
  EducationLevel: EducationLevel = {} as EducationLevel;
  showDeleteModal: boolean = false;
  dataRowSource!: any[];
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addBtn') addBtn!: ElementRef;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  totalPages: number[] = [];
  totalItems: any;
  gridApi: any;
  searchText: any;
  private helper = new commonHelper();

  columnDefs: any[] = [];
  // columnDefs = [
  //     { headerName: this.translateService.instant('i18n.configuration.educationLevelDetails.educationlevel'), field: 'level', sortable: false, filter: true },
  //   ];
  showEdit: boolean = true;
  showDelete: boolean = true;
  currentId: any;
  allEducationLevel: any;
  DeleteEducationLevel: any;
  editId: any;
  // lang: string = '';
  lang!: string;

  constructor(private EducationService: EducationLevelService,
    private toastr: ToastrService,
    private translateService: TranslateService,
    private router: Router,
    private totalPageArray: totalPageArray,
    private permission: PermissionService,
    private spinner: NgxSpinnerService
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "level";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
    this.showEdit = true;
    this.showDelete = true;
  }
  ngOnInit() {
    this.lang = localStorage.getItem('lang') || 'en';
    // console.log("Language(Education)", this.lang);
    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.configuration.educationLevelDetails.educationlevel'), field: 'level', sortable: false, filter: true },
      ];
    }, 50);

    this.getFilterEducationLevel();

  }

  hasPermission(permission: any) {
    debugger
    return this.permission.hasPermission(permission);
  }

  // headerTranslation(translateKey: string) {
  //   return () => this.translateService.instant(translateKey);
  // }
  // refreshGridHeaders(): void {
  //   if (this.gridApi) {
  //     this.gridApi.setColumnDefs(this.columnDefs);
  //   }
  // }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  getAllEducationLevel() {
    this.EducationService
      .getAllEducationLevel()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.allEducationLevel = resp.httpResponse;

        },
        error: (err: any) => {
          this.toastr.error(err);
        },

      })
  }

  getFilterEducationLevel() {
    this.EducationService
      .getFilterEducationLevel(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {
        //  console.warn(resp,"filter");

        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.eductionlevel;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      })
  }

  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.EducationService
      .getEducationLevelById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.allEducationLevel = resp;
        this.EducationLevel = this.allEducationLevel.httpResponse;
        this.getFilterEducationLevel();
        /// // debugger
        if (this.EducationLevel) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }
  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteEducationLevel = currentRowData.level;
  }
  cancelDelete() {
    this.showDeleteModal = false;
  }

  ValidateModalData(): boolean {
    if (!this.EducationLevel.level) {
      this.toastr.error(this.translateService.instant("i18n.configuration.educationLevelDetails.educationlevel",(this.spinner.hide())));
      return false;
    }

    return true;
  }

  closeModal() {
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
      this.EducationLevel = {} as EducationLevel;
    }
  }

  SaveEducationLevel() {
    this.spinner.show();
    if (!this.EducationLevel.id) {
      this.EducationLevel.action = ActionEnum.Insert;
    } else {
      this.EducationLevel.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.EducationService
        .saveEducationLevel(this.EducationLevel)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getFilterEducationLevel();
            this.closeModal();
            this.spinner.hide();
            this.EducationLevel = {} as EducationLevel;
          },
          error: (err: any) => {
            this.toastr.error(err.message);
            this.spinner.hide();
          },
        });
    }
  }

  deleteItem() {
    this.spinner.show();
    this.EducationService
      .deleteEducationLevel(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getAllEducationLevel();
          this.getFilterEducationLevel();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        },
      });
  }
  gridReady(params: any) {
    this.gridApi = params.api;
  }

  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['level']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getFilterEducationLevel();

  }
  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length == 0 || inputValue.length >= 4) {
      this.commonSearchWithinGrid();
    }
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

    this.getFilterEducationLevel();
  }

}
