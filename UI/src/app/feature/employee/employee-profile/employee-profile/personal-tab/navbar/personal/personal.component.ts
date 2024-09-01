import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { EmployeeService } from '../../../../../../../shared/services/employee.service';
import {
  employee,
  LanguageCompetence,
  EmployeeYearlyLeaveBalance,
} from '../../../../../../../shared/models/employee';
import { leaveTypeService } from '../../../../../../../shared/services/leaveType.service';
import { ToastrService } from 'ngx-toastr';
import { LanguageLevelService } from '../../../../../../../shared/services/language-level.service';
import { first } from 'rxjs';
import {
  FormBuilder,
  FormGroup,
  FormControl,
  RequiredValidator,
  Validators,
  FormArray,
  AbstractControl,
} from '@angular/forms';
import { ActionEnum } from '../../../../../../../shared/constant/enum.const';
import { LeaveTypeEmployee } from '../../../../../../../shared/constant/enum.const';
import { SlgGroupService } from '../../../../../../../shared/services/slg-group.service';
import { DesignationService } from '../../../../../../../shared/services/designation.service';
import { roleservice } from '../../../../../../../shared/services/role.service';
import { CommonModule, formatDate } from '@angular/common';
import { v4 as uuidv4 } from 'uuid';

import {
  RequestWithFilterAndSort,
  sortModel,
} from '../../../../../../../shared/models/FilterRequset';
import {
  commonHelper,
  totalPageArray,
} from '../../../../../../../shared/helpers/common.helpers';
import { CountryService } from '../../../../../../../shared/services/country.service';
import { __values } from 'tslib';
import { PermissionService } from '../../../../../../../shared/services/permission.service';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeType } from '../../../../../../../shared/models/employeeType';
import { EmployeeFlagService } from '../../../../../../../shared/services/EmployeFlag.service';

@Component({
  selector: 'app-personal',
  templateUrl: './personal.component.html',
  styleUrl: './personal.component.scss',
})
export class PersonalComponent implements OnInit {

  isFormValid = false;
  LeaveType: any;
  languageForm: any;
  showOtherLanguageGroup: boolean = false;
  editId: string = '';
  // showEdit: boolean = true;
  showDelete: boolean = true;
  totalPages: number[] = [];
  totalItems: any;
  languagelevedata: any;
  selectedFileName: { [key: string]: string } = {};
  otherLanguages!: FormArray;
  // englishFormGroup!: FormGroup;
  Language: any[] = [];
  germanyFormGroup!: FormGroup;
  otherFormGroup!: FormGroup;
  parentFormGroup!: FormGroup;
  dataRowSource!: any[];
  previousValues!: any[];
  defaultFormGroup!: FormGroup;
  Employeees: any[] = [];
  leaveTypes: any[] = [];
  adjustEndDateCheckedControl = new FormControl(false);
  formattedDateControl = new FormControl(Date);
  selectedDate!: Date;
  formattedDate: string = '';
  firstDayOfMonth: string = '';
  lastDayOfYear: string = '';
  formattedDateValue: any;
  leaveEndDateValue: any;
  isUnlimitedAnnualLeaveType: boolean = false;
  isUnlimitedSickLeaveType: boolean = false; // Boolean flag to control visibility for Sick Leave
  isUnlimitedSpecialLeaveType: boolean = false; // Boolean flag to control visibility for Special Leave
  SelectedEmployee!: string;
  languageForms!: FormGroup;
  languagesArray: any[] = [];
  lang!: string;
  flag: boolean = false;
  Editflag: boolean = true;
  currentId!: string;
  fileName!: string;
  isFileSelected!: boolean;
  fileNamePersonalSheet!: string;
  isFileSelectedPersonal!: boolean;
  SSfileName!: string;
  isFileSelectedSS!: boolean;
  socialcarefileName!: string;
  isFileSelectedsocialcare!: boolean;
  birthcertificatefileName!: string;
  isFileSelectedbirthcertificate!: boolean;
  fileNameAdjustedofemplyee!: string;
  isFileSelectedAdjustedofemployee!: boolean;
  TerminationfileName!: string;
  isFileSelectedTermination!: boolean;


  constructor(
    private employeeservies: EmployeeService,
    private toastr: ToastrService,
    private languagelevel: LanguageLevelService,
    private formBuilder: FormBuilder,
    private Slgservice: SlgGroupService,
    private Designationservice: DesignationService,
    private translateService: TranslateService,
    private Rolservice: roleservice,
    private totalPageArray: totalPageArray,
    private countryservice: CountryService,
    private permission: PermissionService,
    private leaveTypeService: leaveTypeService,
    private route: ActivatedRoute,
    private EmployeFlage:EmployeeFlagService,
    private spinner: NgxSpinnerService
  ) {}
  columnDefs: any[] = [];

  private setLeaveEndDate(value: any): void {
    this.LeaveType.get('Annual')?.get('leaveEndDate')?.setValue(value);
    this.LeaveType.get('Sick')?.get('leaveEndDate')?.setValue(value);
    this.LeaveType.get('Special')?.get('leaveEndDate')?.setValue(value);
  }

  ngOnInit(): void {
   
    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {   
      console.warn(employee);  
      this.SelectedEmployee = employee;
      if (!this.flag) {
        this.GetEmployeeById();
      }
      console.log('Selected Employee in Personal Component:', this.SelectedEmployee);
    });


