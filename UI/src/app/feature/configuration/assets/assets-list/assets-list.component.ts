import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable, first } from 'rxjs';
import { AssetService } from '../../../../shared/services/asset.service';
import { EmployeeService } from '../../../../shared/services/employee.service';
import { AssetStatusService } from '../../../../shared/services/assetsstatus.service';
import { AssettypeService } from '../../../../shared/services/assetstype.service';
import { ActionEnum, StatusEnum } from '../../../../shared/constant/enum.const';
import { ManageAssets, SaveAssets } from '../../../../shared/models/assets';
import { employee } from '../../../../shared/models/employee';
import { Asset_Status } from '../../../../shared/models/assets_status';
import { Asset_type } from '../../../../shared/models/assets_type';
import { RequestWithFilterAndSort,filterModel,sortModel } from '../../../../shared/models/FilterRequset';
import { GridApi } from 'ag-grid-community';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-assets-list',
  templateUrl: './assets-list.component.html',
  styleUrls: ['./assets-list.component.scss'],
})
export class AssetsListComponent implements OnInit {
  PageSize!: number;
  PageNumber!: number;
  dataRowSource!: any[];
  totalPages: number[] = [];
  totalItems: any;
  columnDefs: any[] = [];
  employees: employee[] = [];
  assetsStatus: Asset_Status[] = [];
  assetTypes: Asset_type[] = [];
  assets = {} as SaveAssets;
  assetsdata: any;
  sortModel = {} as sortModel;
  newFilterModel = {} as filterModel;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  private helper = new commonHelper();
  private gridApi!: GridApi;
  searchText: any;
  editId: any;
  downloadId: any;
  showEdit: boolean = true;
  showDelete: boolean = true;
  showDownload: boolean = true;
  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('addDownloadModel') addDownloadModel!: any;
  @ViewChild('closeButton') closeButton!: ElementRef;
  showDeleteModal: boolean = false;
  DeleteAsset: any;
  employeeResponse: any;
  selectedFileName: string = '';
  selectedCurrentOwner: string = '';
  selectedPreviousOwner: string = '';
  lang!: string;

