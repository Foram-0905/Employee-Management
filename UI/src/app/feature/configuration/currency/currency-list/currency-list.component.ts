import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, first } from 'rxjs';
import { CurrencyService } from '../../../../shared/services/currency.service';
import { CountryService } from '../../../../shared/services/country.service';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { Currency, SaveCurrency } from '../../../../shared/models/currency';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { RequestWithFilterAndSort, filterModel, sortModel } from '../../../../shared/models/FilterRequset';
import { GridApi } from 'ag-grid-community';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-currency-list',
  templateUrl: './currency-list.component.html',
  styleUrl: './currency-list.component.scss'
})
export class CurrencyListComponent implements OnInit {


  //***********  Configure AG-Grid  ****************/
  PageSize !: number;
  PageNumber!: number;
  dataRowSource!: any[];
  columnDefs:any[]=[]
  // columnDefs = [

  //   // { headerName: 'Id', field: 'id',sortable: false, filter: true },
  //   { headerName: 'Country', field: 'countryId.countryName', sortable: false, filter: true },
  //   { headerName: 'Short Word', field: 'shortWord', sortable: false, filter: true },
  //   { headerName: 'Symbol', field: 'symbol', sortable: false, filter: true },

  // ];
  country: any;
  currency = {} as SaveCurrency;
  selectedCountryId: string = '';
  currencydata:any;
  shortWord: string = '';
  symbol: string = '';
  ManageCurrency: string = "";
  lang: string = "";
  showEdit: boolean = true;
  showDelete: boolean = true;
  @ViewChild('addEditModal') addEditModal!: any;
  // @ViewChild('AddEditModalToggle') modal!: ElementRef;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  showDeleteModal: boolean = false;
  DeleteCurrency: any;
  totalItems: any;
  editId: any;
  currencyResponse: any;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  totalPages: number[] = [];
  private gridApi!: GridApi;
  private helper = new commonHelper();
  searchText: any;

  //***********  End of Configuration of AG-Grid  ****************/

  constructor(private http: HttpClient,
    private translateService: TranslateService,
    private router: Router,
    private toastr: ToastrService,
    private permission: PermissionService,
    private currencyService: CurrencyService,
    private countryService: CountryService,
    private totalPageArray: totalPageArray,
    private spinner:NgxSpinnerService,
  )
   {this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "shortWord";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {}; }
  ngOnInit() {
    setTimeout(() => {
      this.columnDefs = [
        { headerName:  this.translateEnumValue('i18n.configuration.manageCurrencyDetails.country'), field: 'countryId.countryName', sortable: false, filter: true },
        { headerName:  this.translateEnumValue('i18n.configuration.manageCurrencyDetails.currencyshortword'), field: 'shortWord', sortable: false, filter: true },
        { headerName:  this.translateEnumValue('i18n.configuration.manageCurrencyDetails.currencysymbol'), field: 'symbol', sortable: false, filter: true },      ];
    }, 50);
    this.getFilterCurrency();
    this.getCountry();
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  getCurrency(): void {
    this.currencyService
      .getCurrencies()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.currencydata = resp;
        },
        error: (err: any) => {
          this.toastr.error("Currency", err.message);
        },
      });
  }
  getFilterCurrency() {

    this.currencyService
      .GetFilterCurrency(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {
        
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.currencies
        ;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }
  getCountry() {
    this.countryService.getCountryList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          // var allSLGGroup = resp;
          this.country = resp.httpResponse;
        },
        error: (err: any) => {
           this.toastr.error(err.message);
        },
      });
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  ValidateModalData(): boolean {
    if (!this.currency.country) {
      this.toastr.error('Country required');
      return false;
    }
    if (!this.currency.shortWord) {
      this.toastr.error('Short Word required');
      return false;
    }
    if (!this.currency.symbol) {
      this.toastr.error('Symbol required');
      return false;
    }
    return true;
  }

  addCurrency(): void {
    this.spinner.show();
    if (!this.currency.id) {
      this.currency.action = ActionEnum.Insert;
    } else {
      this.currency.action = ActionEnum.Update;
    }
    // const newCurrency: Omit<Currency, 'id'> = {
    //   country: this.selectedCountryId,
    //   shortWord: this.shortWord,
    //   symbol: this.symbol,
    //   action:ActionEnum.Insert
    // };
    if (this.ValidateModalData()) {
      this.currencyService.addCurrency(this.currency)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            // this.getCountry();
            this.getFilterCurrency();
            // this.getCurrency();
            this.closeModal();
            this.currency = {} as SaveCurrency;
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
    this.currencyService
      .deleteCurrency(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getCountry();
          this.getFilterCurrency();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        },
      });
  }
  closeModal() {
  
    this.currency = {} as SaveCurrency;
   
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
    }
  }

  onSelect(value: any) {
    this.currency.country = value.target.value;
  }

  onAddClick() {
    this.router.navigate([`/addCurrency`]);
  }

  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.currencyService
      .getCurrencyById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.currencyResponse = resp;
        this.currency = this.currencyResponse.httpResponse;
        this.getCountry();
        this.getFilterCurrency();
        /// // debugger
        if (this.currency) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteCurrency = currentRowData.shortWord;
  }
  onPageSizeChange(pageSize: number) {
    // Handle page size change
    this.PageSize = pageSize;
  }

  onPageNumberChange(pageNumber: number) {
    // Handle page number change
    this.PageNumber = pageNumber;
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

    this.getFilterCurrency();
  }

  gridReady(params: any) {
    this.gridApi = params.api;
  }
  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['countryId.countryName', 'shortWord', 'symbol']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getFilterCurrency();

  }

  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length==0 ||inputValue.length >= 4) {
      this.commonSearchWithinGrid();
    }
  }

}