import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable, first, switchMap } from 'rxjs';
import { EducationService } from '../../../../../../../../shared/services/education.service';
import { EmployeeService } from '../../../../../../../../shared/services/employee.service';
import { CountryService } from '../../../../../../../../shared/services/country.service';
import { EducationLevelService } from '../../../../../../../../shared/services/education-level.service';
import { StateRegionService } from '../../../../../../../../shared/services/state-region.service';
import { CityService } from '../../../../../../../../shared/services/city.service';
import {
  ActionEnum,
  DefaultEmployee,
} from '../../../../../../../../shared/constant/enum.const';
import {
  Education,
  SaveEducation,
} from '../../../../../../../../shared/models/education';
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
import { EducationLevel } from '../../../../../../../../shared/models/education-level';
import { Renderer2 } from '@angular/core';
import { DocumentList } from '../../../../../../../../shared/models/documentlist';
import { DocumentListService } from '../../../../../../../../shared/services/documentlist.service';
import { PermissionService } from '../../../../../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFlagService } from '../../../../../../../../shared/services/EmployeFlag.service';
@Component({
  selector: 'app-education1',
  templateUrl: './education.component.html',
  styleUrl: './education.component.scss',
})
export class Education1Component implements OnInit {
  PageSize!: number;
  PageNumber!: number;
  dataRowSource!: any[];
  totalPages: number[] = [];
  documentlist = {} as DocumentList;
  totalItems: any;
  columnDefs: any[] = [];
  // columnDefs = [
  //   { headerName: 'Education Level', field: 'educationLevelName', sortable: false, filter: true },
  //   { headerName: 'Subject', field: 'subject', sortable: false, filter: true },
  //   { headerName: 'Name of Institution', field: 'institute', sortable: false, filter: true },
  //   // {headerName: 'Documents', field: 'id', sortable: false,filter: true},

  // ];
  employees: employee[] = [];
  selectedStateId: string = '';
  countries: Country[] = [];
  selectedCountryId: string = '';
  states: State[] = [];
  cities: City[] = [];
  educationlevels: EducationLevel[] = [];
  filteredStates: State[] = [];
  filteredCity: City[] = [];
  education = {} as SaveEducation;
  sortModel = {} as sortModel;
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  private helper = new commonHelper();
  private gridApi!: GridApi;
  editId: any;
  downloadId: any;
  showEdit: boolean = true;
  showDelete: boolean = true;
  showDownload: boolean = true;
  showCheckbox: boolean = true;
  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addDownloadModel') addDownloadModel!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  showDeleteModal: boolean = false;
  searchText: any;
  DeleteEducation: any;
  educationdata: any;
  educationResponse: any;
  cityname: any;
  country: any;
  statename: any;
  filename?: any;
  selectedFileName: string = '';
  educationlevel: any;
  selectedFileName2: string = '';
  SelectedEmployee: any;

  constructor(
    private router: Router,
    private toastr: ToastrService,
    private educationService: EducationService,
    private employeeService: EmployeeService,
    private countryService: CountryService,
    private stateService: StateRegionService,
    private cityService: CityService,
    private permission: PermissionService,
    private educationlevelService: EducationLevelService,
    private documentlistService: DocumentListService,
    private translateService: TranslateService,
    private totalPageArray: totalPageArray,
    private spinner: NgxSpinnerService,
    private renderer: Renderer2,
    private elementRef: ElementRef,
    private EmployeFlage: EmployeeFlagService
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = 'subject';
    this.sortModel.sortOrder = 'asc';
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
  }

