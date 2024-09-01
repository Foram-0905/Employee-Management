import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IdentityCardService } from '../../../../../../../shared/services/IdentityCard.service';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
// import { WorkPermitService } from '../../../../../../../shared/services/workPermit.service';
import { first } from 'rxjs';
// import { WorkPermit } from '../../../../../../../shared/models/work-permit';
import { ActionEnum } from '../../../../../../../shared/constant/enum.const';
import { IdentityCard } from '../../../../../../../shared/models/identity-card';
import { WorkPermitDetail } from '../../../../../../../shared/models/identity-card';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { formatDate } from '@angular/common';
import { v4 as uuidv4 } from 'uuid';
import { PermissionService } from '../../../../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFlagService } from '../../../../../../../shared/services/EmployeFlag.service';
@Component({
  selector: 'app-identity-card',
  templateUrl: './identity-card.component.html',
  styleUrl: './identity-card.component.scss',
})
export class IdentityCardComponent implements OnInit {
  dataRowSource!: any[];
  editId: any;
  showEdit: boolean = true;
  showDelete: boolean = true;
  totalPages: number[] = [];
  WorkPermitDetails: any[] = [];
  workpermitDetail = {} as WorkPermitDetail;
  totalItems: any;
  searchText: any;
  allWorkPermit: any;
  // WorkPermit = {} as WorkPermit;
  identity = {} as IdentityCard;
  selectedFileName: string = '';
  selectedPassportFileName: string = '';
  selectedVisaFileName: string = '';
  selectedBluecardFileName: string = '';
  showDeleteModal: boolean = false;
  DeleteWorkPermit: any;
  WorkPermit: any;
  userForm!: FormGroup;
  formDataArray: any[] = [];
  Permit: any[] = [];
  SelectedEmployee!: string;
  Editflag: boolean = true;
  currentId!: string;

  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addEditModal') addEditModal!: any;

  columnDefs = [
    {
      headerName: 'Type Of Permit',
      field: 'permitType',
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: 'Start Date ',
      field: 'permitStartdate',
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: 'Expiry Date',
      field: 'permitExpirytdate',
      filterParams: { maxNumConditions: 1 },
    },
    {
      headerName: 'Document',
      field: 'document',
      filterParams: { maxNumConditions: 1 },
    },
  ];
  constructor(
    private identityCardService: IdentityCardService,
    private translateService: TranslateService,
    private permission: PermissionService,
    private spinner: NgxSpinnerService,
    private formBuilder: FormBuilder,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private EmployeFlage: EmployeeFlagService
  ) {
    this.userForm = this.formBuilder.group({
      index: null,
      permitType: '',
      permitStartdate: '',
      permitExpirytdate: '',
      document: '',
    });
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  ngOnInit(): void {
    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
      this.dataRowSource = [];  
      this.identity = {} as IdentityCard; // Reset identity
      if (employee) {
        this.SelectedEmployee = employee;
        this.GetIdentityByEmployeeId();
      }
    });
    this.dataRowSource = [];
    this.initializeWorkPermit();
    this.route.queryParams.subscribe((params) => {
      this.Editflag = params['Editflag'] === 'true';
      this.currentId = params['id'];
    });
    // this.retrieveSelectedEmployee();
    
    // if (this.Editflag) {
    //   this.retrieveSelectedEmployee();
    // } else {
    //   this.SelectedEmployee = this.currentId;
    //   console.warn('dataayaedits',this.SelectedEmployee);
      
