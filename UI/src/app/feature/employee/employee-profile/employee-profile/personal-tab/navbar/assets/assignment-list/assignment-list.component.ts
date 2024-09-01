import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable, first } from 'rxjs';
import { AssetService } from '../../../../../../../../shared/services/asset.service';
import { EmployeeService } from '../../../../../../../../shared/services/employee.service';
import { AssetStatusService } from '../../../../../../../../shared/services/assetsstatus.service';
import { AssettypeService } from '../../../../../../../../shared/services/assetstype.service';
import { ActionEnum, DefaultEmployee } from '../../../../../../../../shared/constant/enum.const';
import { StatusEnum } from '../../../../../../../../shared/constant/enum.const';
import { SaveAssets } from '../../../../../../../../shared/models/assets';
import { employee } from '../../../../../../../../shared/models/employee';
import { Asset_Status } from '../../../../../../../../shared/models/assets_status';
import { Asset_type } from '../../../../../../../../shared/models/assets_type';
import { RequestWithFilterAndSort, filterModel, sortModel } from '../../../../../../../../shared/models/FilterRequset';
import { GridApi } from 'ag-grid-community';
import { totalPageArray } from '../../../../../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../../../../../shared/helpers/common.helpers';
import { EmployeeFlagService } from '../../../../../../../../shared/services/EmployeFlag.service';


@Component({
  selector: 'app-assignment-list',
  templateUrl: './assignment-list.component.html',
  styleUrls: ['./assignment-list.component.scss']
})
export class AssignmentListComponent implements OnInit {

  PageSize!: number;
  PageNumber!: number;
  dataRowSource!: any[];
  totalPages: number[] = [];
  totalItems: any;
  columnDefs:any[]=[];
  // columnDefs = [
  //   { headerName: 'Serial Number', field: 'serialNumber', sortable: false, filter: true },
  //   //{ headerName: 'Status', field: 'statusname.status', sortable: false, filter: true },
  //   { headerName: 'Manufacturer', field: 'manufacturer', sortable: false, filter: true },
  //   { headerName: 'Asset Type', field: 'assetTypeName', sortable: false, filter: true },
  //   { headerName: 'Model', field: 'model', sortable: false, filter: true },
  //   { headerName: 'Previous Owner', valueGetter: this.getPreviousOwnerFullName, sortable: false, filter: true },
  // ];
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
  showEdit: boolean = true;
  showDelete: boolean = false;
  showCheckbox: boolean = true;
  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  showDeleteModal: boolean = false;
  DeleteAsset: any;
  employeeResponse: any;
  selectedFileName: string = '';
  selectedCurrentOwner: string = '';
  selectedPreviousOwner: string = '';
  lang!: string;
  SelectedEmployee: any;

