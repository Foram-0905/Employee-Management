import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { user } from '../../shared/models/user';
import { BehaviorSubject, Observable, Subject, map } from 'rxjs';
import { changeToken, Login, resetPassword, sendMail } from '../../shared/constant/api.const';
import { forgetPassword } from '../../shared/models/forgetpassword';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root',
})
export class LoginService {

  public currentUser: BehaviorSubject<user>;

  constructor(
    private http: HttpClient,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.currentUser = new BehaviorSubject<user>(JSON.parse(localStorage.getItem('currentUser') || '{}'));
  }

  public $refreshToken = new Subject<boolean>();

  public get currentUserValue() {
    return this.currentUser.value;
  }

  login(username: string, password: string) {
    return this.http.post<any>(Login, { username, password }).pipe(map((User) => {
      if (User.isSuccess) {
        console.log(User.httpResponse);

        localStorage.setItem('currentUser', JSON.stringify(User.httpResponse));
        localStorage.setItem('CurrentEmployeeForNotification',JSON.stringify(User.httpResponse.name));
        localStorage.setItem('SelectedEmployeeForEdit', JSON.stringify(User.httpResponse.name));
        this.currentUser.next(User.httpResponse);
      }  
      return User;
    }));
  }
 expirationes:boolean=true;
  getRefreshToken(): Observable<any> {
    const obj: any = {
      token: this.currentUser.value.token,
      expiration: this.expirationes,
     email: this.currentUser.value.email
    };
    return this.http.post<any>(changeToken, obj).pipe(
      map((response: any) => {
        if (response.isSuccess) {
          let currentUser = this.currentUserValue;
          currentUser.token = response.httpResponse.token;
          currentUser.expiration = response.httpResponse.expiration;
          currentUser.email = response.httpResponse.email;
          localStorage.setItem('currentUser', JSON.stringify(currentUser));
          this.currentUser.next(currentUser);
        }
        return response;
      })
    );
  }

  // getRefreshToken(): Observable<any> {
  //   const obj: any = {
  //     token: this.currentUser.value.token,
  //     expiration: this.currentUser.value.expiration,
  //     email: this.currentUser.value.email
  //   };
  //   return this.http.post<any>(changeToken, obj).pipe(
  //     map((response: any) => {
  //       if (response.isSuccess) {
  //         let currentUser = this.currentUserValue;
  //         currentUser.token = response.httpResponse.token;
  //         currentUser.expiration = response.httpResponse.expiration;
  //         currentUser.email = response.httpResponse.email;
  //         localStorage.setItem('currentUser', JSON.stringify(currentUser));
  //         this.currentUser.next(currentUser);
  //       }
  //       return response;
  //     })
  //   );
  // }

  sendMail(Mail: string) {
    return this.http.get<any>(`${sendMail}/${Mail}`);
  }

  forgotPassword(setPassword: forgetPassword) {
    return this.http.post<any>(resetPassword, setPassword);
  }

  clearToken(): void {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('SelectedEmployeeForEdit');
    localStorage.removeItem('lang');
  }

  logout() {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('SelectedEmployeeForEdit');
    localStorage.removeItem('lang');
    this.router.navigate(['/login']);
  }

  isUserAuthenticated() {
    return JSON.parse(localStorage.getItem("currentUser") || '{}').id ? true : false;
  }

  getLoginUser() {
    return JSON.parse(localStorage.getItem("currentUser") || '{}').name;
  }

}
