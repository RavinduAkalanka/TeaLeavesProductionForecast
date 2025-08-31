import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserService } from '../../services/user.service';
import { ToasterService } from '../../services/toaster.service';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { jwtDecode } from 'jwt-decode';
import * as bootstrap from 'bootstrap';

@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, CommonModule, RouterModule, FormsModule],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css',
})
export class LoginPageComponent implements OnInit {
  loginForm!: FormGroup;

  otp = '';
  newPassword = '';
  email = '';
  step: number = 1;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toasterService: ToasterService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  resetForm(): void {
    this.loginForm.reset();
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const formData = this.loginForm.value;
      console.log('Form Data:', formData);
      this.userService.loginUser(formData).subscribe({
        next: (response: any) => {
          const token = response.token;
          this.authService.setToken(token);

          try {
            const decoded: any = jwtDecode(token);
            const userId = decoded.sub || decoded['User ID'];
            sessionStorage.setItem('userId', userId);
            console.log('Decoded user ID:', userId);
          } catch (err) {
            console.error('Token decoding failed:', err);
          }

          this.resetForm();
          this.toasterService.success('Login successfully!');
          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          const errorMessage =
            error.error?.message || 'Login failed. Please try again.';
          this.toasterService.error(errorMessage);
          console.error('Login failed', error);
        },
      });
    } else {
      this.toasterService.error('Please fill in all required fiels correctly.');
    }
  }

  openOtpModal() {
    this.step = 1;
    const modalEl = document.getElementById('otpModal');
    if (modalEl) {
      const modal = new bootstrap.Modal(modalEl);
      modal.show();
    }
  }
  Error: string = '';
  sendEmail() {
    if (!this.email) {
      this.Error = 'Please enter your email';
      return;
    }

    this.userService.sendResetPasswordEmailByEmail(this.email).subscribe({
      next: (res) => {
        this.toasterService.success('OTP sent to your email!');
        this.step = 2;
      },
      error: (err) => {
        console.log(err);
        this.toasterService.error('Failed to send OTP');
      },
    });
  }

  verifyOtpAndResetPassword() {
    if (!this.otp || !this.newPassword) {
      this.Error = 'Please enter OTP and new password';
      return;
    }

    this.userService.verifyOtpWithEmailAndResetPassword({
      email: this.email,
      otp: this.otp,
      newPassword: this.newPassword
    }).subscribe({
      next: (res) => {
        console.log(res);
        this.email = '';
        this.otp = '';
        this.newPassword = '';
        this.step = 1;
        const modalEl = document.getElementById('otpModal');
        if (modalEl) {
          const modal = bootstrap.Modal.getInstance(modalEl);
          modal?.hide();
        }
        this.toasterService.success('Password reset successful!');
      },
      error: (err) => {
        console.log(err);
        this.toasterService.error('Failed to reset password');
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.toasterService.success('Logged out successfully!');
    this.router.navigate(['/login']);
  }
}
