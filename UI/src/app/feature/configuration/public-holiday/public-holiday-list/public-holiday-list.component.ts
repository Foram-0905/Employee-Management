import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PublicHolidayService } from '../../../../shared/services/publicHoliday.service';
import { first } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { PublicHoliday } from '../../../../shared/models/public-holiday';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { CountryService } from '../../../../shared/services/country.service';
import { StateRegionService } from '../../../../shared/services/state-region.service';
import { State } from '../../../../shared/models/state';
import { ColDef } from 'ag-grid-community';
// import { StateRegionService } from '../../../../shared/services/state-region.service';
import { GridApi } from 'ag-grid-community';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { RequestWithFilterAndSort, sortModel } from '../../../../shared/models/FilterRequset';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-public-holiday-list',
  templateUrl: './public-holiday-list.component.html',
  styleUrl: './public-holiday-list.component.scss'
})
export class PublicHolidayListComponent implements OnInit {

  dataRowSource!: any[];
  showEdit: boolean = true;
  showDelete: boolean = true;
  showDeleteModal: boolean = false;
  allPublicHoliday: any;
  editId: string = '';
  publicHoliday = {} as PublicHoliday;
  deletePublicHoliday: any;
  Country: any;
  selectedCountryId: string = '';

  State: any;
  filteredStates: State[] = [];
  searchText: any;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  totalPages: number[] = [];
  totalItems: any;
  selectedStateId: string = '';
  statename: State[]=[];


  private gridApi!: GridApi;
  private helper = new commonHelper();


  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addEditModal') addEditModal!: any;

  columnDefs: ColDef[]=[]
  lang!: string;
  // columnDefs: ColDef[] = [

  //   { headerName: 'Country', field: 'countryId.countryName', filterParams: { maxNumConditions: 1 } },
  //   { headerName: 'State', field: 'stateId.name', filterParams: { maxNumConditions: 1 } },
  //   { headerName: 'Holiday Name', field: 'holidayName', filterParams: { maxNumConditions: 1 } },
  //   { headerName: 'Holiday Date', field: 'holidayDate', filterParams: { maxNumConditions: 1 } },
  // ];