    this.lang = localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.personalDetails.language'
          ),
          field: 'name',
          sortable: false,
          filterParams: { maxNumConditions: 1 },
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.personalDetails.languageLevel'
          ),
          field: 'levelName',
          sortable: false,
          filterParams: { maxNumConditions: 1 },
        },
        {
          headerName: this.translateEnumValue(
            'i18n.employeeProfile.personal-details.personalDetails.languageCertificate'
          ),
          field: 'languagesCertificate',
          sortable: false,
          filterParams: { maxNumConditions: 1 },
        },
      ];
    }, 50);
    this.GetRole();
    
    // this.handleEditFlagFalse();
    this.getcountry();
    this.GetDesignation();
    this.getEmployee();
    this.GetLanguageLevel();
    this.GetEmploymentType();
    this.GetTypeofEmployment();
    this.GetLeaveTypeType();
    this.Gettaxclass();
    this.GetMaritalStatus();
    this.GetEmployeenStatus();
    this.GetDeliverymethod();
    this.initializeFormLanguage();
   

    

    this.selectedDate = new Date(); // Initialize selectedDate to current date
    this.formattedDate = formatDate(this.selectedDate, 'yyyy-MM-dd', 'en-US');

    const today = new Date();
    const currentYear = today.getFullYear();
    this.firstDayOfMonth = this.getFirstDayOfJanuary();
    // console.log('First day of January of current year:', this.firstDayOfMonth);
    this.lastDayOfYear = this.getLastDayOfYear();
    // console.log('Last day of December of current year:', this.lastDayOfYear);

    this.formattedDateControl.valueChanges.subscribe((value: any) => {
      this.formattedDateValue = value;
      console.warn('date', this.formattedDateValue);
    });

    // Subscribe to value changes of adjustEndDateCheckedControl
    this.adjustEndDateCheckedControl.valueChanges.subscribe((value) => {
      if (value) {
        // If adjustEndDateCheckedControl is true, use formattedDateValue or fallback to lastDayOfYear
        this.leaveEndDateValue = this.formattedDateValue || this.lastDayOfYear;
        this.setLeaveEndDate(this.leaveEndDateValue);
      } else {
        // If adjustEndDateCheckedControl is false, use lastDayOfYear
        this.setLeaveEndDate(this.lastDayOfYear);
      }
    });

    this.adjustEndDateCheckedControl.valueChanges.subscribe((value) => {
      this.LeaveType.get('Annual')?.get('adjustedEndDate')?.setValue(value);
      this.LeaveType.get('Sick')?.get('adjustedEndDate')?.setValue(value);
      this.LeaveType.get('Special')?.get('adjustedEndDate')?.setValue(value);
    });

    this.initializeForm();

    this.languageForm = this.formBuilder.group({
      English: this.formBuilder.group({
        name: ['English', Validators.required],
        level: ['', Validators.required],
        levelName:[''],
        languagesCertificate: ['', Validators.required],
      }),
      Germany: this.formBuilder.group({
        name: ['Germany', Validators.required],
        level: ['', Validators.required],
        levelName:[''],
        languagesCertificate: ['', Validators.required],
      }),
      Other: this.formBuilder.group({
        name: ['', Validators.required],
        level: ['', Validators.required],
        levelName:[''],
        languagesCertificate: ['', Validators.required],
        Addlanguage: this.formBuilder.array([]),
      }),
    });

    // Subscribe to form value changes if languageForm and its controls are not null
    if (
      this.languageForm &&
      this.languageForm.get('English') &&
      this.languageForm.get('Germany') &&
      this.languageForm.get('Other')
    ) {
      this.subscribeToFormChanges();
    }
  }
  retrieveSelectedEmployee() {
    // Retrieve the JSON string from localStorage
    let selectedEmployeeJson: string | null = localStorage.getItem('SelectedEmployeeForEdit');

    // Parse the JSON string back to a JavaScript object, or assign a default value if it's null
    let selectedEmployee = selectedEmployeeJson ? JSON.parse(selectedEmployeeJson) : null;

    if (selectedEmployee !== null) {
      // Remove quotes if selectedEmployee is a quoted string
      if (typeof selectedEmployee === 'string') {
        selectedEmployee = selectedEmployee.replace(/^"|"$/g, '');
      }
      // Now you can use the 'selectedEmployee' variable
      this.SelectedEmployee = selectedEmployee;
    } else {
      this.SelectedEmployee = 'No selected employee found in localStorage.';
    }

    // Output the message to the console
    console.log('identity',this.SelectedEmployee);
  }

  subscribeToFormChanges() {
    // Subscribe to form value changes if languageForm and its controls are not null
    if (
      this.languageForm &&
      this.languageForm.get('English') &&
      this.languageForm.get('Germany') &&
      this.languageForm.get('Other')
    ) {
      this.languageForm.valueChanges.subscribe((value: any) => {
        const English = this.languageForm.get('English').value;
        const Germany = this.languageForm.get('Germany').value;

        const otherGroup = this.languageForm.get('Other');
        if (otherGroup) {
          const { name, level, languagesCertificate } = otherGroup.value;
          const Addlanguage = otherGroup.get('Addlanguage').value;

          this.Language = [];

          // Add English if valid
          if (English.name && English.level && English.languagesCertificate) {
            this.Language.push(English);
          }

          // Add Germany if valid
          if (Germany.name && Germany.level && Germany.languagesCertificate) {
            this.Language.push(Germany);
          }

          // Add Other if valid
          if (name && level && languagesCertificate) {
            this.Language.push({ name, level, languagesCertificate });
          }

          // Add additional languages if any
          Addlanguage.forEach((lang: any) => {
            if (lang.name && lang.level && lang.languagesCertificate) {
              this.Language.push(lang);
            }
          });

          console.warn( this.Language);
          
        }
      });
    }
  }

  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }


  initializeFormLanguage(): void {
    this.languageForms = this.formBuilder.group({
      otherLanguage: new FormControl('', Validators.required),
      level: new FormControl('', Validators.required),
      levelName:new FormControl(''),
      languageCertificate: new FormControl(''),
    });
  }
  
  onSubmit(): void {
    const selectedLevel = this.languagelevedata.find(
      (level: { id: any }) => level.id === this.languageForms.value.level
    );
    const formData = {
      employeeId: this.Employeess.id,
      tempId: uuidv4(), // Generate a temporary ID for new entries
      id: null, // New entry so no ID
      name: this.languageForms.value.otherLanguage,
      level: this.languageForms.value.level,
      levelName: selectedLevel ? selectedLevel.level : '', // Map level ID to level name
      languagesCertificate: this.languageForms.value.languageCertificate,
    };
  
    this.addLanguage(formData);
    this.languageForms.reset();
  }
  
  addLanguage(formData: any): void {
    this.languagesArray.push(formData);
  
    this.dataRowSource = this.languagesArray.map(language => {
      if (language.id === null) {
        const { id, ...rest } = language;
        return rest;
      }
      return language;
    });
  
   
  }
  
  onDeleteClick(currentRowData: any): void {
    let index = -1;
  
    if (currentRowData.id) {
      index = this.languagesArray.findIndex(item => item.id === currentRowData.id);
    } else {
      index = this.languagesArray.findIndex(item => item.tempId === currentRowData.tempId);
    }
  
    if (index !== -1) {
      this.languagesArray.splice(index, 1);
  
      this.dataRowSource = this.languagesArray.map(language => {
        if (language.id === null) {
          const { id, ...rest } = language;
          return rest;
        }
        return language;
      });
  
     
      this.toastr.success('Language competence will be delete ');
    } else {
      this.toastr.error('Language competence not found');
    }
  }
  
  selectedFileNames: any;
 
  onFileSelectedlanguage(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFileNames = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`; 
        this.languageForms.patchValue({
                  languageCertificate: Concatenatefile,
                });
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedFileNames = '';
    }
  }
  getFileExtension(filename: string): string {
    return filename.split('.').pop()?.toLowerCase() || ''; // Get the lowercase file extension
  }

  get Addlanguage(): FormArray {
    return this.languageForm.get('Other.Addlanguage') as FormArray;
  }

  AddOtherLanguage(): void {
    const levelControl = this.languageForm.get('Other.level').value;
    const nameControl = this.languageForm.get('Other.name').value;
    const certificateControl = this.languageForm.get(
      'Other.languagesCertificate'
    ).value;

    // Check if all required fields are filled
    if (!levelControl || !nameControl || !certificateControl) {
      // console.error('Please fill in all required fields.');
      this.toastr.error(
        'Please fill in all required fields in Other Language.'
      );
      return; // Do not add a new language if any required field is empty
    }

    // If all required fields are filled, proceed to add a new language
    this.Addlanguage.push(
      this.formBuilder.group({
        name: ['', Validators.required],
        level: ['', Validators.required],
        languagesCertificate: ['', Validators.required],
      })
    );

    // Add the newly added language form group to the Language array
    const newLanguage = this.Addlanguage.at(this.Addlanguage.length - 1).value;
    this.Language.push(newLanguage);
  }

  DeleteLanguage(i: number) {
    // Remove the language form group from the FormArray
    this.Addlanguage.removeAt(i);

    // Remove the corresponding language from the Language array
    this.Language.splice(i, 1);
  }


  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);
  }

  initializeForm(): void {
    this.LeaveType = this.formBuilder.group({
      Annual: this.formBuilder.group({
        id: [null],
        employeeId: [null],
        leaveStartDate: [this.firstDayOfMonth, Validators.required],
        leaveEndDate: [this.lastDayOfYear, Validators.required],
        leaveTypesEmployee: ['', Validators.required],
        leaveTypeEmployee: ['', Validators.required],
        leaveQuota: ['', Validators.required],
        adjustedEndDate: [this.adjustEndDateCheckedControl.value],
      }),
      Sick: this.formBuilder.group({
        id: [null],
        leaveStartDate: [this.firstDayOfMonth, Validators.required],
        leaveEndDate: [this.lastDayOfYear, Validators.required],
        leaveTypesEmployee: ['', Validators.required],
        leaveTypeEmployee: ['', Validators.required],
        leaveQuota: ['', Validators.required],
        adjustedEndDate: [this.adjustEndDateCheckedControl.value],
      }),
      Special: this.formBuilder.group({
        id: [null],
        leaveStartDate: [this.firstDayOfMonth, Validators.required],
        leaveEndDate: [this.lastDayOfYear, Validators.required],
        leaveTypesEmployee: ['', Validators.required],
        leaveTypeEmployee: ['', Validators.required],
        leaveQuota: ['', Validators.required],
        adjustedEndDate: [this.adjustEndDateCheckedControl.value],
      }),
    });

    this.setupLeaveTypeSubscriptions();
    this.subscribeToFormChanges();
  }

  setupLeaveTypeSubscriptions(): void {
    this.LeaveType.get('Annual.leaveTypesEmployee')?.valueChanges.subscribe(
      (value: string) => {
        this.isUnlimitedAnnualLeaveType =
          value === '6fa85f64-5717-4562-b3fc-2c963f66afa2';
        this.LeaveType.get('Annual.leaveQuota')?.setValue(
          this.isUnlimitedAnnualLeaveType ? -1 : ''
        );
      }
    );

    this.LeaveType.get('Sick.leaveTypesEmployee')?.valueChanges.subscribe(
      (value: string) => {
        this.isUnlimitedSickLeaveType =
          value === '6fa85f64-5717-4562-b3fc-2c963f66afa2';
        this.LeaveType.get('Sick.leaveQuota')?.setValue(
          this.isUnlimitedSickLeaveType ? -1 : ''
        );
      }
    );

    this.LeaveType.get('Special.leaveTypesEmployee')?.valueChanges.subscribe(
      (value: string) => {
        this.isUnlimitedSpecialLeaveType =
          value === '6fa85f64-5717-4562-b3fc-2c963f66afa2';
        this.LeaveType.get('Special.leaveQuota')?.setValue(
          this.isUnlimitedSpecialLeaveType ? -1 : ''
        );
      }
    );
  }

  subscribeToFormChangees(): void {
    this.LeaveType.valueChanges.subscribe((value: any) => {
      // Define a type that includes an optional id property
      type LeaveType = {
        id?: any;
        leaveName: string;
        leaveStartDate: any;
        leaveEndDate: any;
        leaveTypesEmployee: any;
        leaveTypeEmployee: any;
        leaveQuota: any;
        adjustedEndDate: any;
      };

      // Updating leaveTypes array with current form values, including id only if it has a value
      this.leaveTypes = Object.keys(value).map((key) => {
        const leaveType: LeaveType = {
          leaveName: key + ' Leave',
          leaveStartDate: value[key].leaveStartDate,
          leaveEndDate: value[key].leaveEndDate,
          leaveTypesEmployee: value[key].leaveTypesEmployee,
          leaveTypeEmployee: value[key].leaveTypeEmployee,
          leaveQuota: value[key].leaveQuota,
          adjustedEndDate: value[key].adjustedEndDate,
        };

        if (value[key].id !== null) {
          leaveType.id = value[key].id;
        }

        return leaveType;
      });
    });
  }

  patchFormValues(): void {
    this.leaveTypes.forEach((leaveType: any) => {
      if (leaveType.leaveName.trim() === 'Annual Leave') {
        this.LeaveType.get('Annual')?.patchValue({
          id: leaveType.id,
          leaveStartDate: leaveType.leaveStartDate,
          leaveEndDate: leaveType.leaveEndDate,
          leaveTypesEmployee: leaveType.leaveTypesEmployee,
          leaveTypeEmployee: leaveType.leaveTypeEmployee,
          leaveQuota: leaveType.leaveQuota,
          adjustedEndDate: leaveType.adjustedEndDate,
        });
      } else if (leaveType.leaveName.trim() === 'Sick Leave') {
        this.LeaveType.get('Sick')?.patchValue({
          id: leaveType.id,
          leaveStartDate: leaveType.leaveStartDate,
          leaveEndDate: leaveType.leaveEndDate,
          leaveTypesEmployee: leaveType.leaveTypesEmployee,
          leaveTypeEmployee: leaveType.leaveTypeEmployee,
          leaveQuota: leaveType.leaveQuota,
          adjustedEndDate: leaveType.adjustedEndDate,
        });
      } else if (leaveType.leaveName.trim() === 'Special Leave') {
        this.LeaveType.get('Special')?.patchValue({
          id: leaveType.id,
          leaveStartDate: leaveType.leaveStartDate,
          leaveEndDate: leaveType.leaveEndDate,
          leaveTypesEmployee: leaveType.leaveTypesEmployee,
          leaveTypeEmployee: leaveType.leaveTypeEmployee,
          leaveQuota: leaveType.leaveQuota,
          adjustedEndDate: leaveType.adjustedEndDate,
        });
      }
    });

    console.log('Form Values After Patching:', this.LeaveType.value);
  }

  getUUIDFromEnumValue(value: LeaveTypeEmployee): string {
    switch (value) {
      case LeaveTypeEmployee.Quotabased:
        return '6fa85f64-5717-4562-b3fc-2c963f66afa1';
      case LeaveTypeEmployee.Unlimited:
        return '6fa85f64-5717-4562-b3fc-2c963f66afa2';
      default:
        return ''; // Return default value or handle error as per your requirement
    }
  }

  showSpecialLeaveEntitlement: boolean = false;
  showAnnualLeaveEntitlement: boolean = false;
  showSickLeaveEntitlement: boolean = false;

  toggleAnnualLeaveField() {
    const annualLeaveType =
      this.LeaveType.get('Annual').get('leaveTypeEmployee').value;
    // Show the field only when "Quota-based" is selected
    this.showAnnualLeaveEntitlement = annualLeaveType === 'Quota-based';
  }

  toggleSickLeaveField() {
    const sickLeaveType =
      this.LeaveType.get('Sick').get('leaveTypeEmployee').value;
    // Show the field only when "Quota-based" is selected
    this.showSickLeaveEntitlement = sickLeaveType === 'Quota-based';
  }

  toggleSpecialLeaveField() {
    const specialLeaveType =
      this.LeaveType.get('Special').get('leaveTypeEmployee').value;
    // Show the field only when "Quota-based" is selected
    this.showSpecialLeaveEntitlement = specialLeaveType === 'Quota-based';
  }

  //#region All Date Function

  getCurrentDate(): string {
    const currentDate = new Date();
    return currentDate.toISOString().substring(0, 10); // Format as YYYY-MM-DD
  }

  ////////date for Leave function
  getFirstDayOfJanuary(): string {
    // Get the current year
    const currentYear = new Date().getFullYear();

    // Create a new Date object with the current year, January (month 0), and day 1
    const firstDayOfJanuary = new Date(currentYear, 0, 1); // Month 0 represents January

    // Extract the date part in 'YYYY-MM-DD' format
    const dateString = firstDayOfJanuary.toISOString().split('T')[0];

    return dateString;
  }

  getLastDayOfYear(): string {
    // Get the current year
    const currentYear = new Date().getFullYear();

    // Create a new Date object with the current year, December (month 11), and day 31
    const lastDayOfYear = new Date(currentYear, 11, 31);

    // Extract the date part in 'YYYY-MM-DD' format
    const dateString = lastDayOfYear.toISOString().split('T')[0];

    return dateString;
  }

  //#endregion

  GetLeaveTypes: any;
  annualLeaveId!: string;
  sickLeaveId!: string;
  specialLeaveId!: string;
  employees: any[] = [];
  slg1Employees: any[] = [];
  slg2And3Employees: any[] = [];
  slg4Employees: any[] = [];

  //#region  GETEmployeeSLG Filte
  getEmployee() {
    this.employeeservies.getEmployee().subscribe(
      (data: any) => {
        this.employees = data.httpResponse;

        // Filter employees based on slgStatusname
        this.slg1Employees = this.employees.filter(
          (employee) => employee.slgStatusname === 'SLG 1'
        );
        this.slg2And3Employees = this.employees.filter(
          (employee) =>
            employee.slgStatusname === 'SLG 2' ||
            employee.slgStatusname === 'SLG 3'
        );
        this.slg4Employees = this.employees.filter(
          (employee) => employee.slgStatusname === 'SLG 4'
        );
      },
      (error: any) => {}
    );
  }
  //#endregion

  //#region  GetLeaveType
  GetLeaveTypeType() {
    this.leaveTypeService
      .getLeaves()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.GetLeaveTypes = resp.httpResponse;
          // console.warn('LeaveTypes', this.GetLeaveTypes);

          // Extracting and assigning IDs based on typeName
          this.GetLeaveTypes.forEach((leaveType: any) => {
            if (leaveType.typeName === 'Annual Leave ') {
              this.annualLeaveId = leaveType.id;
            } else if (leaveType.typeName === 'Sick Leave') {
              this.sickLeaveId = leaveType.id;
            } else if (leaveType.typeName === 'Special Leave') {
              this.specialLeaveId = leaveType.id;
            }
          });

          // Logging the extracted IDs

          // Update form controls with the fetched IDs
          this.updateFormControls();
        },
        error: (err: any) => {},
      });
  }

  updateFormControls() {
    this.LeaveType.get('Annual.leaveTypeEmployee')?.setValue(
      this.annualLeaveId
    );
    this.LeaveType.get('Sick.leaveTypeEmployee')?.setValue(this.sickLeaveId);
    this.LeaveType.get('Special.leaveTypeEmployee')?.setValue(
      this.specialLeaveId
    );
  }

  //#endregion

  Deliverymethod: any;
  filteredSLGGroups: any[] = [];
  Designationdata!: any[];
  Countrydata: any;
  Roledata: any;
  EmployeLeave: any[] = [];
  TypeofEmployment: any;
  taxclass: any;
  MaritalStatus: any;
  EmployeenStatus: any;
  EmploymentType: any;
  LeaveTypeEmployee = LeaveTypeEmployee;
  previousEmailValidity: boolean = true;
  isEmailValid: boolean = true;
  //genral
  Employeess = {} as employee;
  EmployeeYearlyLeaveBalance = {} as EmployeeYearlyLeaveBalance;
  LanguageCompetence = {} as LanguageCompetence;
  imageUrl!: string | ArrayBuffer;

  //#region  OnFileSelected All Function

  // onFileSelectedProfile(event: any) {
  //   const file: File = event.target.files[0];
  //   const fileName = file.name; // Get the filename

  //   // Update the label text with the filename
  //   const label = document.querySelector('.label[data-js-label]');
  //   if (label) {
  //     label.textContent = fileName;
  //   }

  //   const reader = new FileReader();

  //   reader.onload = (e: any) => {
  //     this.imageUrl = e.target.result;
  //     this.Employeess.profilePhoto = e.target.result;
  //   };

  //   reader.readAsDataURL(file);
  // }
  onFileSelectedProfile(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.fileName = input.files[0].name;
      this.isFileSelected = true;
    }
    const file: File = event.target.files[0];
    if (file) {
      this.fileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.imageUrl = e.target.result;
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Employeess.profilePhoto = Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.fileName = '';
    }
  }


  // onFileSelectedesPersonalsheet(event: any) {
  //   const fileInput = event.target;
  //   const label = fileInput.parentElement.querySelector('[data-js-label]');

  //   if (fileInput.files.length > 0) {
  //     const fileName = fileInput.files[0].name;
  //     label.textContent = fileName;

  //     const file: File = event.target.files[0];
  //     const reader = new FileReader();

  //     reader.onload = (e: any) => {
  //       // Assign the file data to the Employeess property
  //       this.Employeess.personalSheet = e.target.result;
  //     };

  //     reader.readAsDataURL(file);
  //   } else {
  //     label.textContent = 'Upload document (Personal Sheet)';
  //   }
  // }

  onFileSelectedesPersonalsheet(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.fileNamePersonalSheet = input.files[0].name;
      this.isFileSelectedPersonal = true;
    }
    const file: File = event.target.files[0];
    if (file) {
      this.fileNamePersonalSheet = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Employeess.personalSheet = Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.fileNamePersonalSheet = '';
    }
  }



  // onFileSelectedeSocialsecurityfile(event: any) {
  //   const fileInput = event.target;
  //   const label = fileInput.parentElement.querySelector('[data-js-label]');

  //   if (fileInput.files.length > 0) {
  //     const fileName = fileInput.files[0].name;
  //     label.textContent = fileName;

  //     const file: File = event.target.files[0];
  //     const reader = new FileReader();

  //     reader.onload = (e: any) => {
  //       // Assign the file data to the Employeess property
  //       this.Employeess.socialSecurityFile = e.target.result;
  //     };

  //     reader.readAsDataURL(file);
  //   } else {
  //     label.textContent = 'Upload document (SSN)';
  //   }
  // }

  onFileSelectedeSocialsecurityfile(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.SSfileName = input.files[0].name;
      this.isFileSelectedSS = true;
    }
    const file: File = event.target.files[0];
    if (file) {
      this.SSfileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Employeess.socialSecurityFile = Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.SSfileName = '';
    }
  }



  onFileSelected(event: any) {
    const fileInput = event.target;
    const label = fileInput.parentElement.querySelector('[data-js-label]');
    const file = fileInput.files[0];

    if (file) {
      const fileName = file.name;
      label.textContent = fileName;

      // Read the file content as a base64 encoded string
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String = e.target.result.split(',')[1];
        // Now you have the base64 encoded string, you can use it as needed
        // console.log(base64String);
      };
      reader.readAsDataURL(file);
    } else {
      label.textContent = 'Upload file';
    }
  }

  onFileSelectedone(event: any) {
    const fileInput = event.target;
    const label = fileInput.parentElement.querySelector('[data-js-label]');
    const file = fileInput.files[0];

    if (file) {
      const fileName = file.name;
      label.textContent = fileName;

      // Read the file content as a base64 encoded string
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String = e.target.result.split(',')[1];
        // Now you have the base64 encoded string, you can use it as needed
        // console.log(base64String);
      };
      reader.readAsDataURL(file);
    } else {
      label.textContent = 'Upload file';
    }
  }

  onFileSelectedtwo(event: any) {
    const fileInput = event.target;
    const label = fileInput.parentElement.querySelector('[data-js-label]');
    const file = fileInput.files[0];

    if (file) {
      const fileName = file.name;
      label.textContent = fileName;

      // Read the file content as a base64 encoded string
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String = e.target.result.split(',')[1];
        // Now you have the base64 encoded string, you can use it as needed
        // console.log(base64String);
      };
      reader.readAsDataURL(file);
    } else {
      label.textContent = 'Upload file';
    }
  }

  onFileSelectedthree(event: any) {
    const fileInput = event.target;
    const label = fileInput.parentElement.querySelector('[data-js-label]');
    const file = fileInput.files[0];

    if (file) {
      const fileName = file.name;
      label.textContent = fileName;

      // Read the file content as a base64 encoded string
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String = e.target.result.split(',')[1];
        // Now you have the base64 encoded string, you can use it as needed
        // console.log(base64String);
      };
      reader.readAsDataURL(file);
    } else {
      label.textContent = 'Upload file';
    }
  }

  //employeechild
  // onFileSelectedesocialCareInsurancefile(event: any) {
  //   const fileInput = event.target;
  //   const label = fileInput.parentElement.querySelector('[data-js-label]');

  //   if (fileInput.files.length > 0) {
  //     const fileName = fileInput.files[0].name;
  //     label.textContent = fileName;

  //     const file: File = event.target.files[0];
  //     const reader = new FileReader();

  //     reader.onload = (e: any) => {
  //       // Assign the file data to the Employeess property
  //       this.Employeess.socialcareinsurance = e.target.result;
  //     };

  //     reader.readAsDataURL(file);
  //   } else {
  //     label.textContent = 'Upload document (Social Care Insurance)';
  //   }
  // }

  onFileSelectedesocialCareInsurancefile(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.socialcarefileName = input.files[0].name;
      this.isFileSelectedsocialcare = true;
    }
    const file: File = event.target.files[0];
    if (file) {
      this.socialcarefileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Employeess.socialcareinsurance = Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.socialcarefileName = '';
    }
  }


  // onFileSelectedebirthCertificatefile(event: any) {
  //   const fileInput = event.target;
  //   const label = fileInput.parentElement.querySelector('[data-js-label]');

  //   if (fileInput.files.length > 0) {
  //     const fileName = fileInput.files[0].name;
  //     label.textContent = fileName;

  //     const file: File = event.target.files[0];
  //     const reader = new FileReader();

  //     reader.onload = (e: any) => {
  //       // Assign the file data to the Employeess property
  //       this.Employeess.birthCertificate = e.target.result;
  //     };

  //     reader.readAsDataURL(file);
  //   } else {
  //     label.textContent = 'Upload document (Birth Certificate)';
  //   }
  // }


  onFileSelectedebirthCertificatefile(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.birthcertificatefileName = input.files[0].name;
      this.isFileSelectedbirthcertificate = true;
    }
    const file: File = event.target.files[0];
    if (file) {
      this.birthcertificatefileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Employeess.birthCertificate = Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.birthcertificatefileName = '';
    }
  }


  /////Probation
  // onFileSelectedeAdjustedofemplyeefile(event: any) {
  //   const fileInput = event.target;
  //   const label = fileInput.parentElement.querySelector('[data-js-label]');

  //   if (fileInput.files.length > 0) {
  //     const fileName = fileInput.files[0].name;
  //     label.textContent = fileName;

  //     const file: File = event.target.files[0];
  //     const reader = new FileReader();

  //     reader.onload = (e: any) => {
  //       // Assign the file data to the Employeess property
  //       this.Employeess.adjustedDocument = e.target.result;
  //     };

  //     reader.readAsDataURL(file);
  //   } else {
  //     label.textContent = 'Upload file';
  //   }
  // }

  onFileSelectedeAdjustedofemplyeefile(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.fileNameAdjustedofemplyee = input.files[0].name;
      this.isFileSelectedAdjustedofemployee = true;
    }
    const file: File = event.target.files[0];
    if (file) {
      this.fileNameAdjustedofemplyee = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Employeess.adjustedDocument = Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.fileNameAdjustedofemplyee = '';
    }
  }


  onFileSelectedeTerminationofemplyeefile(event: any): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.TerminationfileName = input.files[0].name;
      this.isFileSelectedTermination = true;
    }
    const file: File = event.target.files[0];
    if (file) {
      this.TerminationfileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Employeess.terminationofemplyee = Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.TerminationfileName = '';
    }
  }


  //#endregion

  //#region  GetDesignation with SLG Filter
  GetDesignation() {
    this.Designationservice.GetAllDesignation()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.Designationdata = resp.httpResponse;
        },
        error: (error: any) => {
          console.error('Error fetching designation:', error);
        },
      });
  }
  
  onDesignationChange() {
    const selectedDesignation = this.Designationdata.find(
      (item) => item.id === this.Employeess.designation
    );
  
    if (selectedDesignation && selectedDesignation.slgGroup) {
      this.filteredSLGGroups = [selectedDesignation];
      this.Employeess.slgStatus = selectedDesignation.slgGroup.initialStatus;
      this.setHiddenValue(selectedDesignation.slgGroup.id);
    } else {
      this.filteredSLGGroups = [];
      this.Employeess.slgStatus = '';
    }
  }
  
  setHiddenValue(slgGroupId: string) {
    const selectedSLGGroup = this.filteredSLGGroups.find(
      (item) => item.slgGroup.id === slgGroupId
    );
  
    if (selectedSLGGroup) {
      this.Employeess.slgStatus = selectedSLGGroup.slgGroup.id;
    }
  }
  
  //#endregion

  //#region  GetData For DropDown

  GetLanguageLevel() {
    this.languagelevel.GetAllLanguagelevel().subscribe({
      next: (resp: any) => {
        this.languagelevedata = resp.httpResponse;
        //  console.warn('language',this.languagelevedata);
      },
    });
  }

  GetEmploymentType() {
    this.employeeservies
      .GetEmploymentType()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.EmploymentType = resp.httpResponse;
        },
        error: (err: any) => {},
      });
  }

  GetRole() {
    this.Rolservice.getRoleList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.dataRowSource = resp.httpResponse;
          this.Roledata = resp.httpResponse;
          // console.warn('conutry',this.Countrydata);
          // console.warn('Roledata',this.Roledata);
        },
      });
  }

  getcountry(): void {
    this.countryservice
      .getCountryList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.dataRowSource = resp.httpResponse;
          this.Countrydata = resp.httpResponse;
          //  console.log("country",this.Countrydata)
        },
        error: (err: any) => {
          this.toastr.error(err.message);
        },
      });
  }

  GetDeliverymethod() {
    this.employeeservies
      .GetDeliverymethod()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.Deliverymethod = resp.httpResponse;
        },
        error: (err: any) => {},
      });
  }

  GetTypeofEmployment() {
    this.employeeservies.GetTypeofEmployment().pipe(first()).subscribe({
      next: (resp: any) => {
        this.TypeofEmployment = resp.httpResponse.map((type: any) => {
          const employmentType = type as EmployeeType; // Type assertion
          employmentType.translationKey = 'i18n.employeeProfile.personal-details.job-History-Details.typeofemployeementdetails.' + type.typeofemployment.toLowerCase();
          return employmentType;
        });
      },
      error: (err: any) => {
        // Handle error
        console.error('Error fetching employment types:', err);
      }
    });
  }

  Gettaxclass() {
    this.employeeservies
      .Gettaxclass()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.taxclass = resp.httpResponse;
        },
        error: (err: any) => {},
      });
  }

  GetMaritalStatus() {
    this.employeeservies
      .GetMaritalStatus()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.MaritalStatus = resp.httpResponse;
        },
        error: (err: any) => {},
      });
  }

  GetEmployeenStatus() {
    this.employeeservies
      .GetEmployeenStatus()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.EmployeenStatus = resp.httpResponse;
        },
        error: (err: any) => {},
      });
  }

  //#endregion

  //#region GetEmployeeById
  GetEmployeeById() {
    if (this.SelectedEmployee) {
      // Check if SelectedEmployee has a value
      this.employeeservies
        .getEmployeeById(this.SelectedEmployee)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.Employeess = resp.httpResponse;
            // Ensure Employeess.slgStatus is set
            if (this.Employeess.slgStatus) {
            //  console.warn('employedata thorw get function', this.Employeess.slgStatus);
              this.onDesignationChange(); // Call onDesignationChange after setting Employeess
            }
            this.languagesArray = resp.httpResponse.languageCompetences;
            this.dataRowSource = [...this.languagesArray];

            this.leaveTypes = resp.httpResponse.employeeYearlyLeaveBalances;

            this.patchFormValues();
            this.LeaveType.patchValue({});
          },
          error: (err: any) => {
            this.toastr.error(err.message);
          },
        });
    } else {
     // this.toastr.warning('Selected Employee is not defined');
    }
  }

  onEmployeeSelected(employeeId: string) {
    this.SelectedEmployee = employeeId;
    this.GetEmployeeById();
  }

  //#endregion

  //#region saveEmployee
  SaveEmployee() {
    this.spinner.show();
    if (!this.Employeess.id) {
      this.Employeess.action = ActionEnum.Insert;
      this.Employeess.languageCompetences = this.Language;
      this.Employeess.rolename = '';
      this.Employeess.slgStatusname = '';
      this.Employeess.officeEmail = '';
      this.Employeess.fullName = '';
      this.Employeess.phoneNumber = '';
      // this.Employeess.languageCompetences.levelName = '';
      this.Employeess.workLocation = 0;
      this.Employeess.employeeYearlyLeaveBalances = this.leaveTypes;
      // this.Employeess.name=this.NameRole;
    } else {
      this.Employeess.rolename = '';
      this.Employeess.slgStatusname = '';
      this.Employeess.officeEmail = '';
      this.Employeess.fullName = '';
      this.Employeess.phoneNumber = '';
      this.Employeess.workLocation = 0;
      this.Employeess.languageCompetences = this.dataRowSource;
      this.Employeess.employeeYearlyLeaveBalances = this.leaveTypes;
      this.Employeess.action = ActionEnum.Update;
    }
    if (this.ValidateModalData()) {
      this.employeeservies
        .SaveEmployee(this.Employeess)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.spinner.hide();
            this.toastr.success(resp.message);
            // this.GetEmploye()
            this.Employeess = {} as employee;
            console.log("Employee:",this.Employeees);
            
            this.GetEmployeeById();
          },
          error: (err: any) => {
            this.toastr.error(err.message);
            this.spinner.hide();
          },
        });
    }
  }
  //#endregion
  onTerminationChange() {
    if (!this.Employeess.isTerminated) {
      this.Employeess.terminationStartDate = '';
      this.Employeess.terminationEndDate = '';
      this.Employeess.deliverymethodId = '';
      this.Employeess.dateofreceipt = '';
    }
  }
  onEndofEmployementChange() {
    if (!this.Employeess.isthistheendofemployment) {
      this.Employeess.noticePeriodEndDate = '';
      this.Employeess.noticePeriodStartDate = '';
    }
  }

  onProbationChange() {
    if (!this.Employeess.istheemployeeonprobation) {
      this.Employeess.startDate = '';
      this.Employeess.endDate = '';
      this.Employeess.adjustedenddatecheck = false;
      this.Employeess.prob_AdjustedEndDate = '';
      this.Employeess.probationUnlimited = false;
      // Additional logic to clear the file input if necessary
    }
  }

  onChildrenChange() {
    if (!this.Employeess.employeehavechildren) {
      this.Employeess.child_FirstName = '';
      this.Employeess.familyName = '';
      this.Employeess.chIld_BirthDate = '';
      this.Employeess.locationchildregistered = '';
      this.Employeess.birthCertificate = '';
      this.Employeess.socialcareinsurance = '';
      // Additional logic to clear the file inputs if necessary
    }
  }
  //#region ValidateModalData for Employee
  ValidateModalData(): boolean {
 
    if (!this.Employeess.employeeNumber) {
     
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.EmployeNamerequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.currentStatusId) {
    
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.CurrentStatusrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.employementTypeId) {
    
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.EmploymentTyperequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.typeofEmploymentId) {

      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.TypeofEmploymentrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.workingHours) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.WorkingHoursrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.jobTitle) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.JobTitlerequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.contractualStartDate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.ContractualStartDaterequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.contractualEndDate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.ContractualEndDaterequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.email) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.EmployeeEmailrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.firstName) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.EmployeeFirstNamerequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.middleName) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.EmployeeMiddleNamerequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.lastName) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.EmployeeLastNamerequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.gender) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.Genderrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.birthdate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.BirthDaterequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.birthCity) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.BirthCityrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.birthCountryId) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.BirthCountryrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.taxId) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.TaxIDrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.socialSecurity) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.SocialSecurityNumberrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.taxClassId) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.Taxclassrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.healthInsaurance) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.NameofHealthInsaurancerequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.maritalStatusId) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.MaritalStatusrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.religiousaffiliation) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.Religiousaffiliationrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.profilePhoto) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.ProfilePhotorequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.personalSheet) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.PersonalSheetrequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.socialSecurityFile) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.SocialSecurityFilerequired',(this.spinner.hide())
        )
      );
      return false;
    }
    if (!this.Employeess.employeeStatusId) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.personalDetails.EmployeeStatusrequired',(this.spinner.hide())
        )
      );
      return false;
    }

    return true;
  }
  //#endregion

  // handleInput(event: any) {
  //   const inputValue = event.target.value;

  //   // Check if input value has more than 10 characters
  //   if (inputValue.length > 10) {
  //     this.toastr.error('Please enter a maximum of 10 digits.');
  //     // Clear the input value or handle the error as needed
  //     event.target.value = ''; // Clears the input field
  //     return;
  //   }

  //   // Check if the input value contains only numbers
  //   const regex = /^[0-9]+$/;
  //   if (!regex.test(inputValue)) {
  //     this.toastr.error('Please enter a valid number.');
  //     // Clear the input value or handle the error as needed
  //     event.target.value = ''; // Clears the input field
  //   }
  // }

  //#region Validations Function

  handleInputsMaxWord(event: any) {
    const inputValue = event.target.value;
    const maxLength = 20; // Maximum allowed characters

    // Check if input value exceeds the maximum length
    if (inputValue.length > maxLength) {
     // this.toastr.error(`Please enter a maximum of ${maxLength} characters.`);
      // Truncate the input value or handle the error as needed
      event.target.value = inputValue.substring(0, maxLength); // Truncate the input value
    }
  }

  validateEmail() {
    const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    const isValid = regex.test(this.Employeess.email);
    this.isEmailValid = isValid;
    if (!isValid) {
      this.toastr.error('Please enter a valid email address.');
    }
  }
  handleInputEmployeName(event: any) {
    const inputValue = event.target.value;

    // Check if input value has more than 15 characters
    if (inputValue.length > 15) {
    //  this.toastr.error('Please enter a maximum of 15 characters.');
      // Truncate the input value to 15 characters
      event.target.value = inputValue.substring(0, 15);
      return;
    }

    // Check if the input value contains only letters
    const regex = /^[a-zA-Z\s]*$/;
    if (!regex.test(inputValue)) {
     // this.toastr.error('Please enter only letters.');
      // Clear the input value or handle the error as needed
      event.target.value = ''; // Clears the input field
    }
  }

  handleInputMaxWithLetter(event: any) {
    const inputValue = event.target.value;

    // Check if input value has more than 15 characters
    if (inputValue.length > 25) {
      this.toastr.error('Please enter a maximum of 25 characters.');
      // Truncate the input value to 15 characters
      event.target.value = inputValue.substring(0, 25);
      return;
    }

    // Check if the input value contains only letters
    const regex = /^[a-zA-Z\s]*$/;
    if (!regex.test(inputValue)) {
      this.toastr.error('Please enter only letters.');
      // Clear the input value or handle the error as needed
      event.target.value = ''; // Clears the input field
    }
  }

  handleInputMaxhealthInsaurance(event: any) {
    const inputValue = event.target.value;

    // Check if input value has more than 15 characters
    if (inputValue.length > 30) {
      this.toastr.error('Please enter a maximum of 30 characters.');
      // Truncate the input value to 15 characters
      event.target.value = inputValue.substring(0, 30);
      return;
    }

    // Check if the input value contains only letters
    const regex = /^[a-zA-Z\s]*$/;
    if (!regex.test(inputValue)) {
      this.toastr.error('Please enter only letters.');
      // Clear the input value or handle the error as needed
      event.target.value = ''; // Clears the input field
    }
  }

  handleInputTaxMax(event: any) {
    const inputValue = event.target.value;

    // Check if the input value contains only numbers
    const regex = /^[0-9]+$/;
    if (!regex.test(inputValue)) {
      this.toastr.error('Please enter only numbers.');
      // Clear the input value or handle the error as needed
      event.target.value = ''; // Clears the input field
      return;
    }

    // Check if input value has more than 25 characters
    if (inputValue.length > 25) {
      this.toastr.error('Please enter a maximum of 25 digits.');
      // Truncate the input value to 25 characters
      event.target.value = inputValue.substring(0, 25);
    }
  }

  //#endregion
}
