import { HttpErrorResponse } from '@angular/common/http';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserLoginRequestDto } from '../../Model/userLoginRequestDto'
@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    standalone: false
})
export class LoginComponent {
  public loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }
  onLogin() {
    Object.keys(this.loginForm.controls).forEach(controlName => {
      this.loginForm.controls[controlName].markAsTouched();
    });

    if (this.loginForm.valid) {

      const user: UserLoginRequestDto = {
        email: this.loginForm.value.email,
        password : this.loginForm.value.password
      };
      this.auth.login(this.loginForm.value).subscribe({
        next: () => {
          if (this.auth.isLoggedIn()) {
            this.router.navigate(['']);
          }
        }
      });
    }

  }



  type: string = "password";
  isText: boolean = false;
  eyeIcon: string ="fa-eye-slash";
  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password";

  }
  get email(): FormControl{
    return this.loginForm.get("email") as FormControl;
  }
  get password(): FormControl{
    return this.loginForm.get("password") as FormControl;
  }
}
