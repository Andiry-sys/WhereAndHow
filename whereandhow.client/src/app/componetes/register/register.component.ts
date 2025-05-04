import { Component } from '@angular/core';
import {Form, FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  public registerForm!: FormGroup;
  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      email: ['', Validators.email],
      Name: ['',Validators.minLength(2)],
      PhoneNumber: ['', Validators.pattern("[0]{1}[0-9]{9}")],
      SureName: ['', Validators.minLength(2)],
      password: ['', Validators.pattern("^(?=.*[A-Z])(?=.*\\d)(?=.*[@#$%^&+=!*]).{5,}.+")],
    });
  }

  onRegister() {
    Object.keys(this.registerForm.controls).forEach(controlName => {
      this.registerForm.controls[controlName].markAsTouched();
    });
    if (this.registerForm.valid) {
      this.auth.register(this.registerForm.value);
      this.registerForm.reset();
      this.router.navigate(['']);
    }else{
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
  get Name(): FormControl{
    return this.registerForm.get("Name") as FormControl;
  }
  get SureName(): FormControl{
    return this.registerForm.get("SureName") as FormControl;
  }

  get email(): FormControl{
    return this.registerForm.get("email") as FormControl;
  }

  get PhoneNumber(): FormControl{
    return this.registerForm.get("PhoneNumber") as FormControl;
  }

  get password(): FormControl{
    return this.registerForm.get("password") as FormControl;
  }


}