  constructor(
    private router: Router,
    private toastr: ToastrService,
    private assetService: AssetService,
    private employeeService: EmployeeService,
    private assetsstatusService: AssetStatusService,
    private permission: PermissionService,
    private assetstypeService: AssettypeService,
    private translateService: TranslateService,
    private totalPageArray: totalPageArray,
    private spinner: NgxSpinnerService
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = 'Manufacturer';
    this.sortModel.sortOrder = 'asc';
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.serialnumber'
          ),
          field: 'serialNumber',
          sortable: false,
          filter: true,
          filterParams: { maxNumConditions: 1 },
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.status'
          ),
          field: 'statusName',
          sortable: false,
          filter: true,
          filterParams: { maxNumConditions: 1 },
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.manufacturer'
          ),
          field: 'manufacturer',
          sortable: false,
          filter: true,
          filterParams: { maxNumConditions: 1 },
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.assetstype'
          ),
          field: 'assetTypeName',
          sortable: false,
          filter: true,
          filterParams: { maxNumConditions: 1 },
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.model'
          ),
          field: 'model',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.moreDetails'
          ),
          field: 'moreDetails',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.purchaseDate'
          ),
          field: 'purchaseDate',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.warrantyDueDate'
          ),
          field: 'warrantyDueDate',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.selectCurrentOwner'
          ),
          field: 'currentOwnerFullName',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.selectPreviousOwner'
          ),
          field: 'previousOwnerFullName',
          sortable: false,
          filter: true,
        },
        {
          headerName: this.translateEnumValue(
            'i18n.configuration.manageAssetDetails.note'
          ),
          field: 'note',
          sortable: false,
          filter: true,
        },
      ];
    }, 50);
    this.getFilterAssets();
    this.getEmployees();
    this.getAssetsStatus();
    this.getAssetsType();
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);
  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }

  getAssets(): void {
    this.assetService
      .getAssets()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.assetsdata = resp;
        },
        error: (err: any) => {
          this.toastr.error('Assets', err.message);
        },
      });
  }

  getFilterAssets() {
    this.assetService
      .GetFilterAssets(this.requestWithFilterAndPage)
      .pipe(first())
      .subscribe((resp) => {
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.assets;
        console.log('Assets:', this.dataRowSource);

        this.totalPages = this.totalPageArray.GetTotalPageArray(
          this.totalItems,
          this.requestWithFilterAndPage.pageSize
        );
      });
  }

  getEmployees() {
    this.employeeService.getEmployee().subscribe({
      next: (data: any) => {
        this.employees = data.httpResponse;
      },
      error: (error: any) => {
        // console.error("Error fetching employees:", error);
      },
    });
  }
  getAssetsStatus() {
    this.assetsstatusService.getAssetsStatus().subscribe({
      next: (data: any) => {
        this.assetsStatus = data.httpResponse.map((status: any) => {
          const assetStatus: {
            id: string;
            status: string;
            translationKey: string;
          } = { ...status };
          assetStatus.translationKey = status.status.toLowerCase();
          return assetStatus;
        });
      },
      error: (error: any) => {
        // Handle error
      },
    });
  }

  getAssetsType() {
    this.assetstypeService.getAssetstype().subscribe({
      next: (data: any) => {
        this.assetTypes = data.httpResponse.map((type: any) => {
          const assetType = type as Asset_type; // Type assertion
          assetType.translationKey =
            'i18n.configuration.manageAssetDetails.assetstypeDetails.' +
            type.assetTypes.toLowerCase();
          return assetType;
        });
      },
      error: (error: any) => {
        // Handle error7
      },
    });
  }

  onSelect(event: any) {
    const selectedValue = event.target.value;
    if (selectedValue === 'null') {
      this.assets.currentOwner = null;
    } else {
      // Store selected current owner ID
      this.assets.currentOwner = selectedValue;
    }
  }

  onSelect2(event: any) {
    const selectedValue = event.target.value;
    if (selectedValue === 'null') {
      this.assets.previousOwner = null;
    } else {
      // Store selected previous owner ID
      this.assets.previousOwner = selectedValue;
    }
  }

  getCurrentDate(): string {
    const currentDate = new Date();
    return currentDate.toISOString().substring(0, 10); // Format as YYYY-MM-DD
  }

  onStatusChange(event: Event): void {
    const selectedValue = (event.target as HTMLSelectElement).value;
  
    const retireId = StatusEnum.Retired;
    const repairId = StatusEnum.InRepair;
    const availableId = StatusEnum.Available;
  
    if (selectedValue === retireId || selectedValue === repairId || selectedValue === availableId) {
      this.assets.previousOwner = this.assets.currentOwner;
      this.assets.currentOwner = null;
    }
  }

  addAsset(): void {
    this.spinner.show();
    if (!this.assets.id) {
      this.assets.action = ActionEnum.Insert;
    } else {
      this.assets.action = ActionEnum.Update;
    }
    // const selectedValue = this.assets.status;
  
    // const retireId = 'f32e061c-5ca0-4449-95e4-6a5e7809b1ba';
    // const repairId = 'd1f24060-4667-4512-b752-c273f0fc6ec4';
    // const availableId = 'b0b9c254-3760-4089-8a6f-c6353c808aa8';
  
    // if (selectedValue === retireId || selectedValue === repairId || selectedValue === availableId) {
    //   this.assets.previousOwner = this.assets.currentOwner;
    //   this.assets.currentOwner = null;
    // }
    this.assets.statusName = '';
    this.assets.assetTypeName = '';
    this.assets.currentOwnerFullName = '';
    this.assets.previousOwnerFullName = '';
    // console.log('Assets data:', this.assets);

    if (this.ValidateModalData()) {
      this.assetService
        .SaveAssets(this.assets)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.resetFormFields();
            this.toastr.success(resp.message);
            this.getEmployees();
            this.getAssets();
            this.getFilterAssets();
            this.closeModal();

            this.assets = {} as SaveAssets;
            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error(err);
            this.spinner.hide();
          },
        });
    }
  }

  resetFormFields(): void {
    // Reset form fields

    this.assets = {} as SaveAssets;
    // Clear file name
    this.selectedFileName = '';
    // Clear current owner and previous owner
    this.selectedCurrentOwner = '';
    this.selectedPreviousOwner = '';
  }
  deleteItem() {
    this.assetService
      .DeleteAssets(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          if (resp.message.includes("Asset cannot be deleted ")) {
            this.toastr.warning(resp.message);
          } else {
            this.toastr.success(resp.message);
          }
          this.showDeleteModal = false;
          this.getAssets();
          this.getFilterAssets();
        },
        error: (err: any) => {
          this.toastr.error(err.message);
        },
      });
}

  closeModal() {
    this.assets = {} as SaveAssets;
    if (this.closeButton) {
      this.closeButton.nativeElement.click();
    }
  }
  cancelDelete() {
    this.showDeleteModal = false;
  }
  onAddClick() {
    this.router.navigate([`/addAsset`]);
  }

  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.assetService
      .getAssetsById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.employeeResponse = resp;
        this.assets = this.employeeResponse.httpResponse;
        this.getAssets();
        this.getFilterAssets();
        if (this.assets) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }
  onDownloadClick(currentRowData: any): void {
    this.spinner.show();
    this.downloadId = currentRowData.id;

    this.assetService
      .getAssetsById(this.downloadId)
      .pipe(first())
      .subscribe((resp) => {
        const FileData = resp.httpResponse.warranty;
        const indexOfPlus = FileData.indexOf('+');

        if (indexOfPlus !== -1) {
          const fileFormat = FileData.slice(0, indexOfPlus); // Extract before the first '+'
          const base64String = FileData.slice(indexOfPlus + 1); // Extract after the first '+'

          // Create a blob from the base64 string
          const blob = this.base64toBlob(
            base64String,
            `application/${fileFormat}`
          );
          const blobUrl = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = blobUrl;
          link.download = `document.${fileFormat}`; // Use the extracted format

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

  onDeleteClick(currentRowData: any) {
    this.spinner.show();
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteAsset = currentRowData.serialNumber;
    this.spinner.hide(); // You can change this to any property you want to display
  }

  onPageSizeChange(pageSize: number) {
    this.PageSize = pageSize;
  }

  onPageNumberChange(pageNumber: number) {
    this.PageNumber = pageNumber;
  }

  ValidateModalData(): boolean {
    if (!this.assets.serialNumber) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.configuration.manageAssetDetails.SerialNumberrequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.assets.status) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.configuration.manageAssetDetails.Statusrequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.assets.manufacturer) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.configuration.manageAssetDetails.Manufacturerrequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.assets.assetType) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.configuration.manageAssetDetails.AssetTyperequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.assets.model) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.configuration.manageAssetDetails.Modelrequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.assets.purchaseDate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.configuration.manageAssetDetails.PurchaseDaterequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.assets.warrantyDueDate) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.configuration.manageAssetDetails.WarrantyDueDaterequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    // if (!this.assets.currentOwner) {
    //   this.toastr.error('Current Owner required');
    //   return false;
    // }
    // if (!this.assets.previousOwner) {
    //   this.toastr.error('Previous Owner required');
    //   return false;
    // }
    if (!this.assets.note) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.configuration.manageAssetDetails.Noterequired',
          this.spinner.hide()
        )
      );
      return false;
      this.spinner.hide();
    }
    return true;
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        const fileFormat: string = this.getFileExtension(file.name);
        // console.log("File Format:",fileFormat);
        const Concatenatefile = `${fileFormat}+${base64String}`;
        // console.log("Whole File:",Concatenatefile);
        this.assets.warranty = Concatenatefile;
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedFileName = '';
    }
  }
  getFileExtension(filename: string): string {
    return filename.split('.').pop()?.toLowerCase() || ''; // Get the lowercase file extension
  }
  // this.assets.warranty
  getDataRowsourse(event: RequestWithFilterAndSort) {
    // //debugger
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

    this.getFilterAssets();
  }

  gridReady(params: any) {
    this.gridApi = params.api;
  }
  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    const gridColumnsToFileter: string[] = [
      'serialNumber',
      'statusname.status',
      'manufacturer',
      'assetTypename.assetTypes',
      'model',
      'moreDetails',
      'currentOwnerEmployee.firstName',
      'previousOwnerEmployee.firstName',
      'note',
    ];
    this.helper.commonSearchWithinGrid(
      gridColumnsToFileter,
      this.searchText,
      this.requestWithFilterAndPage
    );

    this.getFilterAssets();
  }

  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length == 0 || inputValue.length >= 4) {
      this.commonSearchWithinGrid();
    }
  }

  ChangeLang(lang: any) {
    const selectedLanguage = lang.target.value;
    localStorage.setItem('lang', selectedLanguage);
    this.translateService.use(selectedLanguage);
  }
}
