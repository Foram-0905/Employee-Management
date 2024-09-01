import { Component, OnInit } from '@angular/core';
import { Country } from '../../../../shared/models/country'; 
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-country-add',
  templateUrl: './country-add.component.html',
  styleUrl: './country-add.component.scss'
})
export class CountryAddComponent implements OnInit {

  constructor(private http: HttpClient) { }
  ngOnInit(): void {

  }
  showPopup = false;
  togglePopup() {
    this.showPopup = !this.showPopup;
  }

  AddCountry(formData: Country) {
    // Remove 'id' field as it's auto-incremented
    const { id, ...dataToAdd } = formData;

    this.http.post<any>('https://localhost:7153/api/Country/AddCountry', dataToAdd)
      .subscribe(
        response => {
          console.log('Data added successfully:', response);
          // Handle success, update UI, etc.
        },
        error => {
          console.error('Error adding data:', error);
          // Handle error, show error message, etc.
        }
      );
  }

}
