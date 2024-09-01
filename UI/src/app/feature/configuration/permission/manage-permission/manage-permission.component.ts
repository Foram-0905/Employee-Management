import { permission } from './../../../../shared/models/permission';
import { PermissionService } from './../../../../shared/services/permission.service';
import { permissionGuard } from './../../../../core/guards/permission.guard';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Action } from 'rxjs/internal/scheduler/Action';
import { first } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

import { HttpClient } from '@angular/common/http';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-manage-permission',
  templateUrl: './manage-permission.component.html',
  styleUrl: './manage-permission.component.scss',
})
export class ManagePermissionComponent implements OnInit {
  // permissions = [
  //   {
  //     model: 'Configuration',
  //     subModel: 'SLG Group',
  //     action: [{ Label: 'Add', isSelectd: false, permission: 'configuration.slggroup.Add' }, { Label: 'Edit', isSelectd: false, permission: 'configuration.slggroup.Edit' }, { Label: 'Delete', isSelectd: false, permission: 'configuration.slggroup.Delete' }, { Label: 'View', isSelectd: false, permission: 'configuration.slggroup.View' }]
  //   },
  //   {
  //     model: 'Configuration',
  //     subModel: 'Designation',
  //     action: [{ Label: 'Add', isSelectd: false, permission: 'configuration.designation.Add' }, { Label: 'Edit', isSelectd: false, permission: 'configuration.designation.Edit' }, { Label: 'Delete', isSelectd: false, permission: "Permissions.Designation.Delete" }, { Label: 'View', isSelectd: false, permission: 'configuration.designation.View' }]
  //   },
  //   {
  //     model: 'Configuration',
  //     subModel: 'Role',
  //     action: [{ Label: 'Add', isSelectd: false, permission: 'configuration.role.Add' }, { Label: 'Edit', isSelectd: false, permission: 'configuration.role.Edit' }, { Label: 'Delete', isSelectd: false, permission: 'configuration.role.Delete' }, { Label: 'View', isSelectd: false, permission: 'configuration.role.View' }]
  //   },
  // ];
  checkCheckedornot(elem: any) {
    return this.rolePermission?.includes(elem);
  }

