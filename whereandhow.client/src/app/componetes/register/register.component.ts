import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
    standalone: false
})
export class RegisterComponent implements OnInit {
  public registerForm!: FormGroup;
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      Name: ['', [Validators.required, Validators.minLength(2)]],
      SureName: ['', [Validators.required, Validators.minLength(2)]],
      PhoneNumber: ['', [Validators.required, Validators.pattern("[0]{1}[0-9]{9}")]],
      password: ['', [Validators.required, Validators.pattern("^(?=.*[A-Z])(?=.*\\d)(?=.*[@#$%^&+=!*]).{5,}")]],
    });
  }

  onRegister(): void {
    Object.keys(this.registerForm.controls).forEach(controlName => {
      this.registerForm.controls[controlName].markAsTouched();
    });

    if (this.registerForm.valid) {
      this.auth.register(this.registerForm.value).subscribe({
        next: () => {
          this.router.navigate(['']).then(() => this.registerForm.reset());
        }
      });
    }
  }

  hideShowPass(): void {
    this.isText = !this.isText;
    this.eyeIcon = this.isText ? "fa-eye" : "fa-eye-slash";
    this.type = this.isText ? "text" : "password";
  }

  get Name(): FormControl {
    return this.registerForm.get("Name") as FormControl;
  }
  get SureName(): FormControl {
    return this.registerForm.get("SureName") as FormControl;
  }
  get email(): FormControl {
    return this.registerForm.get("email") as FormControl;
  }
  get PhoneNumber(): FormControl {
    return this.registerForm.get("PhoneNumber") as FormControl;
  }
  get password(): FormControl {
    return this.registerForm.get("password") as FormControl;
  }
}
