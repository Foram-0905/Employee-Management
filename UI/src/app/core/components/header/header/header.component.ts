// header.component.ts
import { Component, ViewChild, ElementRef, OnInit, HostListener, Renderer2, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../../../services/login.service';
import { first } from 'rxjs';
import { ContactAddressDetail, Contact } from '../../../../shared/models/contact';
import { ContactService } from '../../../../shared/services/contact.service';
import { ActionEnum, DefaultEmployee } from '../../../../shared/constant/enum.const';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from '../../../../shared/services/notification.service';
import { Notification } from '../../../../shared/models/notification';
import { getNotificationByEmployeeId } from '../../../../shared/constant/api.const';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']  // Corrected to styleUrls
})
export class HeaderComponent implements OnInit {
  isSendMailPopUp: boolean = false;
  lang: string = '';
  selectedLang: string = '';  // Added this line
  Contact: Contact[] = []; // Ensure this is an array
  contact: Contact = {} as Contact;
  contactAdressDetails = {} as ContactAddressDetail;
  isModalOpen: boolean = false;
  ContactResponse: any;
  // currentEmployee: any = localStorage.getItem('SelectedEmployeeForEdit')?.toLowerCase().replace(/"/g, "") || '';
  SelectedEmployee!: string;
  dropdownOpen = false;
  notification: Notification = {} as Notification;
  NotificationgetData: any;
  notificationCount: number = 0;
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('notificationPopup') notificationPopup!: ElementRef;
  @ViewChild('notificationIcon') notificationIcon!: ElementRef;
  @ViewChild('dropdownMenu') dropdownMenu!: ElementRef;
  @ViewChild('modalForm') modalForm!: ElementRef;
  editId: any;
  dataRowSource: any;
  currentEmployee: any = localStorage.getItem('SelectedEmployeeForEdit')?.toLowerCase().replace(/"/g, '') || '';

  private notificationIntervalID: any;
  private documentClickListener: (() => void) | null = null;
  constructor(
    private loginService: LoginService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private contactService: ContactService,
    private router: Router,
    private translateService: TranslateService,
    private notificationService: NotificationService,
    private renderer: Renderer2
  ) {
   
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
    if (!this.loginService.isUserAuthenticated()) {
      router.navigate(['/login']);
    }
  }

  private retrieveSelectedEmployee() {
    // Retrieve the JSON string from localStorage
    const selectedEmployeeJson: string | null = localStorage.getItem('CurrentEmployeeForNotification');

    // Parse the JSON string back to a JavaScript object, or assign a default value if it's null
    let selectedEmployee = selectedEmployeeJson ? JSON.parse(selectedEmployeeJson) : null;

    if (selectedEmployee !== null) {
      // Remove quotes if selectedEmployee is a quoted string
      if (typeof selectedEmployee === 'string') {
        selectedEmployee = selectedEmployee.replace(/^"|"$/g, '');
      }
      // Now you can use the 'selectedEmployee' variable
      this.SelectedEmployee = selectedEmployee;
    } else {
      this.SelectedEmployee = 'No selected employee found in localStorage.';
    }
  }


  ngOnInit(): void {
    
    this.lang = localStorage.getItem('lang') || 'en';
    this.selectedLang = this.lang;  // Added this line
    this.retrieveSelectedEmployee()
    this.onEditClick();
    this.GetNotificationByEmployeeId();
    // if (!this.SelectedEmployee)
      this.notificationIntervalID = setInterval(() => {
        this.GetNotificationByEmployeeId();
      }, 30000); 

    if (!this.SelectedEmployee)
      this.GetNotificationByEmployeeId();

    this.renderer.listen('window', 'click', (event: Event) => {
      this.onWindowClick(event);
    });
  }
  toggleDropdown(event: MouseEvent) {
    event.stopPropagation(); // Prevent event from propagating to document click listener
    this.dropdownOpen = !this.dropdownOpen;
  }


  onWindowClick(event: Event) {
    if (this.dropdownOpen && this.dropdownMenu && !this.dropdownMenu.nativeElement.contains(event.target)) {
      this.dropdownOpen = false;
    }
    if (this.isPopupVisible && this.notificationPopup && !this.notificationPopup.nativeElement.contains(event.target)) {
      this.isPopupVisible = false;
    }
    if (this.isPopupVisible && this.notificationPopup && !this.notificationPopup.nativeElement.contains(event.target)) {
      this.isPopupVisible = false;
    }
  }

  one: any;
  allcontact: any;

  
  onEditClick(): void {
    this.contactService.GetContactByEmployeeId(this.SelectedEmployee).pipe(first()).subscribe((resp) => {
      this.ContactResponse = resp.httpResponse;
      // console.log("Contact Response:", this.ContactResponse);
      
      if (this.ContactResponse) {
        this.allcontact = this.ContactResponse;
        // console.warn("All contacts:", this.allcontact);
        
        if (this.ContactResponse.contactAdressDetails && this.ContactResponse.contactAdressDetails.length > 0) {
          this.contactAdressDetails = this.ContactResponse.contactAdressDetails[0];
          this.contactAdressDetails.contactPhone1 = this.contactAdressDetails.contactPhone1.replace(/"/g, '');
          this.contactAdressDetails.contactEmailbeON = this.contactAdressDetails.contactEmailbeON.replace(/"/g, '');
        }
      }
    });
  }
  

  SaveHeader() {
    // console.log("Header Trigger");
    // console.log("All Contact:", this.allcontact);
    
    // Ensure contactAdressDetails is initialized
    if (!this.contactAdressDetails) {
      this.contactAdressDetails = {} as ContactAddressDetail;
    }
  
    if (this.allcontact) {
      // console.log("All Contact2:", this.allcontact);
  
      // Ensure that contactAdressDetails is properly initialized and not null
      if (this.contactAdressDetails) {
        // Perform property operations only if contactAdressDetails is valid
        this.contactAdressDetails.contactPhone1 = this.contactAdressDetails.contactPhone1 ? this.contactAdressDetails.contactPhone1.replace(/"/g, '') : '';
        this.contactAdressDetails.contactEmailbeON = this.contactAdressDetails.contactEmailbeON ? this.contactAdressDetails.contactEmailbeON.replace(/"/g, '') : '';
      } else {
        console.warn('contactAdressDetails is null or undefined.');
        // Handle the case where contactAdressDetails is null or undefined
        return;
      }
  
      // Set ActionEnum to Update for existing contacts
      this.allcontact.action = ActionEnum.Update;
  
      // Ensure ContactAdressDetails and BankDetails are set
      this.allcontact.contactAdressDetails = [this.contactAdressDetails];
      this.allcontact.bankDetails = this.allcontact.bankDetails || [];
  
      // Log to check the contact object before sending
      // console.log('Updated Contact:', this.allcontact);
  
      if (this.allcontact) {
        this.spinner.show();
        this.contactService.saveContact(this.allcontact).pipe(first()).subscribe({
          next: (data) => {
            // console.log("API Response:", data);  // Log the API response
            // if (data.success) {
            //   this.toastr.success('Header updated successfully');
            // } else {
            //   this.toastr.error('Error updating header');
            // }
            this.spinner.hide();
            this.toastr.success("Preference Updated")
            this.isModalOpen = false;
            this.changeLanguageAndRefresh();
          },
          error: (error) => {
            // console.error("API Error:", error);  // Log the error response
            this.toastr.error('Error updating header');
            this.spinner.hide();
            this.isModalOpen = false;
            this.changeLanguageAndRefresh();
          }
        });
      }
    } else {
      // console.log('No contacts found');
      this.changeLanguageAndRefresh();
    }
  }
  
  

  

  changeLanguageAndRefresh() {  // Added this method
    localStorage.setItem('lang', this.selectedLang);
    this.translateService.use(this.selectedLang);
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }

  ValidateModalData(): boolean {
    if (!this.one.contactPhone1) {
      this.toastr.error('Phone number is required');
      return false;
    }
    if (!this.one.contactEmailbeON) {
      this.toastr.error('Email is required');
      return false;
    }
    return true;
  }
  

  onLanguageSelect(event: any) {  // Added this method
    this.selectedLang = event.target.value;
  }

  logout(): void {
    this.loginService.logout();
  }

  closeModal() {
    this.isModalOpen = false;
  }

  openModal() {
    // event.stopPropagation();
    this.dropdownOpen = false;
    this.isModalOpen = true;
  }

  SendMail() {
    const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
    const email = currentUser.email;
    if (email) {
      this.spinner.show();
      this.isSendMailPopUp = true;
      this.loginService
        .sendMail(email)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.isSendMailPopUp = false;
            if (!resp.isSuccess) {
              this.toastr.error(resp.message);
              this.spinner.hide();
            } else {
              this.toastr.success(resp.message);
              this.spinner.hide();
            }
          },
          error: (err: any) => {
            this.isSendMailPopUp = false;
            this.toastr.error(err.message);
            this.spinner.hide();
          },
        });
    } else {
      this.toastr.error('Email is not available');
    }
  }

  // Notification

  isPopupVisible = false;


  // togglePopup() {
  //   this.isPopupVisible = !this.isPopupVisible;
  // }
  togglePopup(event: MouseEvent): void {
    event.stopPropagation(); // Prevent event from propagating to document click listener
    this.isPopupVisible = !this.isPopupVisible;

    if (this.isPopupVisible) {
      // Add a global click listener when the popup is shown
      this.documentClickListener = this.renderer.listen('document', 'click', (event) => {
        const clickedInsidePopup = this.notificationPopup.nativeElement.contains(event.target);
        const clickedInsideIcon = this.notificationIcon.nativeElement.contains(event.target); // Add this line
        if (!clickedInsidePopup && !clickedInsideIcon) { // Modify this condition
          this.isPopupVisible = false;
          // Remove the listener after the popup is closed
          if (this.documentClickListener) {
            this.documentClickListener();
            this.documentClickListener = null;
          }
        }
      });
    } else {
      // Remove the listener if the popup is hidden
      if (this.documentClickListener) {
        this.documentClickListener();
        this.documentClickListener = null;
      }
    }
  }
  handleDocumentClick(event: MouseEvent): void {
    const clickedInside = this.notificationPopup.nativeElement.contains(event.target);
    if (!clickedInside) {
      this.isPopupVisible = false;
    }
  }
  GetNotificationByEmployeeId() {
    this.notificationService
      .getNotificationByEmployeeId(this.SelectedEmployee)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          if (resp.httpResponse) {
            this.NotificationgetData = resp.httpResponse;
            this.notificationCount = this.NotificationgetData.filter((notification: Notification) => !notification.isRead).length; // Update notification count
            // console.log(resp.httpResponse)
          }
        },
        error: (err: any) => {
          // this.toastr.error(err.message);
        },
      });
  }

  closeNotification(notificationID: string) {
    this.notificationService.getNotificationId(notificationID)
      .subscribe({
        next: (resp: any) => {
          if (Array.isArray(resp.httpResponse) && resp.httpResponse.length > 0) {
            this.notification = resp.httpResponse[0];
          } else {
            this.toastr.error('Invalid notification data.');
            return;
          }

          // Log the received notification
          // console.log('Received notification:', this.notification);

          this.NotificationUpdate(); // Update NotificationGetData
          this.GetNotificationByEmployeeId();
          // this.notificationIntervalID = setInterval(() => {
          //   this.GetNotificationByEmployeeId();
          // }, 30000);
        },
        error: (err: any) => {
          this.toastr.error(err.message);
        },
      });
  }
  // @HostListener('document:click', ['$event'])
  // onDocumentClick(event: MouseEvent) {
  //   const clickedInside = this.notificationPopup.nativeElement.contains(event.target) ||
  //                         event.target === this.notificationPopup.nativeElement;
  //   if (!clickedInside) {
  //     this.isPopupVisible = false;
  //   }
  // }
  NotificationUpdate() {
    // Ensure all necessary properties are present in this.notification
    if (!this.notification.hasOwnProperty('notificationID') ||
      !this.notification.hasOwnProperty('employeeId') ||
      !this.notification.hasOwnProperty('notificationText') ||
      !this.notification.hasOwnProperty('notificationType') ||
      !this.notification.hasOwnProperty('isRead') ||
      !this.notification.hasOwnProperty('createdAt')) {
      this.toastr.error('Notification data is incomplete.');
      return;
    }

    // console.warn('Updating notification:', this.notification);

    this.notificationService.updateLeaveNotification(this.notification)
      .subscribe({
        next: (resp: any) => {
          this.toastr.success('Marked as Read.');
          this.GetNotificationByEmployeeId(); // Update NotificationGetData
        },
        error: (err: any) => {
          this.toastr.error(err.message);
        },
      });
  }


}




// closeNotification(index: number, notificationID: string) {
//   console.log('Notification ID:', notificationID);
//   const notification = this.NotificationgetData[index];
//   notification.isRead = true;
//   this.NotificationUpdate(notification);
//   this.NotificationgetData.splice(index, 1);
// }