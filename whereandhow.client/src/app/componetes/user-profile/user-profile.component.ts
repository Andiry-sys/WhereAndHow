import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';
import { User } from '../../Model/user';
import { AuthService } from '../../services/auth.service';
import { PartnerService } from '../../services/partner.service';
import { UpdateUserprofileService } from '../../services/update-userprofile.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
  standalone: false,
})
export class UserProfileComponent implements OnInit {
  user!: User;

  /** Reactive form holding the editable profile fields */
  profileForm = new FormGroup({
    name: new FormControl(''),
    sureName: new FormControl(''),
    email: new FormControl(''),
    password: new FormControl(''),
    phoneNumber: new FormControl(''),
    wantsPartner: new FormControl(false),
  });

  /** UI feedback messages */
  partnerMessage: string | null = null;
  partnerError: string | null = null;
  isSubmittingPartner = false;

  constructor(
    private userService: UpdateUserprofileService,
    private partnerService: PartnerService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userService.getUser().subscribe((data) => {
      this.user = data;

      // If the DB marks the user as a partner but the stored JWT doesn't reflect
      // it yet (admin confirmed while user was already logged in), silently
      // exchange the stale token for a fresh one so the entire UI updates now.
      if (data.isLosser && !this.authService.isLosser()) {
        this.authService.refreshToken().subscribe();
      }

      this.profileForm.patchValue({
        name: data.name,
        sureName: data.sureName,
        email: data.email,
        password: data.password,
        phoneNumber: data.phoneNumber,
      });
    });
  }

  /** Convenience accessor so the template can show the partner-request hint */
  get wantsPartner(): boolean {
    return !!this.profileForm.get('wantsPartner')?.value;
  }

  /** Merge the current form values back onto the user object before saving */
  private buildUpdatedUser(): User {
    const value = this.profileForm.value;
    return {
      ...this.user,
      name: value.name ?? '',
      sureName: value.sureName ?? '',
      email: value.email ?? '',
      password: value.password ?? '',
      phoneNumber: value.phoneNumber ?? '',
    };
  }

  public onSubmit(): void {
    const updatedUser = this.buildUpdatedUser();

    // If the user wants to become a partner and is not yet one, trigger the confirmation flow
    if (this.wantsPartner && !this.user.isLosser) {
      this.requestPartner(updatedUser);
      return;
    }

    // Otherwise do a normal profile update (partner status is not changed here)
    this.userService.update(updatedUser);
    this.router.navigate(['']);
  }

  private requestPartner(updatedUser: User): void {
    this.isSubmittingPartner = true;
    this.partnerMessage = null;
    this.partnerError = null;

    this.partnerService.requestPartner().subscribe({
      next: (res) => {
        this.isSubmittingPartner = false;
        this.partnerMessage = res.message;
        this.profileForm.get('wantsPartner')?.setValue(false);
        this.userService.update(updatedUser);
      },
      error: (err) => {
        this.isSubmittingPartner = false;
        this.partnerError =
          err?.error?.message ?? 'Failed to submit partner request. Please try again.';
      },
    });
  }
}
