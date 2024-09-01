import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LoginService } from './core/services/login.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionService } from './shared/services/permission.service';
import { IdleService } from './shared/services/Idle.service';
import { Subscription } from 'rxjs';

// import { BnNgIdleService } from 'bn-ng-idle/public-api';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'beOn HR Solution';
  isUserLoggedIn: boolean = false;
 
  // lang:string ='';
  isNavigationCollapsed: boolean = true;
  lang: string = 'en';

  idleService = inject(IdleService);
  private idleSubscription?:Subscription;

  constructor(
    private translateService: TranslateService,
    public loginService: LoginService,
    private router: Router,
    private permissionService: PermissionService,
    private _route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
    // Use the stored language
    this.translateService.setDefaultLang('en');
    this.translateService.use(this.lang || 'en');

    if (this.loginService.isUserAuthenticated()) {
      this.permissionService.loadPermissions().subscribe();
    }

    this.idleSubscription =this.idleService.idleState.subscribe((isIdle)=>{
      if(isIdle){
       
        localStorage.removeItem('currentUser');
        localStorage.removeItem('SelectedEmployeeForEdit');
        localStorage.removeItem('lang');
        this.loginService.logout();
        this.router.navigate(['/login']);
      } else {
     //   console.warn('user is active');
        
      }
    });
  }

  ngOnDestroy(){
    if(this.idleSubscription){
      this.idleSubscription.unsubscribe();
    }
  }

  onUserActive(){
    this.idleService.resetTimer();
  }

  getNavigationWidth($event: boolean) {
    // console.log('isNavigationCollapsed '+ $event);
    this.isNavigationCollapsed = $event;
  }
}
