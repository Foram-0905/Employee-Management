import { Component } from '@angular/core';
import {LoginService} from '../../core/services/login.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl:'./dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

  constructor(private loginService: LoginService, private router: Router){

    if (!this.loginService.isUserAuthenticated()) {
      router.navigate(['/login'])
    }
    // console.log(this.loginService.getAuthenticatedUserInformation());

    console.log(this.loginService.isUserAuthenticated());

  }
  
  logout(): void {
    console.log('Logging out from Dashboard Page');
    this.loginService.logout();
  }
}
