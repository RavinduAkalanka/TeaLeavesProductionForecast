import { Component, OnInit } from '@angular/core';
import { User, UserUpdate } from '../../model/UserRegister';
import { UserService } from '../../services/user.service';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToasterService } from '../../services/toaster.service';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent implements OnInit {
  user: User | null = null;
  profileForm!: FormGroup;

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
}