  ngOnInit(): void {
    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
      this.dataRowSource = [];
      this.SelectedEmployee = employee;
      this.getFilterEducation();
    });
    setTimeout(() => {
      this.columnDefs = [
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.educationDetails.educationLevel'
          ),
          field: 'educationLevelName',
          sortable: false,
          filter: true,
          filterParams: { maxNumConditions: 1 },
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.educationDetails.subjectofstudy'
          ),
          field: 'subject',
          sortable: false,
          filter: true,
          filterParams: { maxNumConditions: 1 },
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.educationDetails.institute'
          ),
          field: 'institute',
          sortable: false,
          filter: true,
          filterParams: { maxNumConditions: 1 },
        },
      ];
    }, 50);
    this.getEducationLevel();
    // this.getCity();

    this.getStatename();
    this.getCountry();
    this.getFilterEducation();
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);
  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  getEducation(): void {
    this.educationService
      .getEducation()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.educationdata = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error('Education', err.message);
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

        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  getEducationLevel(): void {
    this.educationlevelService
      .getAllEducationLevel()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.educationlevel = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  getFilterEducation() {
    this.educationService
      .getEducationbyEmployee(this.SelectedEmployee)
      .pipe(first())
      .subscribe((resp) => {
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse;
        this.totalPages = this.totalPageArray.GetTotalPageArray(
          this.totalItems,
          this.requestWithFilterAndPage.pageSize
        );

      });
  }


  selectedCityId: string = '';
  onStateSelect(selectedStateId: any) {
    this.selectedStateId = selectedStateId;

    this.cityService
      .GetCityByState(this.selectedStateId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.filteredCity = resp.httpResponse;

          if (this.education.city) {
            this.selectedCityId = this.education.city;
          }
        },
        error: (err: any) => {
          this.toastr.error(err.message || 'Error fetching cities');
        },
      });
  }

  SaveEducation(): void {
    this.spinner.show();
    if (!this.education.id) {
      this.education.action = ActionEnum.Insert;
      this.education.employee = this.SelectedEmployee;
      this.education.educationLevelName = '';
      this.education.filename = this.selectedFileName;
    } else {
      this.education.action = ActionEnum.Update;
      this.education.educationLevelName = '';
      this.education.filename = this.selectedFileName;
    }


    if (this.ValidateModalData()) {
      this.educationService
        .saveEducation(this.education)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getFilterEducation();
            this.getEducation();
            this.education = {} as SaveEducation;
            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error(err);
            this.spinner.hide();
          },
        });
    }
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

        this.education.certificate = Concatenatefile;
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedFileName = '';
    }
  }
  getFileExtension(filename: string): string {
    return filename.split('.').pop()?.toLowerCase() || '';
  }
  onEditClick(currentRowData: any): void {

    this.editId = currentRowData.id;
    this.educationService
      .getEducationById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.educationResponse = resp;
        this.education = this.educationResponse.httpResponse;

        this.onSelect(this.education.country);
        this.onStateSelect(this.education.state);

        this.selectedCountryId = this.education.country;
        this.selectedStateId = this.education.state;
        this.selectedCityId = this.education.city;

        this.getFilterEducation();
        this.getEducation();

        if (this.education) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }


  onDownloadClick(currentRowData: any): void {
    this.spinner.show();
    this.downloadId = currentRowData.id;

    this.educationService
      .getEducationById(this.downloadId)
      .pipe(first())
      .subscribe((resp) => {
        const FileData = resp.httpResponse.certificate;
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
          this.spinner.hide();
        } else {
          console.error('No "file extention" found in FileData');
          this.spinner.hide();
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


  onSelect(selectedCountryId: any) {
    this.selectedCountryId = selectedCountryId;


    this.stateService
      .GetStateByCountryId(this.selectedCountryId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.filteredStates = resp.httpResponse;


          if (this.education.country) {
            this.selectedStateId = this.education.state;
          }
        },
        error: (err: any) => {
          this.toastr.error(err.message || 'Error fetching states');
        },
      });
  }

  onCountrySelect(value: any) {
    this.selectedCountryId = value.target.value;
    this.filteredStates = this.statename.filter(
      (state: any) => state.countryId === this.selectedCountryId
    );
  }
  onEducationSelect(value: any) {
    this.education.educationLevels = value.target.value;
  }

  onAddClick() {
    this.router.navigate([`/SaveEducation`]);
  }
  cancelDelete() {
    this.showDeleteModal = false;
  }



  onFileSelected2(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFileName2 = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1];
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile = `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.education.anabin = Concatenatefile;
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedFileName2 = '';
    }
  }



  ValidateModalData(): boolean {
    if (!this.education.educationLevels) {
      this.toastr.error('Education Level required');
      return false;
    }
    if (!this.education.subject) {
      this.toastr.error('Subject required');
      return false;
    }
    if (!this.education.institute) {
      this.toastr.error('Institute required');
      return false;
    }
    if (!this.education.city) {
      this.toastr.error('City required');
      return false;
    }
    if (!this.education.state) {
      this.toastr.error('State required');
      return false;
    }
    if (!this.education.country) {
      this.toastr.error('Country required');
      return false;
    }
    if (!this.education.completionDate) {
      this.toastr.error('Completion Date required');
      return false;
    }
    if (!this.education.certificate) {
      this.toastr.error('Certificate required');
      return false;
    }
    return true;
  }

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteEducation = currentRowData.education;
  }
  deleteItem() {
    this.educationService
      .deleteEducation(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getFilterEducation();
          this.getEducation();
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  onPageSizeChange(pageSize: number) {
    this.PageSize = pageSize;
  }

  onPageNumberChange(pageNumber: number) {
    this.PageNumber = pageNumber;
  }
  gridReady(params: any) {
    this.gridApi = params.api;
  }

  getDataRowsourse(event: RequestWithFilterAndSort) {
    // // debugger
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

    this.getEducationLevel();
  }
}
function saveAs(file: File, arg1: string) {
  throw new Error('Function not implemented.');
}