  constructor(private translateService: TranslateService, private toastr: ToastrService, private publicHolidayService: PublicHolidayService, private countryService: CountryService,
    private stateRegion: StateRegionService, private totalPageArray: totalPageArray,    private permission: PermissionService,private spinner:NgxSpinnerService
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "holidayName";
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
        { headerName: this.translateEnumValue('i18n.configuration.managePublicHolidayDetails.Country'), field: 'countryId.countryName', filterParams: { maxNumConditions: 1 } },
        { headerName:this.translateEnumValue('i18n.configuration.managePublicHolidayDetails.State/Region'), field: 'stateId.name', filterParams: { maxNumConditions: 1 } },
        { headerName: this.translateEnumValue('i18n.configuration.managePublicHolidayDetails.HolidayName'), field: 'holidayName', filterParams: { maxNumConditions: 1 } },
        { headerName: this.translateEnumValue('i18n.configuration.managePublicHolidayDetails.HolidayDate'), field: 'holidayDate', filterParams: { maxNumConditions: 1 } },
      ];
    }, 50);
    this.getPublicHoliday();
    this.getCountry();
    this.getState();
    // this.getAllPublicHoliday();
  }

  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }

  getAllPublicHoliday() {


    this.publicHolidayService
      .getHolidayList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          this.allPublicHoliday = resp.httpResponse;


        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  getCountry() {
    this.countryService
      .getCountryList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {

          this.Country = resp.httpResponse;
          // console.warn("fghbjn", this.Country);

        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  getState() {
    this.stateRegion
      .GetAllStates()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {

          this.statename = resp.httpResponse;
          // this.filteredStates = this.State;

          // console.warn("State", this.State);

        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  filterStates() :void{
    if(this.selectedStateId) {
      this.filteredStates = this.statename.filter(state => state.id === this.selectedStateId);
    } else if (this.selectedCountryId) {
      this.filteredStates = this.statename.filter(state => state.countryId === this.selectedCountryId);
    }
}
onStateSelect(value: any) {
  this.selectedStateId = value.target.value;
 }
 onSelect(selectedCountryId: any) {
  this.selectedCountryId = selectedCountryId;
  console.warn('selectcountryid', this.selectedCountryId);

  this.stateRegion
    .GetStateByCountryId(this.selectedCountryId)
    .pipe(first())
    .subscribe({
      next: (resp: any) => {
        this.filteredStates = resp.httpResponse;
        console.log('Filtered States:', this.filteredStates);
        // Ensure selected state if already filled
        if (this.publicHoliday.country) {
          this.selectedStateId = this.publicHoliday.state;
        }
      },
      error: (err: any) => {
        this.toastr.error(err.message || 'Error fetching states');
      },
    });
}


  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.publicHolidayService
      .getHolidayListById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.allPublicHoliday = resp;
        this.publicHoliday = this.allPublicHoliday.httpResponse;
        this.getPublicHoliday();
        /// // debugger
        if (this.publicHoliday) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }

  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }

  onDeleteClick(currentRowData: any) {
    // console.log('Delete clicked');
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.deletePublicHoliday = currentRowData.holidayName;
  }
  getStatesByCountry(): void {
    this.stateRegion.GetAllStates()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.statename = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  validateModalData(): boolean {
    if (!this.publicHoliday.country) {
      this.toastr.error(this.translateService.instant("i18n.configuration.managePublicHolidayDetails.countryrequired",(this.spinner.hide())));
      return false;
    }
    if (!this.publicHoliday.state) {
      this.toastr.error(this.translateService.instant("i18n.configuration.managePublicHolidayDetails.staterequired",(this.spinner.hide())));
      return false;
    }
    if (!this.publicHoliday.holidayName) {
      this.toastr.error(this.translateService.instant("i18n.configuration.managePublicHolidayDetails.HolidayNamerequired",(this.spinner.hide())));
      return false;
    }
    if (!this.publicHoliday.holidayDate) {
      this.toastr.error(this.translateService.instant("i18n.configuration.managePublicHolidayDetails.HolidayDaterequired",(this.spinner.hide())));
      return false;
    }
    return true;
  }

  closeModal() {

    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
      this.publicHoliday = {} as PublicHoliday;
    }
  }

  // onCountrySelect(value: any) {
  //   this.publicHoliday.country = value.target.value;


  // }

  // onStateSelect(value: any) {
  //   // this.publicHoliday.state = value.target.value;
  //     this.publicHoliday.state = value.target.value;

  // }

  savePublicHoliday() {
    this.spinner.show();
    if (!this.publicHoliday.id) {
      this.publicHoliday.action = ActionEnum.Insert;
    } else {
      this.publicHoliday.action = ActionEnum.Update;
    }

    if (this.validateModalData()) {
      // console.log("holiday",this.publicHoliday);

      this.publicHolidayService
        .savePublicHoliday(this.publicHoliday)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getPublicHoliday();
            this.closeModal();
            this.publicHoliday = {} as PublicHoliday;
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
    this.publicHolidayService
      .deletePublicHoliday(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getAllPublicHoliday();
          this.getPublicHoliday();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        },
      });
  }
  getPublicHoliday() {
    // console.log(this.requestWithFilterAndPage);

    // console.log("Filter requset", this.requestWithFilterAndPage);

    this.publicHolidayService
      .getFilterPublicHoliday(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {
        // console.warn("datafilter", resp);

        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.publicHoliday;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }
  gridReady(params: any) {
    this.gridApi = params.api;
  }


  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['countryId.countryName', 'stateId.name', 'holidayName']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getPublicHoliday();

  }
  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length == 0 || inputValue.length >= 4) {
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

    this.getPublicHoliday();
  }
}