  constructor(
    private router: Router,
    private toastr: ToastrService,
    private assetService: AssetService,
    private employeeService: EmployeeService,
    private assetsstatusService: AssetStatusService,
    private assetstypeService: AssettypeService,
    private translateService: TranslateService,
    private totalPageArray: totalPageArray,
    private EmployeFlage: EmployeeFlagService,
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "Manufacturer";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};

  }

  ngOnInit(): void {
    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
      console.warn(employee);   
        this.SelectedEmployee = employee;
        this.getFilterAssets();
    });
    this.lang = localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.configuration.manageAssetDetails.serialnumber'), field: 'serialNumber', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.configuration.manageAssetDetails.manufacturer'), field: 'manufacturer', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.configuration.manageAssetDetails.assetstype'), field: 'assetTypeName', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.configuration.manageAssetDetails.model'), field: 'model', sortable: false, filter: true },
        { headerName: this.translateEnumValue('i18n.configuration.manageAssetDetails.selectPreviousOwner'), field:'previousOwnerFullName', sortable: false, filter: true },
        // { headerName: 'Select', field: 'select', checkboxSelection: true, headerCheckboxSelection: true },
      ];
    }, 50);
    this.getFilterAssets();
    this.getEmployees();
    this.getAssetsStatus();
    this.getAssetsType();
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  getPreviousOwnerFullName(params: any): string {
    const { previousOwnerFirstName, previousOwnerMiddleName, previousOwnerLastName } = params?.data;
  
    if (!previousOwnerFirstName && !previousOwnerMiddleName && !previousOwnerLastName) {
      return ''; 
    }
  
    const firstName = previousOwnerFirstName || '';
    const middleName = previousOwnerMiddleName || '';
    const lastName = previousOwnerLastName || '';
  
    return `${firstName} ${middleName} ${lastName}`;
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
          this.toastr.error("Assets", err.message);
        },
      });
  }

  getFilterAssets() {

    this.assetService
      .GetFilterAssets(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {

        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.assets.filter((asset: any) => asset.status === StatusEnum.InUse).filter((asset: any) => asset.currentOwner === this.SelectedEmployee);
        ;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }

  getEmployees() {
    this.employeeService.getEmployee().subscribe({
      next: (data: any) => {
        this.employees = data.httpResponse;
      },
      error: (error: any) => {
        console.error("Error fetching employees:", error);
      }
    });
  }
  getAssetsStatus() {
    this.assetsstatusService.getAssetsStatus().subscribe({
      next: (data: any) => {
        this.assetsStatus = data.httpResponse;
      },
      error: (error: any) => {
        console.error("Error fetching Status:", error);
      }
    });
  }
  getAssetsType() {
    this.assetstypeService.getAssetstype().subscribe({
      next: (data: any) => {

        this.assetTypes = data.httpResponse;
      },
      error: (error: any) => {
        console.error("Error fetching Type:", error);
      }
    });
  }

  onSelect(value: any) {
    // Store selected current owner ID
    this.assets.currentOwner = value.target.value;
  }

  onSelect2(value: any) {
    // Store selected previous owner ID
    this.assets.previousOwner = value.target.value;
  }
  addAsset(currentRowData: any): void {
      this.assets.action = ActionEnum.Update;
      this.editId = currentRowData.id;
      this.assetService.SaveAssets(this.assets)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getEmployees();
            this.getAssets();
            this.closeModal();
            this.assets = {} as SaveAssets;
            this.assets.status = StatusEnum.InUse;
            this.assets.currentOwner =localStorage.getItem('SelectedEmployeeForEdit')?.toLowerCase().replace(/"/g, "") || '';
          },
          error: (err: any) => {
            this.toastr.error(err);
          },
        });
    
  }
  closeModal() {
    this.assets = {} as SaveAssets;
    if (this.closeButton) {
      this.closeButton.nativeElement.click();
    }
  }

  onCheckboxClick(params: any): void {
    const currentRowData = params.data;

    // Update status and current owner
    currentRowData.status = StatusEnum.InUse;
    currentRowData.currentOwner =localStorage.getItem('SelectedEmployeeForEdit')?.toLowerCase().replace(/"/g, "") || '';

    this.addAsset(currentRowData);
  }

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteAsset = currentRowData.assetType; // You can change this to any property you want to display
  }

  onPageSizeChange(pageSize: number) {
    this.PageSize = pageSize;
  }

  onPageNumberChange(pageNumber: number) {
    this.PageNumber = pageNumber;
  }

  ValidateModalData(): boolean {
    if (!this.assets.serialNumber) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.SerialNumberrequired"));
      return false;
    }
    if (!this.assets.status) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.Statusrequired"));
      return false;
    }
    if (!this.assets.manufacturer) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.Manufacturerrequired"));
      return false;
    }
    if (!this.assets.assetType) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.AssetTyperequired"));
      return false;
    }
    if (!this.assets.model) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.Modelrequired"));
      return false;
    }
    if (!this.assets.purchaseDate) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.PurchaseDaterequired"));
      return false;
    }
    if (!this.assets.warrantyDueDate) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.WarrantyDueDaterequired"));
      return false;
    }
    if (!this.assets.currentOwner) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.CurrentOwnerrequired"));
      return false;
    }
    if (!this.assets.previousOwner) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.PreviousOwnerrequired"));
      return false;
    }
    if (!this.assets.note) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageAssetDetails.Noterequired"));
      return false;
    }
    return true;
  }


  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        // Convert file to base64 and store the result
        const base64String: string = e.target.result.split(',')[1]; // Split off base64 part
        this.assets.warranty = base64String;
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedFileName = '';
    }
  }

  getDataRowsourse(event: RequestWithFilterAndSort) {
    // //debugger
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

    this.getFilterAssets();
  }

  gridReady(params: any) {
    this.gridApi = params.api;
  }
  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['serialNumber', 'statusname.status', 'manufacturer', 'assetTypename.assetTypes', 'model', 'moreDetails', 'currentOwnerEmployee.firstName', 'previousOwnerEmployee.firstName', 'note']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getFilterAssets();

  }

}