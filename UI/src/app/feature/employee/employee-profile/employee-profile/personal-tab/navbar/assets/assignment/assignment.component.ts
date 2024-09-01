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
  selector: 'app-assignment',
  templateUrl: './assignment.component.html',
  styleUrls: ['./assignment.component.scss']
})
export class AssignmentComponent implements OnInit {

  PageSize!: number;
  PageNumber!: number;
  dataRowSource!: any[];
  totalPages: number[] = [];
  totalItems: any;
  columnDefs:any[]=[];
  // columnDefs = [
  //   { headerName: 'Serial Number', field: 'serialNumber', sortable: false, filter: true },
  //   { headerName: 'Manufacturer', field: 'manufacturer', sortable: false, filter: true },
  //   { headerName: 'Asset Type', field: 'assetTypeName', sortable: false, filter: true },
  //   { headerName: 'Model', field: 'model', sortable: false, filter: true },
  //   { headerName: 'Previous Owner', valueGetter: this.getPreviousOwnerFullName, sortable: false, filter: true },
  //   { headerName: 'Select', field: 'select', checkboxSelection: true, headerCheckboxSelection: true }
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
  selectedAssets: any[] = [];
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
    private EmployeFlage: EmployeeFlagService,
    private totalPageArray: totalPageArray
  ) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "Manufacturer";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
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
        { headerName: this.translateEnumValue('i18n.configuration.manageAssetDetails.selectPreviousOwner'),field:'previousOwnerFullName', sortable: false, filter: true },
        { headerName: 'Select', field: 'select', checkboxSelection: true, headerCheckboxSelection: true },
      ];
    }, 50);
    this.getFilterAssets();
    this.getEmployees();
    this.getAssetsStatus();
    this.getAssets()
    this.getAssetsType();

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
        this.dataRowSource = resp.httpResponse.assets.filter((asset: any) => asset.status === StatusEnum.Available); //Update the id of Available in enum.const.ts
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }

  getEmployees() {
    this.employeeService.getEmployee().subscribe({
      next: (data: any) => {
        this.employees = data.httpResponse;
      },
      error: (error: any) => {
        console.error("Error fetching employees:", error.message);
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
  addAsset(asset: any): void {
    this.assetService
      .SaveAssets(asset)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          // Update the grid data if needed
          this.getFilterAssets();
          this.getAssets()
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }

  saveSelectedAssets(): void {
    const selectedRows = this.gridApi.getSelectedRows();
    if (selectedRows.length > 0) {
      selectedRows.forEach((row: any) => {
        const updatedAsset = {
          ...row,
          status: StatusEnum.InUse,
          currentOwner: this.SelectedEmployee,
        };
        this.addAsset(updatedAsset);
        this.getFilterAssets();
        this.getAssets()
      });
      this.closeModal();
    } else {
      console.log('No rows selected.');
      this.getFilterAssets();
        this.getAssets()
    }
  }

  closeModal() {
    this.assets = {} as SaveAssets;
    if (this.closeButton) {
      this.closeButton.nativeElement.click();
    }
  }

  onCheckboxClick(params: any): void {

    const currentRowData = params.data;


    currentRowData.status = StatusEnum.InUse; // Update the id of InUse from enum.const.ts
    currentRowData.currentOwner = this.SelectedEmployee;


    if (params.node.isSelected()) {

      this.selectedAssets.push(currentRowData);
    } else {

      const index = this.selectedAssets.findIndex(asset => asset.id === currentRowData.id);
      if (index !== -1) {
        this.selectedAssets.splice(index, 1);
      }
    }

    console.log("Checkbox clicked");
  }

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteAsset = currentRowData.assetType;
  }

  onPageSizeChange(pageSize: number) {
    this.PageSize = pageSize;
  }

  onPageNumberChange(pageNumber: number) {
    this.PageNumber = pageNumber;
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
