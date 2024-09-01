import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { StateRegionService } from '../../../../shared/services/state-region.service';
import { State } from '../../../../shared/models/state';
import { ActionEnum } from '../../../../shared/constant/enum.const';
// import { RequestWithFilterAndSort } from '../../../../shared/models/FilterRequset';
import { first } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { CountryService } from '../../../../shared/services/country.service';
import { Router } from '@angular/router';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { RequestWithFilterAndSort, filterModel, sortModel } from '../../../../shared/models/FilterRequset';
import { GridApi } from 'ag-grid-community';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-state-region-list',
  templateUrl:'./state-region-list.component.html',
  styleUrl: './state-region-list.component.scss',
})
export class StateRegionListComponent implements OnInit {
  editId: string = '';
  AllState: any;
  lang: string = '';
  //***********  Configure AG-Grid  ****************/
  // requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  // pageSize!: number;
  // pageNumber!: number;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addEditModal') addEditModal!: any;
  PageSize!: number;
  showEdit: boolean = true;
  showDelete: boolean = true;
  Countrydata: any;
  PageNumber!: number;
  dataRowSource!: any[];
  State_region: State = {} as State;
  showDeleteModal: boolean = false;
  DeleteState:any;
  private helper = new commonHelper();
  totalPages: number[] = [];
  totalItems: any;
  private gridApi!: GridApi;
  
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  searchText: any;

  columnDefs:any[]=[]
  // columnDefs = [


  //   {
  //     headerName: 'Country',
  //     field: 'country.countryName',
  //     sortable: false,
  //     filterParams: { maxNumConditions: 1 },
  //   },
  //   {
  //     headerName: 'State/Region Name',
  //     field: 'name',
  //     sortable: false,
  //     filterParams: { maxNumConditions: 1 },
  //   },

  // ];
 




  constructor(
    private translateService: TranslateService,
    private stateRegionService: StateRegionService,
    private toastr: ToastrService,
    private router: Router,
    private countryservice:CountryService,
    private totalPageArray: totalPageArray,
    private permission: PermissionService,
    private spinner: NgxSpinnerService
  ) {
    // this.Country = [{ Id: "ce7c792d-93fe-45b7-9a84-910d31c366fd", value: "India" }];
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
    this.getcountry();
    this.getfilterstate();
    setTimeout(() => {
      this.columnDefs = [
        {
          headerName: this.translateEnumValue('i18n.configuration.stateRegionDetails.country'),
          field: 'country.countryName',
          sortable: false,
          filterParams: { maxNumConditions: 1 },
        },
        {
          headerName: this.translateEnumValue('i18n.configuration.stateRegionDetails.state/regionname'),
          field: 'name',
          sortable: false,
          filterParams: { maxNumConditions: 1 },
        },
          ];
    }, 50);
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }

  getcountry(): void {
    this.countryservice
    .getCountryList()
    .pipe(first())
    .subscribe({
      next: (resp: any) => {
        // this.dataRowSource = resp.httpResponse;
        this.Countrydata = resp.httpResponse;

       }
       ,error: (err: any) => {
        this.toastr.error(err.message);
      },
     
    });
  }


  getfilterstate(){
    this.stateRegionService
    .getFilterState
    (this.requestWithFilterAndPage)
    .pipe(first())
    .subscribe((resp) => {
      this.totalItems = resp.httpResponse.totalRecord;
      this.dataRowSource = resp.httpResponse.state;
      this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
    });
  }

  hasPermission(permission: any) {
    debugger
    return this.permission.hasPermission(permission);
  }
  getAllstate(){
    this.stateRegionService
    .GetAllStates()
    .pipe(first())
    .subscribe({
      next: (resp: any) => {
        this.AllState=resp.httpResponse;;
        // this.dataRowSource = this.AllState
        // console.warn('state',this.dataRowSource);

      },
      error: (err: any) => {
        this.toastr.error(err.message);
      },
    });
  }

  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.stateRegionService
      .getStateById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.AllState = resp;
        this.State_region = this.AllState.httpResponse;
        this.getfilterstate();
        /// // debugger
        if (this.State_region) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }




  ValidateModalData(): boolean {
    if (!this.State_region.countryId) {
      this.toastr.error(this.translateService.instant("i18n.configuration.managePublicHolidayDetails.countryrequired",(this.spinner.hide())));
      return false;
    }
    if (!this.State_region.name) {
      this.toastr.error(this.translateService.instant("i18n.configuration.managePublicHolidayDetails.staterequired",(this.spinner.hide())));
      return false;
    }
    return true;
  }

  closeModal() {

    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
      this.State_region = {} as State;
    }
  }

  SaveState() {
    this.spinner.show();
    if (!this.State_region.id) {
      this.State_region.action = ActionEnum.Insert;
    } else {
      this.State_region.action = ActionEnum.Update;
    }
    if (this.ValidateModalData()){
      this.stateRegionService
      .SaveState_Region(this.State_region)
      .pipe(first())
      .subscribe({
        next:(resp:any)=>{
          this.toastr.success(resp.message);
          this.getfilterstate();
          this.State_region = {} as State;
          this.closeModal();
          this.spinner.hide();
        },
        error:(err:any) =>{
          this.toastr.error(err.message)
          this.spinner.hide();
        }
      })
    }
  }


  deleteItem(){
    this.spinner.show();
    this.stateRegionService
    .deleteState(this.editId)
    .pipe(first())
    .subscribe({
      next:(resp:any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getAllstate();
          this.getfilterstate();
          this.spinner.hide();
      },
      error:(err:any) => {
        this.toastr.error(err);
        this.spinner.hide();
      },
    });
  }

  onSelect(value: any) {
    this.State_region.countryId = value.target.value;
  }

  cancelDelete(){
    this.showDeleteModal = false;
  }
  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteState = currentRowData.name;
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
    this.getfilterstate();

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

    this.getfilterstate();
  }

}

