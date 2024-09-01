import { ToastrService } from 'ngx-toastr';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SlgGroupService } from '../../../../shared/services/slg-group.service';
import { SLGGroup } from '../../../../shared/models/slg-group';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { first } from 'rxjs';
import { Router } from '@angular/router';
import { RequestWithFilterAndSort, sortModel } from '../../../../shared/models/FilterRequset';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-slg-list',
  templateUrl: './slg-list.component.html',
  styleUrl: './slg-list.component.scss'
})
export class SlgListComponent implements OnInit {

  //***********  Configure AG-Grid  ****************/
  PageSize !: number;
  PageNumber!: number;
  slgGroups: SLGGroup = {} as SLGGroup;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  showDeleteModal: boolean = false;
  dataRowSource!: any[];
  totalPages: number[] = [];
  totalItems: any;
  private helper = new commonHelper();
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addBtn') addBtn!: ElementRef;
  columnDefs:any[]=[]
  // columnDefs = [
  //   // { headerName: 'Id', field: 'id',sortable: false, filter: true },
  //   { headerName: 'Initial SLG Status', field: 'initialStatus', sortable: false, filter: true },
  //   { headerName: 'Status Name', field: 'statusName', sortable: false, filter: true },
  //   { headerName: 'Status Sequence', field: 'statusSequence', sortable: false, filter: true },
  //   { headerName: 'Relevent Experience', field: 'relevantExperience', sortable: false, filter: true },
  // ];
  showEdit: boolean = true;
  showDelete: boolean = true;
  currentId: any;
  allSLGGroup: any;
  DeleteSLGGroup: any;
  gridApi: any;
  searchText: any;



  constructor(private translateService: TranslateService,
    private slgGroupService: SlgGroupService,
    private router: Router,
    private toastr: ToastrService,
    private totalPageArray: totalPageArray,
    private permission: PermissionService,
    private spinner: NgxSpinnerService) {

    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "initialStatus";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }
  lang: string = '';

  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.configuration.slgGroupDetails.initialslgstatus'), field: 'initialStatus', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.configuration.slgGroupDetails.slgstatusdisplayname'), field: 'statusName', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.configuration.slgGroupDetails.displaysequence'), field: 'statusSequence', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.configuration.slgGroupDetails.relevantworkexperience'), field: 'relevantExperience', sortable: false, filter: true },
      ];
    }, 50);
    this.getFilterslggroup();
   
  }

  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  getAllSlggroup(): void {
    this.slgGroupService.GetAllSlggroup() .pipe(first())
    .subscribe({
      next: (resp: any) => {
        this.allSLGGroup = resp.httpResponse;
        // console.log(data.message);
      },
      error: (error: any) => {
        this.toastr.error(error.message)
      }
    });
  }
  hasPermission(permission: any) {

    return this.permission.hasPermission(permission);
  }
  getFilterslggroup(){
    this.slgGroupService
    .getFilterslggroup(
      this.requestWithFilterAndPage
    )
    .pipe(first())
    .subscribe((resp)=>{
      // console.warn(resp,"slgfilter");

      this.totalItems = resp.httpResponse.totalRecord;
      this.dataRowSource = resp.httpResponse.slggroups;
      this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);

    })
  }

  SaveSLGgroup() {
    this.spinner.show();
    if (!this.slgGroups.id) {
      this.slgGroups.action = ActionEnum.Insert;
    } else {
      this.slgGroups.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.slgGroupService.SaveSlggroup(this.slgGroups).pipe(first()).subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.getFilterslggroup();
          this.slgGroups = {} as SLGGroup;
          this.closeModal();
          this.spinner.hide();

        },
        error: (err: any) => {
          this.spinner.hide();
          this.toastr.error(err.message)
        }

      });
    }

  }
  closeModal() {
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
      this.slgGroups = {} as SLGGroup;
    }
  }
  ValidateModalData(): boolean {
    if (!this.slgGroups.initialStatus) {
      this.toastr.error(this.translateService.instant("i18n.configuration.slgGroupDetails.InitialSLGstatusrequired",(this.spinner.hide())));
      return false;
    }
    if (!this.slgGroups.statusName ) {
      this.toastr.error(this.translateService.instant("i18n.configuration.slgGroupDetails.SLGstatusdisplaynamerequired",(this.spinner.hide())));
      return false;
    }
    if ( !this.slgGroups.statusSequence) {
      this.toastr.error(this.translateService.instant("i18n.configuration.slgGroupDetails.Displaysequencerequired",(this.spinner.hide())));
      return false;
    }
    if (!this.slgGroups.relevantExperience ) {
      this.toastr.error(this.translateService.instant("i18n.configuration.slgGroupDetails.Relevantworkexperiencerequired",(this.spinner.hide())));
      return false;
    }
    return true
  }



  onEditClick(currentRowData: any): void {
    this.currentId = currentRowData.id;
    this.slgGroupService
      .getSLGGroupById(this.currentId)
      .pipe(first())
      .subscribe((resp) => {
        this.allSLGGroup = resp;
        this.slgGroups = this.allSLGGroup.httpResponse;
        this.getFilterslggroup();
        /// // debugger
        if (this.slgGroups) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }

  onDeleteClick(currentRowData: any) {
    this.currentId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteSLGGroup = currentRowData.initialStatus;
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  deleteItem() {
    this.spinner.show();
    this.slgGroupService
      .deleteSLGGroup(this.currentId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getAllSlggroup();
          this.getFilterslggroup();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err.message);
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
    const gridColumnsToFileter: string[] = ['initialStatus', 'statusName', 'statusSequence','relevantExperience']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getFilterslggroup();

  }

  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length==0 ||inputValue.length >= 4) {
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

    this.getFilterslggroup();
  }


}