  permissions: any;
  permissionGroup: any[] = [];
  setPermission = {} as permission;
  roleGroup: any;
  selectedRole: any = null;
  rolePermission: any[] = [];
  isSelected: any
  constructor(private translateService: TranslateService, private permissionService: PermissionService,private spinner:NgxSpinnerService, private toastr: ToastrService, private http: HttpClient) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
    this.getAllRole();

  }

  ngOnInit(): void {
    this.getallpermission()
  }
  Onchange(params: any) {

    if (!this.permissionGroup.includes(params)) {

      this.permissionGroup.push(params);
    }
    else {
      if (this.permissionGroup != null) {

        var index = this.permissionGroup.indexOf(params);
        this.permissionGroup.splice(index, 1);
      }
    }
  }

  fullPermission(params: any) {

    params.forEach((element: any) => {
      if (!this.permissionGroup.includes(element.permission)) {

        this.permissionGroup.push(element.permission);
      }
      else {

      }
    });
  }

  selectAllCheckbox(event: any, params: any) {
    const isChecked = event.target.checked;
    if (isChecked) {
      params.forEach((element: any) => {

        if (!this.permissionGroup.includes(element.permission)) {
          this.permissionGroup.push(element.permission);
        }
      });
    }
    if (!isChecked) {
      params.forEach((element: any) => {

        if (this.permissionGroup.includes(element.permission)) {
          var index = this.permissionGroup.indexOf(element.permission);
          this.permissionGroup.splice(index, 1);
        }
      });
    }
    // Loop through each action and update its state
  }

  nonePermissionCheckbox(event: any, params: any) {
    const isChecked = event.target.checked;
    if (isChecked) {
      params.forEach((element: any) => {

        if (this.permissionGroup.includes(element.permission)) {
          var index = this.permissionGroup.indexOf(element.permission);
          this.permissionGroup.splice(index, 1);
        }
      });
    }
  }


  selectAllCheckboxCommon(event: any) {
    const isChecked = event.target.checked;
    if (isChecked) {
      this.permissions.forEach((element: any) => {

        element.action.forEach((element: any) => {
          if (!this.permissionGroup.includes(element.permission)) {
            this.permissionGroup.push(element.permission);
          }
        });
      });
    }
    if (!isChecked) {
      this.getpermissionById(this.selectedRole);
    }
  }
  onlyViewCheckboxCommon(event: any) {

    const isChecked = event.target.checked;
    if (isChecked) {
      this.permissionGroup = [];
      this.permissions.forEach((element: any) => {

        element.action.forEach((element: any) => {
          if (element.Label == "View") {
            this.permissionGroup.push(element.permission);
          }
        });
      });
    }
    if (!isChecked) {
      this.getpermissionById(this.selectedRole);

    }

  }

  nonePermissionCheckboxCommon(event: any) {
    const isChecked = event.target.checked;
    if (isChecked) {
      this.permissionGroup = [];

    } else {
      this.getpermissionById(this.selectedRole);

    }
  }

  isSelectAllCommon() {
    if(!this.permissionGroup || !this.permissions){
      return false
    }
    else{
      return this.permissions.every((config: any) =>
        config.action.every((action: any) =>
          this.permissionGroup.includes(action.permission)
        )
      );
    }
  }
  isSelectNoneCommon() {
    if(!this.permissionGroup || !this.permissions){
      return false
    }
    else{
    return !this.permissions.some((config: any) =>
      config.action.some((action: any) =>
        this.permissionGroup.includes(action.permission)
      )
    );}
  }
  isSelectViewOnlyCommon() {

    if (this.isSelectNoneCommon() || this.isSelectAllCommon()) {
      return false;
    }
    else{
      return true;
    }
  }



  isSelectAll(params: any) {
    return params.every((item: any) => this.permissionGroup.includes(item.permission))
  }
  isSelectNone(params: any) {
    return !params.some((item: any) => this.permissionGroup.includes(item.permission))
  }
  getallpermission() {
    this.http.get<any[]>('../../../assets/permission/staticPermission.json').subscribe(data => {
      this.permissions = data;
    });
  }


  getpermissionById(id: any) {
    this.permissionGroup = [];
    this.permissionService
      .getPermissionById(this.selectedRole)
      .pipe(first())
      .subscribe((resp) => {
        this.permissionGroup=resp.httpResponse
        // if (Array.isArray(resp.httpResponse)) {
        //   resp.httpResponse.forEach((x: any) => {
        //     this.permissionGroup.push(x.value)
        //   });
        // }
      });
  }
  hasPermission(permission: any) {
    return this.permissionService.hasPermission(permission);
  }
  ValidateModalData(): boolean {
    if (!this.setPermission.role ) {
      this.toastr.error(this.translateService.instant("i18n.configuration.managePermissionDetails.selectRoleRequired",(this.spinner.hide())));
      return false;
    }
    if (!this.setPermission.permissions) {
      this.toastr.error(this.translateService.instant("i18n.configuration.managePermissionDetails.SetpermissionRequired",(this.spinner.hide())));
      return false;
    }

    return true;
  }
  savePermission() {
    this.spinner.show();
    this.setPermission.role = this.selectedRole;
    this.setPermission.permissions = this.permissionGroup;
    if (this.ValidateModalData()){
    this.permissionService
      .savePermission(this.setPermission)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toastr.success(resp.message);
          this.getpermissionById(this.setPermission.role);
          this.setPermission = {} as permission;
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toastr.error("Save Permission", err.message);
          this.spinner.hide();
        },
      });}
  }
  getAllRole() {
    this.permissionService.GetAllrole().pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.roleGroup = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error("Permission Role", err.message);
        },
      });
  }
}
