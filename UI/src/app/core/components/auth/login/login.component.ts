import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router'; // Import Router
import { first } from 'rxjs';
import { LoginService } from '../../../services/login.service';
import { ReactiveFormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import {DefultBeonRoute}  from '../../../../shared/constant/general.const';
import { PermissionService } from '../../../../shared/services/permission.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  resetPasswordScreen: string = '';
  Email: any;
  isSendMailPopUp: boolean=false;
  mailSucess:boolean=false;
  get formControl() {
    return this.loginForm.controls;
  }

  constructor(private FB: FormBuilder,private spinner: NgxSpinnerService,
    private loginService: LoginService,
    private router: Router,
    private toastr: ToastrService,
    private permissionService:PermissionService) {
    this.loginForm = this.FB.group({
      UserName: ['', [Validators.required]],
      Password: ['', [Validators.required]],
    });
  }
  ngOnInit(): void { }

  showScreen(screen: string) {
    this.resetPasswordScreen = screen;
  }
  async authenticate() {
    if (this.loginForm.invalid) {
      this.toastr.error("Please Enter Username and Password");
      return;
    }
    this.loginService.clearToken();
    this.loginService
      .login(this.formControl['UserName'].value, this.formControl['Password'].value)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // // debugger
          if (!resp.isSuccess) {
            this.toastr.error(resp.message);
          }
          else {
            document.cookie = `lang=${resp.httpResponse.preferedLanguage}; expires=Fri, 31 Dec 9999 23:59:59 GMT`;
            this.permissionService.loadPermissions().subscribe();
            localStorage.setItem('lang', resp.httpResponse.preferedLanguage);
            // console.log('lang(login):', resp.httpResponse.preferedLanguage);

            this.router.navigate([DefultBeonRoute]);
            this.loginForm.reset();

          }
        },
        error: (err: any) => {
          this.toastr.error("Login:" , err.message);
        },
      });
  }

  SendMail() {
    if (this.Email) {
      this.spinner.show();
      this.isSendMailPopUp=true;
      this.loginService
        .sendMail(this.Email)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            if (!resp.isSuccess) {
              this.isSendMailPopUp = false;
              this.toastr.error(resp.message);
              this.spinner.hide();
            }
            else{
              this.toastr.success(resp.message);
              this.mailSucess=true
              this.spinner.hide();
            }
          },
          error: (err: any) => {
            this.isSendMailPopUp = false;
            this.toastr.error(err.message);
            this.spinner.hide();
          },
        });
    } else {
      this.toastr.error('Email is not available');
    }
  }
}
