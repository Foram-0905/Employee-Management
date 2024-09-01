import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Observable, first } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { GridApi } from 'ag-grid-community';
import { RequestWithFilterAndSort, filterModel, sortModel } from '../../../../../../../../shared/models/FilterRequset';
import { Router } from '@angular/router';
import { DocumentList } from '../../../../../../../../shared/models/documentlist';
import { DocumentListService } from '../../../../../../../../shared/services/documentlist.service';
import { totalPageArray } from '../../../../../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../../../../../shared/helpers/common.helpers';
import { ActionEnum } from '../../../../../../../../shared/constant/enum.const';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PermissionService } from '../../../../../../../../shared/services/permission.service';
import { ToastrService } from 'ngx-toastr';
import { Document } from '../../../../../../../../shared/models/document';
import { DocumentService } from '../../../../../../../../shared/services/document.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFlagService } from '../../../../../../../../shared/services/EmployeFlag.service';
@Component({
  selector: 'app-documents-list',
  templateUrl: './documents-list.component.html',
  styleUrl: './documents-list.component.scss'
})
export class DocumentsListComponent implements OnInit {
  showEdit: boolean = true;
  DeleteDocumentList: any;
  showDelete: boolean = true;
  showDownload:boolean=true;
  searchText: any;
  slgGroup: any;
  documentResponse: any;
  
  filter: any;
  PageSize!: number
  PageNumber!: number;
  dataRowSource!: any[];
  totalPages: number[] = [];
  totalItems: any
  editId: any;
  downloadId:any;
  documentlistdata: any;
  documentlist = {} as DocumentList;
  selectedFileName: string = '';
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  private gridApi!: GridApi;
  selectedConfidentialityAgreement: string = '';
  selectedOther: string = '';
  SelectedEmployee!: string;

  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('addDownloadModel') addDownloadModel!:any
  @ViewChild('closeButton') closeButton!: ElementRef;
  showDeleteModal: boolean = false;
  DeleteDesignation: any;
  pageSize !: number;
  pageNumber!: number;
  private helper = new commonHelper();
  Document = {} as Document;
  newFilterModel = {} as filterModel;
  sortModel = {} as sortModel;
  columnDefs = [

    { headerName: 'TabName', field: 'tabName', sortable: false, filter: true, },
    { headerName: 'Modulename', field: 'modulename', sortable: false, filter: true },
    { headerName: 'FileName', field: 'fileName', sortable: false, filter: true, },
    // { headerName: 'Documents', field: 'documents', sortable: false, filter: true, cellRenderer: this.fileDownloadLinkRenderer.bind(this) }

  ];
  downloadfilename: any;
  constructor(

    private translateService: TranslateService,
    private permission: PermissionService,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private documentlistService: DocumentListService,
    private totalPageArray: totalPageArray,
    private documentService: DocumentService,
    private spinner:NgxSpinnerService,
    private EmployeFlage: EmployeeFlagService,

  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "TabName";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
   

  }

