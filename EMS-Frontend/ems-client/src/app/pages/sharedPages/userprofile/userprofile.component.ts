import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { ProfileInterface } from '../../../interface/commonInterface/profile-interface';
import { EditProfileInterface } from '../../../interface/commonInterface/edit-profile-interface';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-userprofile',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css'],
})
export class UserprofileComponent implements OnInit {
  title = 'ems-client';
  personId: any = '';
  editMode: boolean = false;
  showSuccessMessage: boolean = false;
  profileForm: FormGroup;
  toaster = inject(ToastrService);
  initialFormValues: any = {};

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private userdataservice: UserdataService,
    private router: Router
  ) {
    this.profileForm = this.fb.group({
      firstName: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(3),
        Validators.maxLength(60),
        Validators.pattern(/^(?!.*?[^aeiou]{5})(?!.*?[aeiou]{3})[a-zA-Z]*$/),]],
      lastName: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(3),
        Validators.maxLength(60),
        Validators.pattern(/^(?!.*?[^aeiou]{5})(?!.*?[aeiou]{3})[a-zA-Z]*$/),]],
      phoneNumber: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
    });
  }

  ngOnInit(): void {
    this.personId = localStorage.getItem('LoginUserId');
    this.fetchProfile(this.personId);
  }

  fetchProfile(personId: string): void {
    this.userdataservice.getProfile(personId).subscribe(
      (response: ProfileInterface) => {
        const personData = response.getPersonById;
        this.initialFormValues = {
          firstName: personData.firstName,
          lastName: personData.lastName,
          phoneNumber: personData.phoneNumber,
        };
        this.profileForm.patchValue({
          firstName: personData.firstName,
          lastName: personData.lastName,
          phoneNumber: personData.phoneNumber,
        });
      },
      (error) => console.error('Error fetching profile :', error)
    );
  }

  toggleEditMode(): void {
    this.editMode = !this.editMode;
    if (this.editMode) {
      this.profileForm.enable();
    } else {
      this.profileForm.reset(this.initialFormValues);
      this.profileForm.disable();
    }
  }

  onSubmit(): void {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched(); // To show validation errors
      return;
    }

    const formData = this.profileForm.value;
    this.userdataservice.editProfile(this.personId, formData).subscribe(
      (response: EditProfileInterface) => {
        this.toggleEditMode();
        this.toaster.success("Profile Updated Successfully");
        try {
          const role = localStorage.getItem('Role');
          if (role == 'User') {
            this.router.navigate(['user-dash', 'user-profile']);
          } else if (role == 'Admin') {
            this.router.navigate(['admin-dash', 'user-profile']);
          } else if (role == 'Organizer') {
            this.router.navigate(['organizer-dash', 'user-profile']);
          }
        } catch {
          this.router.navigate(['/login']);
          this.toaster.error('Try Again');
        }
      },
      (error: any) => {
        console.error('Error submitting form data:', error);
      }
    );
  }

  // Utility getter for easy access to form controls in the template
  get firstName() { return this.profileForm.get('firstName'); }
  get lastName() { return this.profileForm.get('lastName'); }
  get phoneNumber() { return this.profileForm.get('phoneNumber'); }
}
