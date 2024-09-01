import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { Contact } from '../../../../../../../shared/models/contact';
import { ContactService } from '../../../../../../../shared/services/contact.service';
import { SaveCity } from '../../../../../../../shared/models/city';
import { SaveCities } from '../../../../../../../shared/constant/api.const';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Country } from '../../../../../../../shared/models/country';
import { BankDetailsService } from '../../../../../../../shared/services/bankdetails.service';
import { State } from '../../../../../../../shared/models/state';
import { ContactAddressDetail } from '../../../../../../../shared/models/contact';
import { BankDetail } from '../../../../../../../shared/models/contact';
import { CountryService } from '../../../../../../../shared/services/country.service';
import { StateRegionService } from '../../../../../../../shared/services/state-region.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { City } from '../../../../../../../shared/models/city';
import { totalPageArray } from '../../../../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../../../../shared/helpers/common.helpers';
import { GridApi } from 'ag-grid-community';
import {
  ActionEnum,
  DefaultEmployee,
} from '../../../../../../../shared/constant/enum.const';
import { CityService } from '../../../../../../../shared/services/city.service';
import { EmployeeService } from '../../../../../../../shared/services/employee.service';
import {
  sortModel,
  RequestWithFilterAndSort,
  filterModel,
} from '../../../../../../../shared/models/FilterRequset';
import { ContactAddressService } from '../../../../../../../shared/services/contactaddress.service';
import { SaveContactAddress } from '../../../../../../../shared/models/contact-address';
import {
  BankDetails,
  SaveBankDetails,
} from '../../../../../../../shared/models/bank-details';
import { WorkLocationService } from '../../../../../../../shared/services/work-location.service';
import {
  WorkLocation,
  SaveWorkLocation,
} from '../../../../../../../shared/models/work-location';
import { v4 as uuidv4 } from 'uuid';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFlagService } from '../../../../../../../shared/services/EmployeFlag.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.scss',
})
export class ContactComponent implements OnInit {
  pageSize!: number;
  pageNumber!: number;
  ContactAddressDetail = {} as ContactAddressDetail;
  BankDetail = {} as BankDetail;
  Contact = {} as Contact;
  cities = {} as SaveCity;
  dataRowSource!: any[];
  searchText: any;
  selectedStateId: string = '';
  selectedCityId: string = '';
  // selectedCityId1: string = '';
  selectedCountryId: string = '';
  filteredStates: State[] = [];
  filteredCity: City[] = [];
  statename: State[] = [];
  cityname: City[] = [];
  city: any;
  country: any;
  countries!: any[];
  contactdata: any;
  states: State[] = [];
  // filteredCities: any[] = [];
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
  SelectedEmploy!: string;
  employeeName: any;
  flag: boolean = false;
  Editflag: boolean = true;
  currentId!: string;

