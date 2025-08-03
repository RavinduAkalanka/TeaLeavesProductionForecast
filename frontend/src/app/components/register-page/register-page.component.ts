import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { UserService } from '../../services/user.service';
import { ToasterService } from '../../services/toaster.service';

@Component({
  selector: 'app-register-page',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.css',
})
export class RegisterPageComponent implements OnInit {
  registerForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toasterService: ToasterService
  ) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group(
      {
        fullName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phone: [''],
        area: [''],
        password: ['', Validators.required],
        confirmPassword: ['', Validators.required],
        estate: [''],
        role: ['', Validators.required],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  resetForm(): void {
    this.registerForm.reset();
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const formData = this.registerForm.value;

      this.userService.registerUser(formData).subscribe({
        next: (response: any) => {
          this.toasterService.success('Account created successfully!');
          console.log('Registration Succesful:', response);
          this.resetForm();
        },
        error: (error: any) => {
          const backendError =
            error.error?.error || 'Registration failed. Please try again.';
          console.log('Error:', backendError);
          this.toasterService.error(backendError);
        },
      });
    } else {
      this.toasterService.error(
        'Please fill in all required fields correctly.'
      );
    }
  }
}
