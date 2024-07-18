import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { User } from '../../interfaces/user';
import { ActivatedRoute, Router } from '@angular/router';
import { authActions as Actions } from '../../constants/constants';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/authServices/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-authenticate',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.scss',
})
export class AuthenticationComponent {
  registerForm: FormGroup;
  action: string;
  authActions = Actions;
  @ViewChild('password') password!: ElementRef;

  constructor(
    private fb: FormBuilder,
    private route: Router,
    private activatedRoute: ActivatedRoute,
    private toaster: ToastrService,
    private authServices: AuthService,
    private render: Renderer2
  ) {
    this.registerForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required],
    });

    this.action = '';
  }

  ngOnInit(): void {
    this.activatedRoute.data.subscribe((data) => {
      this.action = data['status'];
    });
  }

  register(): void {
    this.registerForm.setValue({
      userName: this.registerForm.value.userName.trim(),
      password: this.registerForm.value.password.trim(),
    });
    if (this.isValidDetails(this.registerForm.value)) {
      this.authServices.register(this.registerForm.value).subscribe({
        next: () => {
          this.toaster.success('User registered successfully');
          this.route.navigateByUrl('/login');
        },
        error: (error) => {
          this.toaster.error(error.error);
        },
      });
    }
  }

  login(): void {
    this.registerForm.setValue({
      userName: this.registerForm.value.userName.trim(),
      password: this.registerForm.value.password.trim(),
    });
    if (this.isValidDetails(this.registerForm.value)) {
      this.authServices.login(this.registerForm.value).subscribe({
        next: (token) => {
          localStorage.setItem('accessToken', token.jwttoken);
          localStorage.setItem('refreshToken', token.refreshToken);
          this.route.navigateByUrl('/dashboard');
        },
        error: (error) => {
          this.toaster.error(error.error);
        },
      });
    }
  }

  isValidDetails(details: User): boolean {
    this.registerForm.markAllAsTouched();
    if (this.registerForm.valid) {
      return true;
    } else {
      return false;
    }
  }

  loginPage() {
    this.route.navigateByUrl('/login');
  }

  registerPage() {
    this.route.navigateByUrl('/register');
  }

  showPassword() {
    if (
      this.render.selectRootElement(this.password.nativeElement).type === 'text'
    ) {
      this.render.setProperty(this.password.nativeElement, 'type', 'password');
    } else {
      this.render.setProperty(this.password.nativeElement, 'type', 'text');
    }
  }
}