  constructor(
    private http: HttpClient,
    private translateService: TranslateService,
    private BankDetailsService: BankDetailsService,
    private cityservice: CityService,
    private Contactservice: ContactService,
    private Countryservice: CountryService,
    private Stateservice: StateRegionService,
    private EmployeService: EmployeeService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private EmployeFlage: EmployeeFlagService,
    private totalPageArray: totalPageArray
  ) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
  ngOnInit() {
    // this.getCity();
    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
      this.ContactAddressDetail = {} as ContactAddressDetail;
      this.BankDetail = {} as BankDetail;
      this.Contact = {} as Contact;
        this.SelectedEmploy = employee;
        this.getContactByEmployeeId();
        this.getEmployeebyID()
    });

    
    this.getCountry();
    // this.getStatesByCountry();
    // this.getStatename();

  
  }
  // getCity(): void {
  //   this.cityservice.getCity()
  //     .pipe(first())
  //     .subscribe({
  //       next: (resp: any) => {
  //         this.cityname = resp.httpResponse;
  //       },
  //       error: (err: any) => {
  //         this.toastr.error(err);
  //       },
  //     });
  // }
  getCountry(): void {
    this.Countryservice.getCountryList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.country = resp.httpResponse;
          // Initialize selected country if already filled
          if (this.Contact.workCountryId) {
            this.selectedCountryId = this.Contact.workCountryId;
            this.onSelect( this.selectedCountryId );
            // this.loadStates(); // Call loadStates after selecting country
          }
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  getEmployeebyID(): void {
    this.EmployeService.getEmployeeById(this.SelectedEmploy)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.employeeName = resp;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  updateCity(cityId: string) {

    this.cityservice.getCityById(cityId).subscribe(
      (response) => {

        this.selectedCityId = cityId;
      },
      (error) => {
        console.error('Error updating city', error);
      }
    );
  }
  getStatesByCountry(): void {
    this.Stateservice.GetAllStates()
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
  getContact(): void {
    this.Contactservice.getContacts()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.contactdata = resp;
        },
        error: (err: any) => {
          this.toastr.error('Contacts', err.message);
        },
      });
  }
  onAddClick() {
    this.router.navigate([`/SaveEducation`]);
  }
  getBankdetails(): void {
    this.BankDetailsService.GetAllBankDetails()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          // var allSLGGroup = resp;
          this.getBankdetails = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  // onStateSelect(event: any) {
  //   const selectedStateId = event.target.value;
  //   this.filteredCity = this.cityname.filter((x: any) => x.state == selectedStateId);

  // }

  onStateSelect(selectedStateId: any) {
    this.selectedStateId = selectedStateId;
    this.cityservice
      .GetCityByState(this.selectedStateId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.filteredCity = resp.httpResponse;
          if (this.ContactAddressDetail.contactCity) {
            this.selectedCityId = this.ContactAddressDetail.contactCity;
          }
        },
        error: (err: any) => {
          this.toastr.error(err.message || 'Error fetching cities');
        },
      });
  }

  selectedStateId1: any;
  filteredCity1: any;

  onStateSelect1(selectedStateId1: any) {
    this.selectedStateId1 = selectedStateId1;
    this.cityservice
      .GetCityByState(this.selectedStateId1)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.filteredCity1 = resp.httpResponse;
          if (this.Contact.workCity) {
            this.selectedCityId1 = this.Contact.workCity;
          }
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  selectedCityId1: any;

  onCitySelect(value: any) {
    // this.selectedCityId = value.target.value;
    // this.selectedCityId1 = value.target.value;
    // console.log("select cities:",this.selectedCityId);
  }

  onSelect(selectedCountryId: any) {
    this.selectedCountryId = selectedCountryId;
    this.Stateservice.GetStateByCountryId(this.selectedCountryId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.filteredStates = resp.httpResponse;
          if (this.ContactAddressDetail.contactStateId) {
            this.selectedStateId = this.ContactAddressDetail.contactStateId;
          }
        },
        error: (err: any) => {
          this.toastr.error(err.message || 'Error fetching states');
        },
      });
  }

  filteredStates1: any;
  selectedCountryId1: any;
  onSelect1(selectedCountryId1: any) {
    this.selectedCountryId1 = selectedCountryId1;

    this.Stateservice.GetStateByCountryId(this.selectedCountryId1)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.filteredStates1 = resp.httpResponse;
          if (this.Contact.workStateId) {
            this.selectedStateId1 = this.Contact.workStateId;
            // this.loadStates();
          }
          // console.log("state", this.filteredStates)
        },

        error: (err: any) => {
          this.toastr.error(err.message || 'Error fetching states');
        },
      });
  }

  SaveContact() {
    this.spinner.show();
    if (!this.Contact.id) {
      this.Contact.action = ActionEnum.Insert;
      this.Contact.employeeId = this.SelectedEmploy;
    } else {
      this.Contact.action = ActionEnum.Update;
    }

    if (!this.Contact.contactAdressDetails) {
      this.Contact.contactAdressDetails = [];
    }

    let newAddressDetail: ContactAddressDetail;

    if (this.ContactAddressDetail.id) {
      newAddressDetail = {
        id: this.ContactAddressDetail.id,
        number: this.ContactAddressDetail.number,
        street: this.ContactAddressDetail.street,
        contactStateId: this.ContactAddressDetail.contactStateId,
        contactCountryId: this.ContactAddressDetail.contactCountryId,
        contactZipCode: this.ContactAddressDetail.contactZipCode,
        contactCity: this.ContactAddressDetail.contactCity,
        contactPhone1: this.ContactAddressDetail.contactPhone1,
        contactPhone2: this.ContactAddressDetail.contactPhone2,
        contactEmailbeON: this.ContactAddressDetail.contactEmailbeON,
        contactEmailPrivate: this.ContactAddressDetail.contactEmailPrivate,
        contactEntitlement: this.ContactAddressDetail.contactEntitlement,
        action: ActionEnum.Update,
      };
    } else {
      // New address detail (insert)
      newAddressDetail = {
        id: uuidv4(),
        number: this.ContactAddressDetail.number,
        street: this.ContactAddressDetail.street,
        contactStateId: this.ContactAddressDetail.contactStateId,
        contactCountryId: this.ContactAddressDetail.contactCountryId,
        contactZipCode: this.ContactAddressDetail.contactZipCode,
        contactCity: this.ContactAddressDetail.contactCity,
        contactPhone1: this.ContactAddressDetail.contactPhone1,
        contactPhone2: this.ContactAddressDetail.contactPhone2,
        contactEmailbeON: this.ContactAddressDetail.contactEmailbeON,
        contactEmailPrivate: this.ContactAddressDetail.contactEmailPrivate,
        contactEntitlement: this.ContactAddressDetail.contactEntitlement,
        action: ActionEnum.Insert,
      };
    }

    // Replace the old address detail with the new one
    this.Contact.contactAdressDetails = [newAddressDetail];

    if (!this.Contact.bankDetails) {
      this.Contact.bankDetails = [];
    }

    let newBankDetail: BankDetail;

    if (this.BankDetail.id) {
      // Existing bank detail (update)
      newBankDetail = {
        id: this.BankDetail.id,
        bankAccountNumber: this.BankDetail.bankAccountNumber,
        bankIFSCCode: this.BankDetail.bankIFSCCode,
        bankName: this.BankDetail.bankName,
        bankAccountHolder: this.BankDetail.bankAccountHolder,
        action: ActionEnum.Update,
      };
    } else {
      // New bank detail (insert)
      newBankDetail = {
        id: uuidv4(),
        bankAccountNumber: this.BankDetail.bankAccountNumber,
        bankIFSCCode: this.BankDetail.bankIFSCCode,
        bankName: this.BankDetail.bankName,
        bankAccountHolder: this.BankDetail.bankAccountHolder,
        action: ActionEnum.Insert,
      };
    }
    // Replace the old bank detail with the new one
    this.Contact.bankDetails = [newBankDetail];

    if (this.ValidateModalData()) {

      this.Contactservice.saveContact(this.Contact).subscribe(
        (resp: any) => {
          this.toastr.success(resp.message);
          this.resetForm();
          this.getContactByEmployeeId();
          this.spinner.hide();
        },
        (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        }
      );
    }
  }
  getContactByEmployeeId(): void {
    this.Contactservice.GetContactByEmployeeId(this.SelectedEmploy)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          if (resp && resp.httpResponse) {
            this.Contact = resp.httpResponse;

            if (this.Contact.workCountryId) {
              this.selectedCountryId1 = this.Contact.workCountryId;
              this.onSelect1(this.selectedCountryId1);
            }

            if(this.Contact.workStateId){
              this.selectedStateId1=this.Contact.workStateId;
              this.onStateSelect1(this.selectedStateId1);
            }

            if (resp.httpResponse.contactAdressDetails?.length) {
              this.ContactAddressDetail = resp.httpResponse.contactAdressDetails[0];
              if (this.ContactAddressDetail.contactCountryId) {
                this.selectedCountryId =
                  this.ContactAddressDetail.contactCountryId;
                this.onSelect(this.selectedCountryId);
              }
              if (this.ContactAddressDetail.contactStateId) {
                this.selectedStateId = this.ContactAddressDetail.contactStateId;
                this.onStateSelect(this.selectedStateId);
              }
            }

            if (resp.httpResponse.bankDetails?.length) {
              this.BankDetail = resp.httpResponse.bankDetails[0];
            }
          }
        },
        error: (err: any) => {
          this.toastr.error(err.message);
        },
      });
  }
  autoFillAccountHolder(event: any) {
    if (event.target.checked) {
      this.BankDetail.bankAccountHolder = this.employeeName.httpResponse.fullName;
    } else {
      this.BankDetail.bankAccountHolder = '';
    }
  }
  resetForm() {
    this.Contact = {} as Contact;
    this.ContactAddressDetail = {} as ContactAddressDetail;
    this.BankDetail = {} as BankDetail;
  }

  ValidateModalData(): boolean {
    if (!this.Contact.workZipCode) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.ZipCodenamerequired'
        )
      );
      return false;
    }
    if (!this.Contact.workCity) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.Citynamerequired'
        )
      );
      return false;
    }
    if (!this.Contact.workStateId) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.FederalStatenamerequired'
        )
      );
      return false;
    }
    if (!this.Contact.workCountryId) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.Countrynamerequired'
        )
      );
      return false;
    }
    if (!this.ContactAddressDetail.number) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.HouseNumberrequired'
        )
      );
      return false;
    }

    if (!this.ContactAddressDetail.street) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.Streetnamerequired'
        )
      );
      return false;
    }
    if (!this.ContactAddressDetail.contactZipCode) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.ZipCodenamerequired'
        )
      );
      return false;
    }
    if (!this.ContactAddressDetail.contactCity) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.Citynamerequired'
        )
      );
      return false;
    }
    if (!this.ContactAddressDetail.contactEmailbeON) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.EmailbeONrequired'
        )
      );
      return false;
    }
    if (!this.ContactAddressDetail.contactEmailPrivate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.EmailPrivaterequired'
        )
      );
      return false;
    }
    if (!this.BankDetail.bankAccountHolder) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.AccountHolderrequired'
        )
      );
      return false;
    }
    if (!this.BankDetail.bankAccountNumber) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.AccountNumberrequired'
        )
      );
      return false;
    }
    if (!this.BankDetail.bankIFSCCode) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.IFSCCoderequired'
        )
      );
      return false;
    }
    if (!this.BankDetail.bankName) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.contact-Details.BankNamerequired'
        )
      );
      return false;
    }
    return true;
  }
}