    // }
    this.GetIdentityByEmployeeId();

  }

  // DataClearfunction(){
  //   this.dataRowSource = []; // Reset dataRowSource
  //   this.WorkPermitDetails = []; // Reset dataRowSource
  //   this.identity = {} as IdentityCard; // Reset identity
  //       console.warn('clear');
  // }

  GetIdentityByEmployeeId() {
    this.identityCardService
      .GetIdentityCardByEmployeeId(this.SelectedEmployee)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // debugger
          if (resp.httpResponse) {
            this.identity = resp.httpResponse;
            console.log('identity ki value', resp.httpResponse);
            this.WorkPermitDetails = resp.httpResponse.workPermitDetails;
            // this.dataRowSource(this.identity.workPermitDetails);
            this.dataRowSource = [...this.WorkPermitDetails];
          }
        },
        error: (err: any) => {
          this.toastr.error(err.message);
        },
      });
  }

  SaveIdentityCard() {
    this.spinner.show();
    if (!this.identity.id) {
      this.identity.employeeId = this.SelectedEmployee;
      debugger
      this.identity.workPermitDetails = this.dataRowSource;

      this.identity.action = ActionEnum.Insert;
    } else {
      this.identity.action = ActionEnum.Update;
      this.identity.workPermitDetails = this.dataRowSource;
    }

    if (this.ValidateModalDatas()) {
      // Map Other Work Permits to match API structure
      // const otherWorkPermitsDTO = this.WorkPermitDetail.map((permit) => {
      //   return {
      //     permitType: permit.permitType,
      //     permitStartDate: permit.permitStartDate,
      //     permitExpiryDate: permit.permitExpiryDate,
      //     document: permit.document,
      //   };
      // });

      // Assign Other Work Permits to the main identity object
      // Assign Other Work Permits to the main identity object
      // this.identity.workPermitDetail = otherWorkPermitsDTO;

      this.identityCardService
        .saveIdentityCard(this.identity)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.identity = {} as IdentityCard;
            this.WorkPermitDetails = [];
            this.workpermitDetail = {} as WorkPermitDetail;
            this.spinner.hide();
            // Clear temporary entries after save
          },
          error: (err: any) => {
            this.toastr.error('Save Identity', err.message);
            this.spinner.hide();
          },
        });
    }
  }

  ValidateModalDatas(): boolean {
    if (!this.identity.passport) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.identiyCardDetails.passportRequired',
          this.spinner.hide()
        )
      );
      return false;
    }

    if (!this.identity.visaStartDate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.identiyCardDetails.visaStartDaterequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.identity.visaExpiryDate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.identiyCardDetails.visaExpiryDaterequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.identity.visa) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.identiyCardDetails.visarequired',
          this.spinner.hide()
        )
      );
      return false;
    }

    if (!this.identity.blueCardStartDate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.identiyCardDetails.blueCardStartDaterequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.identity.blueCardExpiryDate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.identiyCardDetails.blueCardExpiryDaterequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.identity.blueCard) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.identiyCardDetails.blueCardrequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.identity.workPermitDetails) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.personal-details.identiyCardDetails.permitTyperequired',
          this.spinner.hide()
        )
      );
      return false;
    }

    return true;
  }
  // addWorkPermit(formData: any) {
  //   debugger
  //   // Push the new Work Permit data to the temporary array
  //   this.WorkPermitDetails = this.WorkPermitDetails.filter((x: any) => x.index != formData.index)
  //   this.WorkPermitDetails.push(formData);
  //   console.log("Other Work Permits:", this.WorkPermitDetails);
  //   // this.WorkPermitDetail=this.dataRowSource
  //   this.dataRowSource = [...this.WorkPermitDetails]
  //   this.selectedFileName = '';

  // }

  clearWorkPermitData() {
    this.workpermitDetail = {} as WorkPermitDetail;
    this.selectedFileName = '';
  }

  // onFileSelected(event: any) {
  //   const file: File = event.target.files[0];
  //   if (file) {
  //     const reader = new FileReader();
  //     reader.onload = (e: any) => {
  //       // Convert file to base64 and store the result
  //       const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
  //       this.WorkPermit.patchValue({
  //         document: base64String
  //       });
  //       this.selectedFileName = file.name;
  //     };
  //     reader.readAsDataURL(file);
  //   } else {
  //     this.selectedFileName = ''; // Clear the file name if no file is selected
  //   }
  // }
  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        console.log('File Format:', fileFormat);
        const Concatenatefile = `${fileFormat}+${base64String}`;
        console.log('Whole File:', Concatenatefile);
        this.WorkPermit.patchValue({
          document: Concatenatefile,
        });
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedFileName = '';
    }
  }

  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }

  closeModal() {
    this.identity = {} as IdentityCard;
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
    }
  }
  // onPassportFileSelected(event: any) {
  //   const file: File = event.target.files[0];
  //   if (file) {
  //     this.selectedPassportFileName = file.name;

  //     const reader = new FileReader();
  //     reader.onload = (e: any) => {
  //       const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
  //       this.identity.passport = base64String;
  //     };
  //     reader.readAsDataURL(file);
  //   } else {
  //     this.selectedPassportFileName = '';
  //   }
  // }
  onPassportFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedPassportFileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        console.log('File Format:', fileFormat);
        const Concatenatefile = `${fileFormat}+${base64String}`;
        console.log('Whole File:', Concatenatefile);
        this.identity.passport = Concatenatefile;
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedPassportFileName = '';
    }
  }
  getFileExtension(filename: string): string {
    return filename.split('.').pop()?.toLowerCase() || ''; // Get the lowercase file extension
  }
  // onVisaFileSelected(event: any) {
  //   const file: File = event.target.files[0];
  //   if (file) {
  //     this.selectedVisaFileName = file.name;

  //     const reader = new FileReader();
  //     reader.onload = (e: any) => {
  //       // Convert file to base64 and store the result
  //       const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
  //       this.identity.visa = base64String;
  //     };
  //     reader.readAsDataURL(file);
  //   } else {
  //     this.selectedVisaFileName = '';
  //   }
  // }
  onVisaFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedVisaFileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        console.log('File Format:', fileFormat);
        const Concatenatefile = `${fileFormat}+${base64String}`;
        console.log('Whole File:', Concatenatefile);
        this.identity.visa = Concatenatefile;
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedVisaFileName = '';
    }
  }
  // onBlueCardFileSelected(event: any) {
  //   const file: File = event.target.files[0];
  //   if (file) {
  //     this.selectedBluecardFileName = file.name;

  //     const reader = new FileReader();
  //     reader.onload = (e: any) => {
  //       // Convert file to base64 and store the result
  //       const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
  //       this.identity.blueCard = base64String;
  //     };
  //     reader.readAsDataURL(file);
  //   } else {
  //     this.selectedBluecardFileName = '';
  //   }
  // }
  onBlueCardFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedBluecardFileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        console.log('File Format:', fileFormat);
        const Concatenatefile = `${fileFormat}+${base64String}`;
        console.log('Whole File:', Concatenatefile);
        this.identity.blueCard = Concatenatefile;
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedBluecardFileName = '';
    }
  }
  isEditMode: boolean = false;
  currentEditTempId: string | null = null;
  currentEditId: string | null = null;

  // intializeWorkPermit(): void {
  //   this.WorkPermit = this.formBuilder.group({
  //     permitType: new FormControl('', Validators.required),
  //     permitStartdate: new FormControl('', Validators.required),
  //     permitExpirytdate: new FormControl('', Validators.required),
  //     document: new FormControl('', Validators.required),
  //   });
  // }

  // onSubmit(): void {
  //   if (this.isEditMode) {
  //     this.updateWorkPermit(this.WorkPermit.value);
  //   } else {
  //     const formData = {
  //       tempId: uuidv4(),
  //       id: null,
  //       permitType: this.WorkPermit.value.permitType,
  //       permitStartdate: this.WorkPermit.value.permitStartdate,
  //       permitExpirytdate: this.WorkPermit.value.permitExpirytdate,
  //       document: this.WorkPermit.value.document
  //     };
  //     this.addWorkPermit(formData);
  //   }

  //   this.WorkPermit.reset();
  //   this.isEditMode = false;
  //   this.currentEditTempId = null;
  // }

  // addWorkPermit(formData: any): void {
  //   this.WorkPermitDetails.push(formData);
  //   this.updateDataRowSource();
  //   console.log("Other Work Permits:", this.WorkPermitDetails);
  //   console.log("FormData Added", formData);
  // }

  // updateDataRowSource(): void {
  //   debugger
  //   this.dataRowSource = this.WorkPermitDetails.map(otherpermit => {
  //     if (otherpermit.id === null) {
  //       const { id, ...rest } = otherpermit;
  //       debugger
  //       return rest;

  //     }
  //     debugger
  //     return otherpermit;
  //   });
  // }

  // onEditClick(currentRowData: any): void {
  //   this.isEditMode = true;
  //   this.currentEditTempId = currentRowData.tempId;

  //   debugger
  //   this.WorkPermit.patchValue({
  //     permitType: currentRowData.permitType,
  //     permitStartdate: currentRowData.permitStartdate,
  //     permitExpirytdate: currentRowData.permitExpirytdate,
  //     document: currentRowData.document,
  //   });
  // }

  // updateWorkPermit(updatedData: any): void {
  //   const index = this.WorkPermitDetails.findIndex(item => item.tempId === this.currentEditTempId);
  //   if (index !== -1) {
  //     this.WorkPermitDetails[index] = {
  //       ...this.WorkPermitDetails[index],
  //       ...updatedData,
  //     };
  //     this.updateDataRowSource();
  //     console.log('WorkPermit updated:', updatedData);
  //     this.toastr.success('WorkPermit updated successfully');
  //   } else {
  //     this.toastr.error('WorkPermit not found');
  //   }
  // }

  initializeWorkPermit(): void {
    this.WorkPermit = this.formBuilder.group({
      permitType: new FormControl('', Validators.required),
      permitStartdate: new FormControl('', Validators.required),
      permitExpirytdate: new FormControl('', Validators.required),
      document: new FormControl('', Validators.required),
    });
  }

  onSubmit(): void {
    this.spinner.show();
    if (this.isEditMode) {
      this.updateWorkPermit({
        ...this.WorkPermit.value,
        id: this.currentEditId,
        tempId: this.currentEditTempId,
      });
    } else {
      const formData = {
        tempId: uuidv4(),
        id: null,
        permitType: this.WorkPermit.value.permitType,
        permitStartdate: this.WorkPermit.value.permitStartdate,
        permitExpirytdate: this.WorkPermit.value.permitExpirytdate,
        document: this.WorkPermit.value.document,
      };
      this.addWorkPermit(formData);
    }
    this.spinner.hide();

    this.WorkPermit.reset();
    this.isEditMode = false;
    this.currentEditTempId = null;
    this.currentEditId = null;
  }

  addWorkPermit(formData: any): void {
    this.WorkPermitDetails.push(formData);
    this.updateDataRowSource();
    console.log('Other Work Permits:', this.WorkPermitDetails);
    console.log('FormData Added', formData);
  }

  updateDataRowSource(): void {
    this.dataRowSource = this.WorkPermitDetails.map((otherpermit) => {
      if (otherpermit.id === null) {
        const { id, ...rest } = otherpermit;
        return rest;
      }
      return otherpermit;
    });
  }

  onEditClick(currentRowData: any): void {
    this.isEditMode = true;

    if (currentRowData.id) {
      this.currentEditId = currentRowData.id;
      this.currentEditTempId = null;
    } else {
      this.currentEditTempId = currentRowData.tempId;
      this.currentEditId = null;
    }

    console.log(
      'Editing row with id:',
      this.currentEditId,
      'or tempId:',
      this.currentEditTempId
    );

    this.WorkPermit.patchValue({
      permitType: currentRowData.permitType,
      permitStartdate: currentRowData.permitStartdate,
      permitExpirytdate: currentRowData.permitExpirytdate,
      document: currentRowData.document,
      id: currentRowData.id || null,
      tempId: currentRowData.tempId || null,
    });
  }

  updateWorkPermit(updatedData: any): void {
    console.log('Updating work permit with data:', updatedData);
    console.log(
      'Current edit id:',
      this.currentEditId,
      'or tempId:',
      this.currentEditTempId
    );

    let index = -1;

    if (this.currentEditId) {
      index = this.WorkPermitDetails.findIndex(
        (item) => item.id === this.currentEditId
      );
    } else if (this.currentEditTempId) {
      index = this.WorkPermitDetails.findIndex(
        (item) => item.tempId === this.currentEditTempId
      );
    }

    if (index !== -1) {
      this.WorkPermitDetails[index] = {
        ...this.WorkPermitDetails[index],
        ...updatedData,
      };
      this.updateDataRowSource();
      console.log('WorkPermit updated:', updatedData);
      this.toastr.success('WorkPermit updated successfully');
    } else {
      this.toastr.error('WorkPermit not found');
    }
  }

  onDeleteClick(currentRowData: any): void {
    let index = -1;
    if (currentRowData.id) {
      index = this.WorkPermitDetails.findIndex(
        (item) => item.id === currentRowData.id
      );
    } else {
      index = this.WorkPermitDetails.findIndex(
        (item) => item.tempId === currentRowData.tempId
      );
    }

    if (index !== -1) {
      this.WorkPermitDetails.splice(index, 1);
      this.updateDataRowSource();
      console.log('WorkPermit deleted:', currentRowData);
      this.toastr.success('WorkPermit deleted successfully');
    } else {
      this.toastr.error('WorkPermit not found');
    }
  }

  getCurrentDate(): string {
    const currentDate = new Date();
    return currentDate.toISOString().substring(0, 10); // Format as YYYY-MM-DD
  }

  // onEditClick(currentRowData: any): void {
  //   debugger
  //   this.editId = currentRowData;
  //   this.userForm.setValue({
  //     index: this.editId.index,
  //     permitType: this.editId.permitType,
  //     permitStartdate: this.editId.permitStartdate,
  //     permitExpirytdate: this.editId.permitExpirytdate,
  //     document: this.editId.document
  //   });
  // }

  // onDeleteClick(currentRowData: any) {
  //   const index = this.WorkPermitDetails.findIndex(x => x.index === currentRowData.index);
  //   if (index !== -1) {
  //     this.WorkPermitDetails.splice(index, 1);
  //     this.dataRowSource = [...this.WorkPermitDetails];
  //     this.toastr.success('Work permit deleted successfully');
  //   }
  // }
}
