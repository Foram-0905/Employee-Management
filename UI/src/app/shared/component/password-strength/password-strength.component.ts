// import { OnChanges } from "@angular/core";
// import { Component, Input, OnInit, SimpleChange } from '@angular/core';

// @Component({
//   selector: 'app-password-strength',
//   templateUrl: './password-strength.component.html',
//   styleUrl: './password-strength.component.scss'
// })
// export class PasswordStrengthComponent implements OnInit {
//   @Input() passwordToCheck!: string;
//   @Input() barLabel!: string;
//   bar0: string = 'grey';
//   bar1: string = 'grey';
//   bar2: string = 'grey';
//   bar3: string = 'grey';
//   bar4: string = 'grey';
//   PwdStrength!: string;

//   private colors = ['#F00', '#FF0', '#0F0'];
//   constructor() { }
//   ngOnInit(): void {

//   }
//   ngOnChanges(changes: { [propName: string]: SimpleChange }): void {
//     this.bar0 = 'grey', this.bar1 = 'grey', this.bar2 = 'grey', this.bar3 = 'grey', this.bar4 = 'grey';
//     var password = changes['passwordToCheck'].currentValue;
//     const regx = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[0-9])(?=.*?[#(~`<_+)\"'\>,{.|/;:?![@$%^&*-]).{8,}$/;

//     const strongregex = new RegExp(regx);

//     if (strongregex.test(password)) {
//       this.bar0 = 'Green'; this.bar1 = 'Green'; this.bar2 = 'Green'; this.bar3 = 'Green'; this.bar4 = 'Green';
//     }
//     else if (password.length > 5) {
//       this.bar0 = 'Yellow'; this.bar1 = 'Yellow'; this.bar2 = 'Yellow'; this.bar3 = 'Grey'; this.bar4 = 'Grey';
//     }
//     else if (password.length > 0) {
//       this.bar0 = 'red'; this.bar1 = 'red'; this.bar2 = 'Grey '; this.bar3 = 'Grey'; this.bar4 = 'Grey';
//     }
//     else {
//       this.bar0 = 'Grey'; this.bar1 = 'Grey '; this.bar2 = 'Grey '; this.bar3 = 'Grey'; this.bar4 = 'Grey';
//     }
//   }
// }


import { OnChanges } from "@angular/core";
import { Component, Input, OnInit, SimpleChange } from '@angular/core';

@Component({
  selector: 'app-password-strength',
  templateUrl:'./password-strength.component.html',
  styleUrls: ['./password-strength.component.scss']
})
export class PasswordStrengthComponent implements OnInit, OnChanges {
  @Input() barLabel!: string;
  bar0: string = 'grey';
  bar1: string = 'grey';
  bar2: string = 'grey';
  bar3: string = 'grey';
  bar4: string = 'grey';
  passwordToCheck: string = ''; // Added input property for password

  constructor() { }

  ngOnInit(): void {}

  ngOnChanges(changes: { [propName: string]: SimpleChange }): void {
    this.evaluatePasswordStrength(this.passwordToCheck);
  }

  onPasswordChange(): void {
    this.evaluatePasswordStrength(this.passwordToCheck);
  }

  private evaluatePasswordStrength(password: string): void {
    this.bar0 = 'grey'; 
    this.bar1 = 'grey'; 
    this.bar2 = 'grey'; 
    this.bar3 = 'grey'; 
    this.bar4 = 'grey';

    const regx = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[0-9])(?=.*?[#(~`<_+)\"'\>,{.|/;:?![@$%^&*-]).{8,}$/;
    const strongregex = new RegExp(regx);

    if (strongregex.test(password)) {
      this.bar0 = 'green'; 
      this.bar1 = 'green'; 
      this.bar2 = 'green'; 
      this.bar3 = 'green'; 
      this.bar4 = 'green';
    } else if (password.length > 5) {
      this.bar0 = 'yellow'; 
      this.bar1 = 'yellow'; 
      this.bar2 = 'yellow';
    } else if (password.length > 0) {
      this.bar0 = 'red'; 
      this.bar1 = 'red';
    }
  }
}
