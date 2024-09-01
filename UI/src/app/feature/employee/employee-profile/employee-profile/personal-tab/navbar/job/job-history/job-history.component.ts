import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { EmployeeType } from '../../../../../../../../shared/models/employeeType';
import { EmployeeTypeService } from '../../../../../../../../shared/services/employeType.service';
import { TranslateService } from '@ngx-translate/core';
import { JobHistory } from '../../../../../../../../shared/models/job-history';
import { Observable, first } from 'rxjs';
import { JobHistoryService } from '../../../../../../../../shared/services/job-history.service';
import { EmployeeService } from '../../../../../../../../shared/services/employee.service';
import { CountryService } from '../../../../../../../../shared/services/country.service';
import { EducationLevelService } from '../../../../../../../../shared/services/education-level.service';
import { StateRegionService } from '../../../../../../../../shared/services/state-region.service';
import { CityService } from '../../../../../../../../shared/services/city.service';
import {
  ActionEnum,
  DefaultEmployee,
} from '../../../../../../../../shared/constant/enum.const';
import { employee } from '../../../../../../../../shared/models/employee';
import { Country } from '../../../../../../../../shared/models/country';
import { State } from '../../../../../../../../shared/models/state';
import {
  sortModel,
  RequestWithFilterAndSort,
  filterModel,
} from '../../../../../../../../shared/models/FilterRequset';
import { GridApi } from 'ag-grid-community';
import { totalPageArray } from '../../../../../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../../../../../shared/helpers/common.helpers';
import { City } from '../../../../../../../../shared/models/city';
import { DocumentList } from '../../../../../../../../shared/models/documentlist';
import { DocumentListService } from '../../../../../../../../shared/services/documentlist.service';
import { PermissionService } from '../../../../../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFlagService } from '../../../../../../../../shared/services/EmployeFlag.service';
var $: any;

@Component({
  selector: 'app-job-history',
  templateUrl: './job-history.component.html',
  styleUrl: './job-history.component.scss',
})
export class JobHistoryComponent implements OnInit {
  PageSize!: number;
  searchText: any;
  DeleteJobHistory: any;
  jobHistory = {} as JobHistory;
  documentlist = {} as DocumentList;
  PageNumber!: number;
  dataRowSource!: any[];
  totalPages: number[] = [];
  totalItems: any;
  employees: employee[] = [];
  countries: Country[] = [];
  selectedCountryId: string = '';
  // DeleteJobHistory: any;
  selectedStateId: string = '';
  states: State[] = [];
  cities: City[] = [];
  employeeType: EmployeeType[] = [];
  filteredStates: State[] = [];
  filteredCity: City[] = [];
  sortModel = {} as sortModel;
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  private helper = new commonHelper();
  private gridApi!: GridApi;
  jobhistorydata: any[] = [];
  cityname: any;
  country: any;
  editId: string = '';
  statename: any;
  employmentType: any;
  selectedFileName: string = '';
  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  Deletejobhistory: any;

  columnDefs: any[] = [];
  // columnDefs = [
  //   { headerName: 'CompanyName', field: 'companyName', sortable: false, filter: true },
  //   { headerName: 'PostionHeld', field: 'positionHeld', sortable: false, filter: true },
  //   { headerName: 'EmployeeType', field: 'employmentTypeName', sortable: false, filter: true },
  //   { headerName: 'ZipCode', field: 'zipCode', sortable: false, filter: true },
  //   { headerName: 'Country', field: 'countryName', sortable: false, filter: true },
  //   { headerName: 'State', field: 'stateName', sortable: false, filter: true },
  //   { headerName: 'City', field: 'cityName', sortable: false, filter: true },
  //   { headerName: 'StartDate', field: 'startDate', sortable: false, filter: true },
  //   { headerName: 'End Date', field: 'endDate', sortable: false, filter: true },
  //   { headerName: 'Reason For Leaving', field: 'leavingReason', sortable: false, filter: true },