  ngOnInit() {
    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
        this.SelectedEmployee = employee;
    });
    // this.getFilterDocumentList();
    this.GetDocumentListByEmployeeId();
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  SaveDocument() {
    this.spinner.show();
    if (!this.Document.id) {
      this.Document.action = ActionEnum.Insert;
      this.Document.employeeId = this.SelectedEmployee;

    } else {
      this.Document.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.documentService
        .saveDocument(this.Document)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.Document = {} as Document;
            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error("Save Document", err.message);
            this.spinner.hide();
          },
        });
    }
  }
  ValidateModalData(): boolean {
    if (!this.Document.personalDataProtection) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.employeeProfile.personal-details.documents-details.PersonalDataProtectionrequired"));
      return false;
    }
    if (!this.Document.confidentialityAgreement) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.employeeProfile.personal-details.documents-details.ConfidentialityAgreementrequired"));
      return false;
    }
    if (!this.Document.other) {
      this.toastr.error(this.translateService.instant("i18n.employeeProfile.personal-details.employeeProfile.personal-details.documents-details.Otherrequired"));
      return false;
    }

    return true;
  }



  onPersonalDataProtectionFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Document.personalDataProtection = Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedFileName = '';
    }
  }
  
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }



  onFileConfidentialityAgreementSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedConfidentialityAgreement = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Document.confidentialityAgreement = Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedConfidentialityAgreement = '';
    }
  }


  downloadDocument(event:any){
    // console.log("document downloaad",event);
    
  }


  // getDocumentList(): void {
  //   this.documentlistService
  //     .getDocumentlist()
  //     .pipe(first())
  //     .subscribe({
  //       next: (resp: any) => {
  //         this.documentlistdata = resp;
  //         // console.log("document", this.documentlistdata);
  //         // console.log("DOC");

  //       },
  //       error: (err: any) => {
  //         this.toastr.error("document", err.message);
  //       },
  //     });
  // }
  GetDocumentListByEmployeeId(): void {
    this.documentlistService
      .GetDocumentbyEmployeeId(this.SelectedEmployee)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.documentlist = resp;
          // console.log("document", this.documentlistdata);
          this.dataRowSource = resp.httpResponse
          // console.log("FilterDOC", this.dataRowSource);

        },
        error: (err: any) => {
          this.toastr.error("document", err.message);
        },
      });
  }
  getFilterDocumentList() {

    this.documentlistService
      .getFilterDocumentlist(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {

        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.documentlists
        // console.log("FilterDOC", this.dataRowSource);

        ;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }

  onFileOther(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedOther = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile= `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.Document.other= Concatenatefile; 
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedOther = '';
    }
  }
  getFileExtension(filename: string): string {
    return filename.split('.').pop()?.toLowerCase() || ''; // Get the lowercase file extension
  }
  // onEditClick(currentRowData: any): void {
  //   this.editId = currentRowData.id;
  //   this.documentlistService
  //     .getDocumemtlistById(this.editId)
  //     .pipe(first())
  //     .subscribe((resp) => {
  //       this.documentResponse = resp;
  //       this.documentlist = this.documentResponse.httpResponse;
  //       this.getdocumentlist();
  //       this.getFilterdocumentlistById();
  //       /// // debugger
  //       if (this.documentlist) {
  //         if (this.addBtn) {
  //           this.addBtn.nativeElement.click();
  //         }
  //       }
  //     });
  // }


  onDownloadClick(currentRowData: any): void {
    this.spinner.show();
    this.downloadId = currentRowData.id;
    this.downloadfilename=currentRowData.fileName;
  
    this.documentlistService.GetDocumentListByEntityId(this.downloadId,this.downloadfilename)
      .pipe(first())
      .subscribe((resp) => {
        
          const documentData =  resp.httpResponse[0]?.documents;
        // console.log("DATA AYA",documentData);
        
        
        const indexOfPlus = documentData.indexOf('+');


          if (indexOfPlus !== -1) {
              const fileFormat = documentData.slice(0, indexOfPlus); // Extract before the first '+'
              const base64String = documentData.slice(indexOfPlus + 1); // Extract after the first '+'

            
                  // Create a blob from the base64 string
                  const blob = this.base64toBlob(base64String, `application/${fileFormat}`);
                  const blobUrl = window.URL.createObjectURL(blob);
                  const link = document.createElement('a');
                  link.href = blobUrl;
                  link.download = `document.${fileFormat}`; // Use the extracted format
            
                  document.body.appendChild(link);
                  link.click();
                  document.body.removeChild(link);
                  window.URL.revokeObjectURL(blobUrl);
                  this.spinner.hide();
          }
          else {
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

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteDocumentList = currentRowData.name;
  }
  deleteItem() {
    this.documentlistService
      .deleteDocumentlist(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.showDeleteModal = false;
          this.getFilterDocumentList();
          // this.getDocumentList();
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  cancelDelete() {
    this.showDeleteModal = false;
  }
  onPageSizeChange(pageSize: number) {
    this.PageSize = pageSize;
  }

  onPageNumberChange(pageNumber: number) {
    this.pageNumber = pageNumber;
  }
  onAddClick() {
    this.router.navigate([`/addDocumentList`]);
  }
  closeModal() {
    this.documentlist = {} as DocumentList;
    if (this.closeButton) {
      this.closeButton.nativeElement.click();
    } 
  }
  //****************** For Grid Filter ****************//

  gridReady(params: any) {
    this.gridApi = params.api;
  }

  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length == 0 || inputValue.length >= 4) {
    }
  }
  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
     const gridColumnsToFileter: string[] = ['serialNumber', 'statusname.status', 'manufacturer', 'assetTypename.assetTypes', 'model', 'moreDetails', 'currentOwnerEmployee.firstName', 'previousOwnerEmployee.firstName', 'note']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getFilterDocumentList();

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
    }
    this.requestWithFilterAndPage = event;

    this.getFilterDocumentList();
  }
}