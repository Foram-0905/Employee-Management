// // import { refreshToken } from './../../shared/constant/api.const';



// import { HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
// import { LoginService } from '../services/login.service';
// import { BehaviorSubject, Observable } from 'rxjs';
// import { user } from '../../shared/models/user';
// import { Injectable } from '@angular/core';
// import { NgxSpinnerService } from 'ngx-spinner';
// import { ToastrService } from 'ngx-toastr';

// // export const TokenInterceptor: HttpInterceptorFn=(req, next) => {
// //   let currentUser = new BehaviorSubject<user>(JSON.parse(localStorage.getItem('currentUser') || '{}')).value;
// //   const localDataGetToken=  currentUser.token;
// //   const cloneRequest =req.clone({
// //     headers:req.headers.set('Authorization','Bearer '+ localDataGetToken)
// //   });

// //    return next(cloneRequest);
// // };
// @Injectable()
// export class JwtInterceptorService implements HttpInterceptor {
//   constructor(private authenticationService: LoginService,
//     private _toastr: ToastrService,
//     private spinner: NgxSpinnerService) { }
//   intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//     let currentUser = this.authenticationService.currentUserValue;

//     // if (currentUser && currentUser.token) {
//     //   // debugger;
//     //   if (new Date(new Date().toUTCString()) > new Date(currentUser.expiration)) {
//     //     console.info("Token Expires=>", "Your token got expired, Please login back.Client");
//     //     this._toastr.warning("Your token got expired, Please login back.Client");
//     //     //this.authenticationService.logout();
//     //     this.spinner.hide();
//     //   }
//     //   else
//     //     request = request.clone({
//     //       setHeaders: {
//     //         Authorization: `Bearer ${currentUser.token}`
//     //       }
//     //     });
//     // }
//     let authReq = request;


//       if (currentUser && currentUser.token) {
//         authReq = this.addTokenHeader(request, currentUser.token);
//       }
//       else {
//         authReq = this.addEmptyTokenHeader(request);
//       }


//     return next.handle(authReq);
//   }

//   private addEmptyTokenHeader(request: HttpRequest<any>) {
//     return request.clone();
//   }

//   private addTokenHeader(request: HttpRequest<any>, token: string) {
//     return request.clone({ headers: request.headers.set('Authorization', 'Bearer ' + token) });
//   }
// }

import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { LoginService } from '../services/login.service';

export const JwtInterceptorService: HttpInterceptorFn = (req, next) => {

  const UserService = inject(LoginService)

  let loggedUserData: any;
  const localData = localStorage.getItem('currentUser');
  if (localData != null) {
    loggedUserData = JSON.parse(localData);
  }
  
  let authorizedRequest = req; // Initialize authorizedRequest with the original request
  
  if (loggedUserData && loggedUserData.token) {
    authorizedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${loggedUserData.token}`,
      },
    });
  }
  

  return next(authorizedRequest)
};
