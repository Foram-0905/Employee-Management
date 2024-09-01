import { Component, ViewContainerRef, ComponentFactoryResolver, Type, OnInit } from '@angular/core';

// Import all component types
import { PersonalComponent } from './personal/personal.component';
import { ContactComponent } from './contact/contact.component';
import { IdentityCardComponent } from './identity-card/identity-card.component';
import { AssetsComponent } from './assets/assets.component';
import { EducationComponent } from './education/education.component';
import { JobComponent } from './job/job.component';
import { DocumentsComponent } from './documents/documents.component';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  activeTab: string = 'personal'; // Default active tab
  flag: boolean = false;

  constructor(
    private vcr: ViewContainerRef,
    private resolver: ComponentFactoryResolver,
    private translateService: TranslateService,
    private route: ActivatedRoute
  ) {
    // Set the default active tab to "personal"
    this.toggleActive(this.activeTab);
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.flag = params['flag'] === 'true';
    });


    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  toggleActive(tab: string): void {
    if (this.flag && tab !== 'personal') {
      return;
    }
    this.activeTab = tab;
    // Dynamically load the corresponding component based on the clicked tab
    switch (tab) {
      case 'personal':
        this.loadComponent(PersonalComponent);
        break;
      case 'contact':
        this.loadComponent(ContactComponent);
        break;
      case 'identity_card':
        this.loadComponent(IdentityCardComponent);
        break;
      case 'assets':
        this.loadComponent(AssetsComponent);
        break;
      case 'education':
        this.loadComponent(EducationComponent);
        break;
      case 'job':
        this.loadComponent(JobComponent);
        break;
      case 'documents':
        this.loadComponent(DocumentsComponent);
        break;
      default:
        break;
    }
  }

  async loadComponent(componentType: Type<any>): Promise<void> {
    this.vcr.clear();
    if (!componentType) {
      console.error('Component type is undefined');
      return;
    }
    const componentFactory = this.resolver.resolveComponentFactory(componentType);
    this.vcr.createComponent(componentFactory);
  }
  
}
