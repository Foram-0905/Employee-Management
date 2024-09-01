import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { LoginService } from '../services/login.service';
import {DefultBeonRoute}  from '../../shared/constant/general.const';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, map, of } from 'rxjs';
import { cu } from '@fullcalendar/core/internal-common';


export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const loginService = inject(LoginService);
  const tostar= inject(ToastrService);
  let currentUser = loginService.currentUserValue;
// console.log('-- Inside CanActivateFn');

  if ( currentUser &&
    currentUser.expiration && !loginService.isUserAuthenticated()) {
      // loginService.getRefreshToken().subscribe(
      //   (tokenRefresh) => {
      //     let currentUser = loginService.currentUserValue;
      //     // console.log(currentUser)
      //     currentUser.token = tokenRefresh.httpResponse.token;
      //     currentUser.expiration= tokenRefresh.httpResponse.expiration;
      //     currentUser.email=tokenRefresh.httpResponse.username          
      //     localStorage.setItem('currentUser', JSON.stringify(currentUser));
      //   },
      //   (error) => {
      //     console.error("Token refresh error", error);
      //   }
      // );
      router.navigate(['/employes/employelist']);
    return false;

  } else {

    loginService.getRefreshToken().subscribe(
      (tokenRefresh) => {
        let currentUser = loginService.currentUserValue;
       // console.log('second',currentUser)
        currentUser.token = tokenRefresh.httpResponse.token;
          currentUser.expiration= tokenRefresh.httpResponse.expiration;
          currentUser.email=tokenRefresh.httpResponse.email 
        localStorage.setItem('currentUser', JSON.stringify(currentUser));
      },
      (error) => {
        console.error("Token refresh error", error);
      }
    );
    // loginService.getRefreshToken(currentUser.token);
    return true;
  }

};
// export const authGuard: CanActivateFn = (route, state) => {
//   const router = inject(Router);
//   const loginService = inject(LoginService);
//   const tostar = inject(ToastrService);
//   let currentUser = loginService.currentUserValue;


//   if (
//     currentUser &&
//     currentUser.expiration
//   ) {  
//       // loginService.logout();
//       //  router.navigate(['/login']);
//       return false;
    
//   } else {
//     loginService.getRefreshToken().subscribe(
//       (tokenRefresh) => {
//         let currentUser = loginService.currentUserValue;
//         currentUser.token = tokenRefresh.httpResponse.token;
//         currentUser.expiration = false;
//         localStorage.setItem('currentUser', JSON.stringify(currentUser));
//       },
//       (error) => {
//         console.error('Token refresh error', error);
//       }
//     );
//     return true;
//   } 

  
  
// };

// export const authGuard: CanActivateFn = (route, state) => {
//   const router = inject(Router);

//   const loginService = inject(LoginService);

//    //console.log('-- Inside second CanActivateFn');

//     if (!loginService.isUserAuthenticated()) {
//     // isUserLoggedIn = false;
//     //  console.log('User is  Authenticated');
//     router.navigate(['/employes/employelist']);

//     // router.navigate(['/login']);
//     return false;

//   } else {
//     // console.log('User is not Authenticated');

//          //router.navigate(['/configuration/slg']);
//     return true;
//   }

// };


export const authGuards: CanActivateFn = (route, state) => {
  const router = inject(Router);

  const loginService = inject(LoginService);

   //console.log('-- Inside second CanActivateFn');

    if (loginService.isUserAuthenticated()) {
    // isUserLoggedIn = false;
    //  console.log('User is  Authenticated');
    router.navigate([DefultBeonRoute]);

    // router.navigate(['/login']);
    return false;

  } else {
    // console.log('User is not Authenticated');

         //router.navigate(['/configuration/slg']);
    return true;
  }

};
