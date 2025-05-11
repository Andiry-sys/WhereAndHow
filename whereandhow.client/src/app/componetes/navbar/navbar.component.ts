import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { UpdateUserprofileService } from '../../services/update-userprofile.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
  constructor(
    private authService: AuthService,
    private userService: UpdateUserprofileService,
    private router: Router
  ) {}

  isAPartner(): boolean {
    return this.userService.checkIsLosser();
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  Logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
