import { Output, EventEmitter, Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { PermissionService } from '../../../../shared/services/permission.service';
@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {

  @Output() isCollapsedNavigation = new EventEmitter<boolean>();

  lang: string = '';
  isCollapsed: boolean = true; // Initially, the sidebar is collapsed
  isMenuClicked: boolean = false; // Variable to track if the menu button is clicked

  constructor(private translateService: TranslateService, private router: Router,private permission: PermissionService,) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
    this.isCollapsedNavigation.emit(this.isCollapsed); // Ensure initial collapsed state is emitted
  }
  hasPermission(permission: any) {
    return this.permission.hasPermission(permission);
  }
  toggleCollapse() {
    this.isCollapsed = !this.isCollapsed;
    this.isMenuClicked = !this.isMenuClicked; // Update isMenuClicked when menu button is clicked
    this.isCollapsedNavigation.emit(this.isCollapsed); // Emit the collapse state of the navigation bar
  }

  closesidebarhover() {
    // Collapse sidebar only if it's currently expanded and menu button is not clicked
    if (!this.isMenuClicked && !this.isCollapsed) {
      this.isCollapsed = true;
      this.isCollapsedNavigation.emit(this.isCollapsed); // Emit the collapse state of the navigation bar
    }
  }

  opensidebarhover() {
    // Expand sidebar only if it's currently collapsed and menu button is not clicked
    if (!this.isMenuClicked && this.isCollapsed) {
      this.isCollapsed = false;
      this.isCollapsedNavigation.emit(this.isCollapsed); // Emit the collapse state of the navigation bar
    }
  }

  isActive(route: string): boolean {
    return this.router.isActive(route, true);
  }

  ChangeLang(lang: any) {
    const selectedLanguage = lang.target.value;
    localStorage.setItem('lang', selectedLanguage);
    this.translateService.use(selectedLanguage);
  }
}
