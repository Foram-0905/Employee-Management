// import { Component, OnInit } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { TranslateService } from '@ngx-translate/core';
// import { CityService } from '../../../../shared/services/city.service';
// import { City } from '../../../../shared/models/city';
// import { Country } from '../../../../shared/models/country';
// import { State } from '../../../../shared/models/state';
// import { catchError } from 'rxjs/operators';
// import { throwError } from 'rxjs';


// @Component({
//   selector: 'app-city-add',
//   templateUrl:'./city-add.component.html',
//   styleUrl: './city-add.component.scss'
// })
// export class CityAddComponent implements OnInit {
//   lang: string = '';
//   cities: City[] = [];
//   cityname:string='';
//   countryName: { id: string, name: string }[] = [];
//   countryIds: string[] = [];
//   selectedCountryId: string = '';
//   name: { id: string, name: string }[] = [];
//   stateIds: string[] = [];
//   selectedStateId: string = '';

//   constructor(private http: HttpClient, private translateService: TranslateService, private cityService: CityService) {
//     this.translateService.setDefaultLang('en');
//     this.translateService.use(localStorage.getItem('lang') || 'en')
//   }
//   ngOnInit(): void {
//     this.getCountryNames();
//     this.getStateNames();
//     this.lang = localStorage.getItem('lang') || 'en';
//   }
//   showPopup = false;

//   togglePopup() {
//     this.showPopup = !this.showPopup;
//   }
//   getCity(): void {
//     this.cityService.getCity()
//       .pipe(
//         catchError(error => {
//           console.error('Error fetching currencies:', error);
//           return throwError(error); // Rethrow the error to propagate it further
//         })
//       )
//       .subscribe(cities => this.cities = cities);
//   }

//   getCountryNames(): void {
//     this.cityService.getCountryNames()
//       .pipe(
//         catchError(error => {
//           console.error('Error fetching country names:', error);
//           return throwError(error); // Rethrow the error to propagate it further
//         })
//       )
//       .subscribe(countries => this.countryName = countries);
//   }

//   getStateNames(): void {
//     this.cityService.getStateNames()
//       .pipe(
//         catchError(error => {
//           console.error('Error fetching state names:', error);
//           return throwError(error); // Rethrow the error to propagate it further
//         })
//       )
//       .subscribe(states => this.name = states);
//   }

//   addCity(): void {
//     const newCity: Omit<City,'id'> = {
//       name:this.cityname,
//       countryId: this.selectedCountryId,
//       stateId: this.selectedStateId,
//       action:1
//     };

//     this.cityService.addCity(newCity)
//       .pipe(
//         catchError(error => {
//           console.error('Error adding city:', error);
//           return throwError(error); // Rethrow the error to propagate it further
//         })
//       )
//       .subscribe(() => {
//         this.getCity();
//       });
//   }
// }

