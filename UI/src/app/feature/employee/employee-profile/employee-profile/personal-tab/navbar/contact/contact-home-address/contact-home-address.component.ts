import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { first } from 'rxjs';
import { SaveCity } from '../../../../../../../../shared/models/city';
import { SaveCities } from '../../../../../../../../shared/constant/api.const';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Country } from '../../../../../../../../shared/models/country';
import { State } from '../../../../../../../../shared/models/state';
import { CountryService } from '../../../../../../../../shared/services/country.service';
import { StateRegionService } from '../../../../../../../../shared/services/state-region.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { City } from '../../../../../../../../shared/models/city';
import { totalPageArray } from '../../../../../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../../../../../shared/helpers/common.helpers';
import { GridApi } from 'ag-grid-community';
import { ActionEnum } from '../../../../../../../../shared/constant/enum.const';
import { CityService } from '../../../../../../../../shared/services/city.service';
import { EmployeeService } from '../../../../../../../../shared/services/employee.service';
import { sortModel, RequestWithFilterAndSort, filterModel } from '../../../../../../../../shared/models/FilterRequset';
import { ContactAddressService } from '../../../../../../../../shared/services/contactaddress.service';
import { ContactAddress, SaveContactAddress } from '../../../../../../../../shared/models/contact-address';
@Component({
  selector: 'app-contact-home-address',
  templateUrl: './contact-home-address.component.html',
  styleUrl: './contact-home-address.component.scss'
})
export class ContactHomeAddressComponent implements OnInit {
  pageSize !: number;
  pageNumber!: number;
  cities = {} as SaveCity;
  dataRowSource!: any[];
  contactaddress = {} as SaveContactAddress;
  searchText: any;
  selectedStateId: string = '';
  selectedCityId: string = '';
  selectedCountryId: string = '';
  filteredStates: State[] = [];
  filteredCity: City[] = [];
  statename: State[] = [];
  cityname: City[] = [];
  city: any;
  country: any;
  countries!: any[];
  states: State[] = [];
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  private gridApi!: GridApi;
  sortModel = {} as sortModel;
  // contactAddress = {} as ContactAddress;
  editId: string = '';
  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  totalItems: any;
  private helper = new commonHelper();
  showDeleteModal: boolean = false;
  DeleteCity: any;
  showEdit: boolean = true;
  showDelete: boolean = true;
  constructor(private http: HttpClient,
    private translateService: TranslateService,
    private cityservice: CityService,
    private Countryservice: CountryService,
    private Stateservice: StateRegionService,
    private ContactAddressService: ContactAddressService,
    private EmployyeService: EmployeeService,
    private toastr: ToastrService,
    private router: Router,
    private totalPageArray: totalPageArray,) {
    // this.requestWithFilterAndPage.pageNumber = 1;
    // this.requestWithFilterAndPage.pageSize = 5;
    // this.sortModel.colId = "name";
    // this.sortModel.sortOrder = "asc";
    // this.requestWithFilterAndPage.sortModel = this.sortModel;
    // this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }
  ngOnInit() {
    this.getCity();
    this.getCountry();
    this.getStatename();
  }
  getCity(): void {
    this.cityservice.getCity()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          // var allSLGGroup = resp;
          this.cityname = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  getCountry(): void {
    this.Countryservice.getCountryList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.country = resp.httpResponse;
          console.log("country", this.country);

        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  getStatename(): void {
    this.Stateservice.GetAllStates()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.statename = resp.httpResponse;
          console.log("stae", this.statename);

          // this.filteredStates = this.statename;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  
  SaveContactAddress() {
    if (!this.contactaddress.id) {
      this.contactaddress.action = ActionEnum.Insert;
    } else {
      this.contactaddress.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.ContactAddressService
        .SaveContactAddress(this.contactaddress)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getCity();
            this.closeModal();
            this.contactaddress = {} as SaveContactAddress;
          },
          error: (err: any) => {
            this.toastr.error(err);
          },
        });
    }
  }
  onStateSelect(event: any) {
    const selectedStateId = event.target.value;
    console.log('Selected State ID:', selectedStateId);

    console.log("before filterCity", this.cityname);
    this.filteredCity = this.cityname.filter((x: any) => x.state == selectedStateId);

    // Manually filter cities based on the selected state ID
    // this.filteredCity = [];
    // for (const city of this.cityname) {
    //     if (city.stateId === selectedStateId) {
    //         this.filteredCity.push(city);
    //     }
    // }
    console.log('Filtered Cities:', this.filteredCity);
  }
  onCitySelect(value: any) {
    this.selectedCityId = value.target.value;
  }
  onSelect(value: any) {
    this.selectedCountryId = value.target.value;
    this.filteredStates = this.statename.filter(state => state.countryId === this.selectedCountryId)
  }
  ValidateModalData(): boolean {
    if (!this.contactaddress.street) {
      this.toastr.error('street name  required');
      return false;
    }
    if (!this.contactaddress.zipCode) {
      this.toastr.error('zipCode name required');
      return false;
    }
    if (!this.contactaddress.city) {
      this.toastr.error('City name required');
      return false;
    }
    if (!this.contactaddress.emailbeON) {
      this.toastr.error('emailbeON name required');
      return false;
    }
    if (!this.contactaddress.emailPrivate) {
      this.toastr.error('emailPrivate name required');
      return false;
    }
    if (!this.contactaddress.entitlement) {
      this.toastr.error('entitlement name required');
      return false;
    }
    return true;
  }
  closeModal() {
    console.log('Close modal');
    this.contactaddress = {} as SaveContactAddress;
    console.log('contactaddress data', this.contactaddress);

    if (this.closeButton) {

      this.closeButton.nativeElement.click();
    }
  }
}


