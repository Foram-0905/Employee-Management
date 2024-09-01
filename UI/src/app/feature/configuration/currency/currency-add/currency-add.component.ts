import { Component, OnInit } from '@angular/core';
import { CurrencyService } from '../../../../shared/services/currency.service';
import { Currency } from '../../../../shared/models/currency';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { ActionEnum } from '../../../../shared/constant/enum.const';

@Component({
  selector: 'app-currency-add',
  templateUrl: './currency-add.component.html',
  styleUrls: ['./currency-add.component.scss']
})
export class CurrencyAddComponent implements OnInit {
  currencies: Currency[] = [];
  countryName: { id: string, name: string }[] = [];
  countryIds: string[] = [];
  selectedCountryId: string = '';
  shortWord: string = '';
  symbol: string = '';
  ManageCurrency: string = "";
  lang:string="";

  constructor(private currencyService: CurrencyService, private translateService: TranslateService) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }

  ngOnInit(): void {
    // this.loadTranslations();
    this.getCurrencies();
    this.getCountryNames();
    this.lang = localStorage.getItem('lang') || 'en';

  }
  getCurrencies(): void {
    this.currencyService.getCurrencies()
      .pipe(
        catchError(error => {
          // console.error('Error fetching currencies:', error);
          return throwError(error); // Rethrow the error to propagate it further
        })
      )
      .subscribe(currencies => this.currencies = currencies);
  }

  getCountryNames(): void {
    this.currencyService.getCountryNames()
      .pipe(
        catchError(error => {
          // console.error('Error fetching country names:', error);
          return throwError(error); // Rethrow the error to propagate it further
        })
      )
      .subscribe(countries => this.countryName = countries);
  }

  addCurrency(): void {
    const newCurrency: Omit<Currency, 'id'> = {
      country: this.selectedCountryId,
      shortWord: this.shortWord,
      symbol: this.symbol,
      // action:ActionEnum.Insert
    };

    this.currencyService.addCurrency(newCurrency)
      .pipe(
        catchError(error => {
          // console.error('Error adding currency:', error);
          return throwError(error); // Rethrow the error to propagate it further
        })
      )
      .subscribe(() => {
        this.getCurrencies();
      });
  }
  // loadTranslations() {
  //   // Fetch translation for 'manageCurrency'
  //   this.ManageCurrency = this.translateService.instant('configuration.manageCurrency');
  // }
}

