import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../../../services/login.service';
import { HttpContext } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs';
import { forgetPassword } from '../../../../shared/models/forgetpassword';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrl: './password-reset.component.scss'
})
export class PasswordResetComponent implements OnInit {

  ForgotPassword = {} as forgetPassword;

  constructor(private loginService: LoginService,
    private _route: ActivatedRoute, private router: Router,
    private toastr: ToastrService) {

  }
  ngOnInit(): void {
    this.loginService.clearToken();
    this.ForgotPassword.token = this._route.snapshot.queryParamMap.get('token');
  }

  async SetPassword() {
    if (!this.ForgotPassword) {
      return;
    }
    this.loginService.forgotPassword(this.ForgotPassword)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {

          // alert(resp.message)
          if(resp.isSuccess){

            this.toastr.success(resp.message)
          }
          else{
            this.toastr.error("ForgotPassword" , resp.message)

          }
          this.router.navigate(['/login']);
          this.ForgotPassword = {} as forgetPassword;
        },
        error: (err: any) => {
          alert("Forgot password " + err.message);
        }
      });
  }
}
