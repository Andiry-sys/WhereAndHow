import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { UpdateUserprofileService } from '../../services/update-userprofile.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
  constructor(
    private authService: AuthService,
    private userService: UpdateUserprofileService
  ) {}

  isAPartner(): boolean {
    return this.userService.checkIsLosser() 
  }

  Logout() {
    this.authService.logout();
  }

  isLoggedIn(): boolean {
    return this.authService.isUserAuthenticated();
  }
}
