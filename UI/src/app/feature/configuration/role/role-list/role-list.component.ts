import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PermissionService } from '../../../../shared/services/permission.service';
import { first } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { ColDef } from 'ag-grid-community';
import { Role } from '../../../../shared/models/role';
import { roleservice } from '../../../../shared/services/role.service';
import { RequestWithFilterAndSort, filterModel, sortModel } from '../../../../shared/models/FilterRequset';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { GridApi } from 'ag-grid-community';
import { NgxSpinnerService } from 'ngx-spinner';
// import { ActionEnum } from '../../../../shared/constant/enum.const';
@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrl: './role-list.component.scss'
})
export class RoleListComponent implements OnInit {
  dataRowSource!: any[];
  showEdit: boolean = true;
  sortModel = {} as sortModel;
  showDelete: boolean = true;
  showDeleteModal: boolean = false;
  allRole: any;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  deleterole:any;
  searchText: any;
  editId: string = '';
  newFilterModel = {} as filterModel;
  totalPages: number[] = [];
  totalItems: any;
  private helper = new commonHelper();
  private gridApi!: GridApi;
  role = {} as Role;
  // deletePublicHoliday: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addEditModal') addEditModal!: any;

  columnDefs: ColDef[]=[]
  lang!: string;
  // columnDefs: ColDef[] = [

  //   { headerName: 'Role Name', field: 'name' , filterParams: { maxNumConditions: 1 }, },
  //   { headerName: 'Description', field: 'description',   filterParams: { maxNumConditions: 1 }, },
  // ];
  constructor(private translateService: TranslateService, private toastr: ToastrService, private roleService : roleservice,
    private spinner:NgxSpinnerService,
    private totalPageArray: totalPageArray,
    private permission: PermissionService,) {
      this.requestWithFilterAndPage.pageNumber = 1;
      this.requestWithFilterAndPage.pageSize = 5;
      this.sortModel.colId = "name";

      this.sortModel.sortOrder = "asc";
      this.requestWithFilterAndPage.sortModel = this.sortModel;
      this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.configuration.manageRoleDetails.rolename'), field: 'name' , filterParams: { maxNumConditions: 1 }, },
    { headerName: this.translateEnumValue('i18n.configuration.manageRoleDetails.description'), field: 'description',   filterParams: { maxNumConditions: 1 }, },
      ];
    }, 50);
    this.getRole();
  }

  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }

  getRole() {

    this.roleService
      .GetFilterRole(
        this.requestWithFilterAndPage
      )
      .pipe(first())
      .subscribe((resp) => {
        this.totalItems = resp.httpResponse.totalRecord;
        this.dataRowSource = resp.httpResponse.roles;
        this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
      });
  }

  getAllRole() {
    this.roleService
      .getRoleList()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          this.allRole = resp.httpResponse;
          // this.dataRowSource = this.allRole
          // console.warn("data", this.dataRowSource);

        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.roleService
      .getRoleById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.allRole = resp;
        this.role = this.allRole.httpResponse;
        this.getRole();
        /// // debugger
        if (this.role) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
  }

  onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.deleterole = currentRowData.name;
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  validateModalData(): boolean {
    if (!this.role.name) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageRoleDetails.Rolerequired",(this.spinner.hide())));
      return false;
    }
    if (!this.role.description) {
      this.toastr.error(this.translateService.instant("i18n.configuration.manageRoleDetails.Descriptionrequired",(this.spinner.hide())));
      return false;
    }
    return true;
  }
  closeModal() {

    if (this.closeButton) {
      this.closeButton.nativeElement.click();
      this.role = {} as Role;
    }
  }
  saveRole() {
    this.spinner.show();
    if (!this.role.id) {
      this.role.id = "";
      this.role.action = ActionEnum.Insert;
    } else {
      this.role.action = ActionEnum.Update;
    }

    if (this.validateModalData()) {
      this.roleService
        .saveRole(this.role)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getRole();
            this.closeModal();
            
            this.role= {} as Role;
            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error(err);
            this.spinner.hide();
          },
        });
    }
  }
  
  deleteItem() {
    this.spinner.show();
    this.roleService
      .deleterole(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          if (resp.message.includes("cannot be deleted because it is assigned to one or more employees")) {
            this.toastr.warning(resp.message);
          } else {
            this.toastr.success(resp.message);
          }
          this.showDeleteModal = false;
          this.getAllRole();
          this.getRole();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error(err);
          this.spinner.hide();
        },
      });
  }

  //****************** For Grid Filter ****************//

  gridReady(params: any) {
    this.gridApi = params.api;
  }


  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['name','description']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.getRole();

  }

  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length==0 ||inputValue.length >= 4) {
      this.commonSearchWithinGrid();
    }
  }
  getDataRowsourse(event: RequestWithFilterAndSort) {
    
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

    this.getRole();
  }
}
