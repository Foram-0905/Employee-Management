import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { City } from '../../../../../../../../shared/models/city';
import { sortModel, RequestWithFilterAndSort, filterModel } from '../../../../../../../../shared/models/FilterRequset';
import { Router } from '@angular/router';
import { CityService } from '../../../../../../../../shared/services/city.service';
import { SaveCity } from '../../../../../../../../shared/models/city';
import { Country } from '../../../../../../../../shared/models/country';
import { first } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { totalPageArray } from '../../../../../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../../../../../shared/helpers/common.helpers';
import { GridApi } from 'ag-grid-community';
import { catchError, throwError } from 'rxjs';
import { ActionEnum } from '../../../../../../../../shared/constant/enum.const';
import { CountryService } from '../../../../../../../../shared/services/country.service';
import { StateRegionService } from '../../../../../../../../shared/services/state-region.service';
import { State } from '../../../../../../../../shared/models/state';
import { WorkLocationService } from '../../../../../../../../shared/services/work-location.service';
import { WorkLocation } from '../../../../../../../../shared/models/work-location';
import { SaveWorkLocation } from '../../../../../../../../shared/models/work-location';
@Component({
  selector: 'app-work-location',
  templateUrl: './work-location.component.html',
  styleUrl: './work-location.component.scss'
})
export class WorkLocationComponent implements OnInit {
  pageSize !: number;
  pageNumber!: number;
  WorkLocation = {} as WorkLocation;
  saveworklocation = {} as SaveWorkLocation;
  dataRowSource!: any[];
  searchText: any;
  selectedCountryId: string = '';
  filteredStates: State[] = [];
  filteredCity: City[] = [];
  statename: State[] = [];
  cityname: City[]=[];
  filter: any;
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  countryName: any;
  sortModel = {} as sortModel;
  editId: string = '';
  getSelectedStates = {} as City;
  country: any;
  countries!: any[];
  city: any;
  states: State[] = [];
  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  totalPages: number[] = [];
  totalItems: any;
  private helper = new commonHelper();
  showDeleteModal: boolean = false;
  DeleteCity: any;
  selectedStateId: string = '';
  selectedCityId: string = '';
  constructor(private http: HttpClient,
    private translateService: TranslateService,
    private cityservice: CityService,
    private Countryservice: CountryService,
    private Stateservice: StateRegionService,
    private worklocationService: WorkLocationService,
    // private EmployyeService : EmployeeService,
    private toastr: ToastrService,
    private router: Router,
    private totalPageArray: totalPageArray,) {
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
          console.log("country",this.country);
          
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
          console.log("stae",this.statename);
          
          // this.filteredStates = this.statename;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  SaveWorklocation() {
    if (!this.saveworklocation.id) {
      this.saveworklocation.action = ActionEnum.Insert;
    } else {
      this.saveworklocation.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.worklocationService
        .SaveWorkLocation(this.saveworklocation)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            // this.getCity();
            this.closeModal();
            this.saveworklocation = {} as SaveWorkLocation;
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

    console.log("before filterCity",this.cityname);
    this.filteredCity = this.cityname.filter((x:any)=>x.state == selectedStateId);

}
  onCitySelect(value: any) {
    this.selectedCityId= value.target.value;
  }
  onSelect(value: any) {
    this.selectedCountryId = value.target.value;
    this.filteredStates = this.statename.filter(state => state.countryId === this.selectedCountryId)
  }

  ValidateModalData(): boolean {

    if (!this.WorkLocation.zipCode) {
      this.toastr.error('ZipCode name  required');
      return false;
    }
    if (!this.WorkLocation.city) {
      this.toastr.error('City name required');
      return false;
    }
    if (!this.WorkLocation.federalState) {
      this.toastr.error('federalState name required');
      return false;
    }
    if (!this.WorkLocation.country) {
      this.toastr.error('country name required');
      return false;
    }
    if (!this.WorkLocation.employeeId) {
      this.toastr.error('employeeId name required');
      return false;
    }
    return true;
  }

  closeModal() {
    this.WorkLocation = {} as WorkLocation;
    if (this.closeButton) {

      this.closeButton.nativeElement.click();
    }
  }
}