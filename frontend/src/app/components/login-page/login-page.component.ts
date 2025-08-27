import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserService } from '../../services/user.service';
import { ToasterService } from '../../services/toaster.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { jwtDecode } from 'jwt-decode';



@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css',
})
export class LoginPageComponent implements OnInit {
  loginForm!: FormGroup;

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

  logout(): void {
    this.authService.logout();
    this.toasterService.success('Logged out successfully!');
    this.router.navigate(['/login']);
  }
}
