import { Component, OnInit } from '@angular/core';
import { User, UserUpdate } from '../../model/UserRegister';
import { UserService } from '../../services/user.service';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToasterService } from '../../services/toaster.service';
import * as bootstrap from 'bootstrap';
@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent implements OnInit {
  user: User | null = null;
  profileForm!: FormGroup;
  otp = '';
  newPassword = '';

  constructor(
    private userService: UserService,
    private fb: FormBuilder,
    private toasterService: ToasterService
  ) {}

  ngOnInit(): void {
    (this.profileForm = this.fb.group({
      fullName: [''],
      email: ['', Validators.email],
      phone: [''],
      area: [''],
      estate: [''],
      role: [''],
    })),
      this.loadUserData();
  }

  private loadUserData(): void {
    const userId = sessionStorage.getItem('userId');

    if (!userId) {
      console.error('User ID not found in sessionStorage');
      return;
    }

    this.userService.getUserById(+userId).subscribe({
      next: (res: User) => {
        this.profileForm.patchValue({
          fullName: res.fullName,
          email: res.email,
          phone: res.phone,
          area: res.area,
          estate: res.estate,
          role: res.role.toLowerCase(),
        });
      },
      error: (err) => {
        console.error('Failed to load user:', err);
      },
    });
  }

  onSubmit(): void {
    if (this.profileForm.invalid) {
      return;
    }

    const userId = sessionStorage.getItem('userId');
    if (!userId) {
      console.error('No userId in localStorage');
      return;
    }

    const updatedUser: UserUpdate = this.profileForm.value;

    this.userService.updateUser(+userId, updatedUser).subscribe({
      next: (res) => {
        console.log('User updated:', res);
        this.toasterService.success('Profile updated successfully!');
      },
      error: (error) => {
        console.error('Update failed:', error);
        const errorMessage =
          error.error?.message || 'Update failed. Please try again.';
        this.toasterService.error(errorMessage);
      },
    });
  }

  sendResetPasswordEmail(): void {
    const userId = sessionStorage.getItem('userId');
    if (!userId) {
      console.log('User ID not found.');
      return;
    }
    this.userService.sendResetPasswordEmail(+userId).subscribe({
      next: (res) => {
        console.log(res);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  openOtpModalAndSendEmail() {
    const modalEl = document.getElementById('otpModal');
    if (modalEl) {
      const modal = new bootstrap.Modal(modalEl);
      modal.show();
      this.sendResetPasswordEmail();
    }
  }

  verifyOtpAndResetPassword(): void {
    const userId = sessionStorage.getItem('userId'); 
    if (!userId) {
      console.log('User ID not found.');
      return;
    }

    if (!this.otp || !this.newPassword) {
      console.log('OTP or new password is missing.');
      return;
    }

    const payload = {
      userId: +userId,
      otp: this.otp,
      newPassword: this.newPassword,
    };

    this.userService.verifyOtpAndResetPassword(payload).subscribe({
      next: (res) => {
        console.log(res);
        this.otp = '';
        this.newPassword = '';
        const modalEl = document.getElementById('otpModal');
        if (modalEl) {
          const modal = bootstrap.Modal.getInstance(modalEl);
          modal?.hide();
        }
        this.toasterService.success('Password reset successful!');
      },
      error: (err) => {
        console.log('Password reset failed:', err);
        this.toasterService.error('Failed to reset password. Check OTP.');
      },
    });
  }
}
