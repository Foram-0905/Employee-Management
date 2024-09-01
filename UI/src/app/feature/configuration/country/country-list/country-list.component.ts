import { ToastrService } from 'ngx-toastr';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Country } from '../../../../shared/models/country';
import { CountryService } from '../../../../shared/services/country.service';
import { first } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { RequestWithFilterAndSort, filterModel, sortModel } from '../../../../shared/models/FilterRequset';
import { GridApi } from 'ag-grid-community';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';
@Component({
  selector: 'app-country-list',
  templateUrl: './country-list.component.html',
  styleUrls: ['./country-list.component.scss']
})
export class CountryListComponent implements OnInit {


  // ***********  Configure AG-Grid  ****************/

  dataRowSource!: any[];
  editId: string = '';
  showEdit: boolean = true;
  showDelete: boolean = true;
  allCountry: any;
  showDeleteModal: boolean = false;
  deleteCountry: any;
  country = {} as Country;
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  private gridApi!: GridApi;
  totalPages: number[] = [];
  totalItems: any;
  searchText: any;
  private helper = new commonHelper();
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addEditModal') addEditModal!: any;
  columnDefs:any []=[];
  lang!: string;
  // columnDefs = [
  // { headerName: 'Country', field: 'countryName', sortable: true, filter: true },//for demo of width
  // ];
  //***********  End of Configuration of AG-Grid  ****************/

  constructor(private http: HttpClient,
    private totalPageArray: totalPageArray,
    private translateService: TranslateService,
    private permission: PermissionService,
    private spinner:NgxSpinnerService,
    private countryService: CountryService,
    private toastr: ToastrService) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "countryName";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
  }
  ngOnInit() {
    this.lang = localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.configuration.manageCountryDetails.country'), field: 'countryName', sortable: false, filter: true },
      ];
    }, 50);
    this.getCountry();
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }

  getAllCountry() {
    this.countryService
      .getCountryList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          this.allCountry = resp;
          this.dataRowSource = this.allCountry.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error("Country : ", err.message);
        },
      });
  }

  getCountry() {

    this.countryService
      .getFilterCountry(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {
        
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.country;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }
  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.countryService
      .getCountryById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.allCountry = resp;
        this.country = this.allCountry.httpResponse;
        this.getAllCountry();
        this.getCountry();
        if (this.country) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }

  onDeleteClick(currentRowData: any) {
    // console.log('Delete clicked');
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.deleteCountry = currentRowData.countryName;
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  validateModalData(): boolean {
    if (!this.country.countryName) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageCountryDetails.Countrynamerequired",(this.spinner.hide())));
      return false;
    }
    return true;
  }

  closeModal() {
  
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
      this.country = {} as Country;
     

    }
  }

  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }

  saveCountry() {
    this.spinner.show();
    if (!this.country.id) {
      this.country.action = ActionEnum.Insert;
    } else {
      this.country.action = ActionEnum.Update;
    }

    if (this.validateModalData()) {
      this.countryService
        .saveCountry(this.country)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getAllCountry();
            this.getCountry();
            this.closeModal();
            this.country = {} as Country;
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
    this.countryService
      .deleteCountry(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          // this.getAllCountry();
          this.getCountry();
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  onSelect(value: any) {
    this.country.countryName = value.target.value;
  }

   //****************** For Grid Filter ****************//

   gridReady(params: any) {
    this.gridApi = params.api;
  }


  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['countryName']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getCountry();

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

    this.getCountry();
  }
}