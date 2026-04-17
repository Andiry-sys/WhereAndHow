import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../Model/User';
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

  /** Whether the "Become a Partner" checkbox is ticked in this session */
  wantsPartner = false;

  /** UI feedback messages */
  partnerMessage: string | null = null;
  partnerError: string | null = null;
  isSubmittingPartner = false;

  constructor(
    private userService: UpdateUserprofileService,
    private partnerService: PartnerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userService.getUser().subscribe((data) => {
      this.user = data;
    });
  }

  public onSubmit(): void {
    // If the user wants to become a partner and is not yet one, trigger the confirmation flow
    if (this.wantsPartner && !this.user.isLosser) {
      this.requestPartner();
      return;
    }

    // Otherwise do a normal profile update (partner status is not changed here)
    this.userService.update(this.user);
    this.router.navigate(['']);
  }

  private requestPartner(): void {
    this.isSubmittingPartner = true;
    this.partnerMessage = null;
    this.partnerError = null;

    this.partnerService.requestPartner().subscribe({
      next: (res) => {
        this.isSubmittingPartner = false;
        this.partnerMessage = res.message;
        this.wantsPartner = false;
        // Also save the other profile fields
        this.userService.update(this.user);
      },
      error: (err) => {
        this.isSubmittingPartner = false;
        this.partnerError =
          err?.error?.message ?? 'Failed to submit partner request. Please try again.';
      },
    });
  }
}