  // ];
  showEdit: boolean = true;
  showDelete: boolean = true;
  showDownload: boolean = true;
  showDeleteModal: boolean = false;
  jobHistoryResponse: any;
  documentListData = {} as DocumentList;
  downloadId: any;
  SelectedEmployee: any;
  constructor(
    private http: HttpClient,

    private router: Router,
    private toastr: ToastrService,
    private jobHistoryService: JobHistoryService,
    private employeeService: EmployeeService,
    private countryService: CountryService,
    private employeeTypeService: EmployeeTypeService,
    private stateService: StateRegionService,
    private cityService: CityService,
    private permission: PermissionService,
    private educationlevelService: EducationLevelService,
    private documentlistService: DocumentListService,
    private translateService: TranslateService,
    private totalPageArray: totalPageArray,
    private spinner: NgxSpinnerService,
    private employeeservies: EmployeeService,
    private EmployeFlage: EmployeeFlagService
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = 'companyName';
    this.sortModel.sortOrder = 'asc';
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  ngOnInit(): void {
    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
      this.dataRowSource = [];
      this.SelectedEmployee = employee;
          });
    setTimeout(() => {
      this.columnDefs = [
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.companyname'
          ),
          field: 'companyName',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.postionheld'
          ),
          field: 'positionHeld',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.typeofemployeement'
          ),
          field: 'employeeType.typeofemployment',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.zipCode'
          ),
          field: 'zipCode',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.country'
          ),
          field: 'countryId.countryName',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.state'
          ),
          field: 'state.name',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.city'
          ),
          field: 'cityId.name',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.startdate'
          ),
          field: 'startDate',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.enddate'
          ),
          field: 'endDate',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.job-History-Details.message'
          ),
          field: 'leavingReason',
          sortable: false,
          filter: true,
        },
      ];
    }, 50);
    this.spinner.show();
    this.getFilterjobHistory()
    this.getStatename();
    this.getCountry();
    this.getEmployeeType();
    this.GetTypeofEmployment();
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);
  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  // getJobHistory(): void {
  //   this.jobHistoryService
  //     .GetJobHistoryByEmployeeId(this.SelectedEmployee)
  //     .pipe(first())
  //     .subscribe({
  //       next: (resp: any) => {
  //         this.dataRowSource = resp.httpResponse;
  //         console.log("GetJob",this.dataRowSource);
  //       },
  //       error: (err: any) => {
  //         this.toastr.error('JobHistory', err.message);
  //       },
  //     });
  // }
  onSelect(selectedCountryId: any) {
    this.selectedCountryId = selectedCountryId;
    console.warn('selectcountryid', this.selectedCountryId);

    this.stateService
      .GetStateByCountryId(this.selectedCountryId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.filteredStates = resp.httpResponse;
          // console.log('state ayagaya', this.filteredStates);
          // Initialize selected state if already filled
          if (this.jobHistory.countryName) {
            this.selectedStateId = this.jobHistory.stateId;
          }
        },
        error: (err: any) => {
          this.toastr.error(err.message || 'Error fetching states');
        },
      });
  }

  getFilterjobHistory():void {
    this.jobHistoryService
      .GetJobHistoryByEmployeeId(this.SelectedEmployee)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.documentlist = resp;
          this.dataRowSource = resp.httpResponse
          this.spinner.hide(); 
        },
        error: (err: any) => {
          this.toastr.error("job", err.message);
          this.spinner.hide(); 
        },
      });
  }
  getCountry() {
    this.countryService
      .getCountryList()
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
    this.stateService
      .GetAllStates()
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
  getEmployeeType(): void {
    this.employeeTypeService
      .GetEmployeeType()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.employmentType = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  TypeofEmployment: any;
  GetTypeofEmployment() {
    this.employeeService
      .GetTypeofEmployment()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.TypeofEmployment = resp.httpResponse.map((type: any) => {
            const employmentType = type as EmployeeType;
            employmentType.translationKey =
              'i18n.employeeProfile.personal-details.job-History-Details.typeofemployeementdetails.' +
              type.typeofemployment.toLowerCase();
            return employmentType;
          });
        },
        error: (err: any) => {
          console.error('Error fetching employment types:', err);
        },
      });
  }

  SaveJobHistory(): void {
    this.spinner.show();
    if (!this.jobHistory.id) {
      this.jobHistory.action = ActionEnum.Insert;
      this.jobHistory.employeeId = this.SelectedEmployee;
      this.jobHistory.employmentTypeName = '';
      this.jobHistory.cityName = '';
      this.jobHistory.stateName = '';
      this.jobHistory.countryName = '';
    }
     else {
      this.jobHistory.action = ActionEnum.Update;
      this.jobHistory.employmentTypeName = '';
      this.jobHistory.cityName = '';
      this.jobHistory.stateName = '';
      this.jobHistory.countryName = '';
     }
    

    if (this.ValidateModalData()) {
      this.jobHistory.filename = this.selectedFileName;
      this.jobHistoryService
        .SaveJobHistory(this.jobHistory)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            // this.getJobHistory();
            this.getFilterjobHistory();
            this.jobHistory = {} as JobHistory;

            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error(err);
            this.spinner.hide();
          },
        });
      // this.spinner.hide();
    }
  }

 

  onStateSelect(selectedStateId: any) {
    this.selectedStateId = selectedStateId;
    // console.log("selected state:", this.selectedStateId);

    this.cityService
      .GetCityByState(this.selectedStateId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.filteredCity = resp.httpResponse;
          console.log("Filtered Cities:", this.filteredCity);
          // Ensure selected city if already filled
          if (this.jobHistory.city) {
            this.selectedCityId = this.jobHistory.city;
          }
        },
        error: (err: any) => {
          this.toastr.error(err.message || 'Error fetching cities');
        },
      });
  }

  selectedCityId: string = '';
  onCitySelect(value: any) {
    this.selectedCityId = value.target.value;
    // console.log("select cities:",this.selectedCityId);
  }
  onCountrySelect(value: any) {
    this.selectedCountryId = value.target.value;
    this.filteredStates = this.statename.filter(
      (state: any) => state.countryId === this.selectedCountryId
    );
  }
  onEmployeeTypeSelect(value: any) {
    const selectElement = value.target as HTMLSelectElement;
    this.jobHistory.employmentType = value.target.value;
  }

  onAddClick() {
    this.router.navigate([`/SaveJobHistory`]);
  }
  deleteItem() {
    this.jobHistoryService
      .DeleteJobHistory(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          // this.getJobHistory();
          this.getFilterjobHistory();
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  cancelDelete() {
    this.showDeleteModal = false;
  }


  onDownloadClick(currentRowData: any): void {
    this.downloadId = currentRowData.id;

    this.jobHistoryService
      .GetJobHistoryByid(this.downloadId)
      .pipe(first())
      .subscribe((resp) => {
        const FileData = resp.httpResponse.document;
        const indexOfPlus = FileData.indexOf('+');

        if (indexOfPlus !== -1) {
          const fileFormat = FileData.slice(0, indexOfPlus); 
          const base64String = FileData.slice(indexOfPlus + 1);

          const blob = this.base64toBlob(
            base64String,
            `application/${fileFormat}`
          );
          const blobUrl = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = blobUrl;
          link.download = `document.${fileFormat}`; 

          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
          window.URL.revokeObjectURL(blobUrl);
        } else {
          console.error('No "file extention" found in FileData');
        }
      });
  }

  base64toBlob(base64Data: string, contentType: string): Blob {
    const sliceSize = 512;
    const byteCharacters = atob(base64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);
      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    return new Blob(byteArrays, { type: contentType });
  }

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.Deletejobhistory = currentRowData.JobHistory;
  }


  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1];
        const fileFormat: string = this.getFileExtension(file.name);
        const Concatenatefile = `${fileFormat}+${base64String}`;
        this.jobHistory.document = Concatenatefile;
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedFileName = '';
    }
  }
  getFileExtension(filename: string): string {
    return filename.split('.').pop()?.toLowerCase() || ''; 
  }
  filteredCity1: any;

  getCurrentDate(): string {
    const currentDate = new Date();
    return currentDate.toISOString().substring(0, 10); 
  }


  ValidateModalData(): boolean {
    if (!this.jobHistory.companyName) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.CompanyNamerequired", (this.spinner.hide())));
      return false;
      
    }
    if (!this.jobHistory.positionHeld) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.PositionHeldrequired", (this.spinner.hide())));
      return false;
    }
    if (!this.jobHistory.zipCode) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.zipCoderequired", (this.spinner.hide())));
      return false;
    }
    if (!this.jobHistory.employmentType) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.EmploymentTyperequired", (this.spinner.hide())));
      return false;
    }
    if (!this.jobHistory.stateId) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.Staterequired", (this.spinner.hide())));
      return false;
    }
    if (!this.jobHistory.city) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.Cityrequired", (this.spinner.hide())));
      return false;
    }
    if (!this.jobHistory.country) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.Countryrequired", (this.spinner.hide())));
      return false;
    }
    if (!this.jobHistory.document) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.Documentrequired",this.spinner.hide()));
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.Documentrequired", (this.spinner.hide())));
      return false;
    }
    if (!this.jobHistory.leavingReason) {
      this.spinner.hide();
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.Reasonrequired",this.spinner.hide()));
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.job-History-Details.Reasonrequired", (this.spinner.hide())));
      return false;
      
    }
    
    return true;
  }
  onEditClick(currentRowData: any): void {
    console.log("edit click");
    this.editId = currentRowData.id;
    this.jobHistoryService
      .GetJobHistoryByid(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.jobHistoryResponse = resp;
        this.jobHistory = this.jobHistoryResponse.httpResponse;
        // console.log("Fetched Education Data:", this.education);

        // Ensure state and city are logged
        this.onSelect(this.jobHistory.country);
        this.onStateSelect(this.jobHistory.stateId);

        // Ensure selected state and city are assigned
        this.selectedCountryId = this.jobHistory.country;
        this.selectedStateId = this.jobHistory.stateName;
        this.selectedCityId = this.jobHistory.city;

        console.log("Selected State ID:", this.selectedStateId);
        console.log("Selected City ID:", this.selectedCityId);

        this.getFilterjobHistory();
        // this.getJobHistory();

        if (this.jobHistory) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }
  closeModal() {
    // console.log('Close modal');
    this.jobHistory = {} as JobHistory;
    // console.log('City data', this.cities);

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
    const gridColumnsToFileter: string[] = [
      'companyName',
      'positionHeld',
      'employeeType.employeeTypeName',
      'zipCode',
      'country.countryName',
      'stateId.name',
      'cityId.name',
      'startDate',
      'endDate',
      'leavingReason',
      'document',
    ];
    this.helper.commonSearchWithinGrid(
      gridColumnsToFileter,
      this.searchText,
      this.requestWithFilterAndPage
    );
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    // this.getFilterjobHistory();
  }

  getDataRowsourse(event: RequestWithFilterAndSort) {
    if (!event.sortModel) {
      event.sortModel = this.sortModel;
    }
    if (!event.filterModel) {
      event.filterModel = this.requestWithFilterAndPage.filterModel;
      event.filterConditionAndOr =
        this.requestWithFilterAndPage.filterConditionAndOr;
    } else {
      this.searchText = '';
    }
    this.requestWithFilterAndPage = event;
    // this.getFilterjobHistory();
  }
}
