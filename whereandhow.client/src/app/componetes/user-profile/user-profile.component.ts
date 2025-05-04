import { User } from '../../model/user';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UpdateUserprofileService } from '../../services/update-userprofile.service';
@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
})
export class UserProfileComponent {
  user!: User;

  constructor(
    private userService: UpdateUserprofileService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userService.getUser().subscribe((data) => {
      this.user = data;
    });
  }

  public onSubmit() {
    this.userService.update(this.user);
    this.router.navigate(['']);
  }
}
