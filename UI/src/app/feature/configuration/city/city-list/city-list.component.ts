import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { City } from '../../../../shared/models/city';
import { RequestWithFilterAndSort, filterModel, sortModel } from '../../../../shared/models/FilterRequset';
import { Router } from '@angular/router';
import { SaveCity } from '../../../../shared/models/city';
import { first } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { CityService } from '../../../../shared/services/city.service';
import { TranslateService } from '@ngx-translate/core';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { GridApi } from 'ag-grid-community';
import { catchError, throwError } from 'rxjs';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { CountryService } from '../../../../shared/services/country.service';
import { StateRegionService } from '../../../../shared/services/state-region.service';
import { State } from '../../../../shared/models/state';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
var $: any;

@Component({
  selector: 'app-city-list',
  templateUrl: './city-list.component.html',
  styleUrl: './city-list.component.scss'
})
export class CityListComponent implements OnInit {
  //***********  Configure AG-Grid  ****************/
  pageSize !: number;
  pageNumber!: number;
  dataRowSource!: any[];
  searchText: any;
  selectedCountryId: string = '';
  filteredStates: State[] = [];
  statename: State[] = [];
  cityname: any;
  filter: any;
  // searchText: any;
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  private gridApi!: GridApi;
  countryName: any;
  sortModel = {} as sortModel;
  editId: string = '';
  cities = {} as SaveCity;
  getCities = {} as City;
  getSelectedStates = {} as City;
  country: any;
  countries!: any[];
  states: State[] = [];
  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  totalPages: number[] = [];
  totalItems: any;
  private helper = new commonHelper();
  showDeleteModal: boolean = false;
  DeleteCity: any;

  columnDefs: any[] = []
  // columnDefs = [

  //   { headerName: 'Country', field: 'country.countryName', sortable: false, filter: true , filterParams: { maxNumConditions: 1 }},
  //   { headerName: 'State', field: 'stateId.name', sortable: false, filter: true ,filterParams: { maxNumConditions: 1 }},
  //   { headerName: 'City', field: 'name', sortable: false, filter: true, filterParams: { maxNumConditions: 1 } }

  // ];
  showEdit: boolean = true;
  showDelete: boolean = true;
  selectedStateId: string = '';
  // statename: any;
  countryId: string = '';
  // showDeleteModal: boolean = false;
  // DeleteCity: any;
  // totalPages: number[] = [];
  // totalItems: any;

  //***********  End of Configuration of AG-Grid  ****************/

  constructor(private http: HttpClient,
    private translateService: TranslateService,
    private permission: PermissionService,
    private countryService: CountryService,
    private stateregionService: StateRegionService,
    private toastr: ToastrService,
    private spinner:NgxSpinnerService,
    private router: Router,
    private totalPageArray: totalPageArray,
    private cityService: CityService) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "name";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }
  ngOnInit() {
    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.configuration.manageCityDetails.country'), field: 'country.countryName', sortable: false, filter: true, filterParams: { maxNumConditions: 1 } },
        { headerName: this.translateEnumValue('i18n.configuration.manageCityDetails.statename'), field: 'stateId.name', sortable: false, filter: true, filterParams: { maxNumConditions: 1 } },
        { headerName: this.translateEnumValue('i18n.configuration.manageCityDetails.cityname'), field: 'name', sortable: false, filter: true, filterParams: { maxNumConditions: 1 } }
      ];
    }, 50);
    this.getCity();
    this.getStatename();
    this.getCountry();

  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }

  getCity() {
    this.cityService
      .GetFilterCity(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.city;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }

  getCityData() {
    this.cityService.getCity().subscribe({
      next: (data: any) => {
        this.dataRowSource = data.httpResponse;
        this.getCities = data.httpResponse;
      },
      error: (error: any) => {
      }
    });
  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  SaveCity() {
    this.spinner.show();
    // console.log('add city' , this.cities);
    if (!this.cities.id) {
      this.cities.action = ActionEnum.Insert;
    } else {
      this.cities.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.cityService
        .addCity(this.cities)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            // this.getCityData();
            this.getCity();
            this.closeModal();
            this.cities = {} as SaveCity;
            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error(err);
            this.spinner.hide();
          },
        });
    }
  }

  getCountry(): void {
    this.countryService.getCountryList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.country = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  getStatename(): void {
    this.stateregionService.GetAllStates()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.statename = resp.httpResponse;
          // this.filteredStates = this.statename;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  onDeleteClick(currentRowData: any) {
    // console.log('Delete clicked');
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteCity = currentRowData.name;
  }

  deleteItem() {
    this.cityService
      .deleteCityById(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getCityData();
          this.getCity();
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  cancelDelete() {
    this.showDeleteModal = false;
  }

  onPageNumberChange(pageNumber: number) {
    this.pageNumber = pageNumber;
  }

  onSelect(value: any) {
    // this.cities.countryId = value.target.value;
    this.selectedCountryId = value.target.value;
    this.filteredStates = this.statename.filter(state => state.countryId === this.selectedCountryId)
  }



  filterStates(): void {
    if (this.selectedStateId) {
      this.filteredStates = this.statename.filter(state => state.id === this.selectedStateId);
    } else if (this.selectedCountryId) {
      this.filteredStates = this.statename.filter(state => state.countryId === this.selectedCountryId);
    }
  }

  onStateSelect(value: any) {
    this.selectedStateId = value.target.value;
  }
  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.cityService
      .getCityById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        // console.log(resp);
        this.cityname = resp;
        this.cities = this.cityname.httpResponse;
        // this.getCityData();
        this.getCity();
        this.cities = this.cityname.httpResponse;
        this.selectedCountryId = this.cities.countryId;
        this.selectedStateId = this.cities.state;
        this.filteredStates = this.statename.filter(state => state.countryId === this.selectedCountryId);

        if (this.cities) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
            this.selectedCountryId = this.cities.countryId;
            this.filterStates();
            this.selectedStateId = this.cityname.httpResponse.state;

          }
        }
      });
  }
  onAddClick() {
    this.router.navigate([`/addCity`]);
  }
  ValidateModalData(): boolean {

    if (!this.cities.countryId) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageCityDetails.Countrynamerequired"));
      return false;
    }
    if (!this.cities.state) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageCityDetails.Statenamerequired"));
      return false;
    }
    if (!this.cities.name) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageCityDetails.Citynamerequired"));
      return false;
    }
    return true;
  }

  closeModal() {
    this.cities = {} as SaveCity;
    if (this.closeButton) {
      this.closeButton.nativeElement.click();
    }
  }
  //****************** For Grid Filter ****************//

  gridReady(params: any) {
    this.gridApi = params.api;
  }

  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['country.countryName', 'stateId.name', 'name']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getCity();

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
    this.getCity();
  }
}

