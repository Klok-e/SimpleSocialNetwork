import {Component, OnInit} from '@angular/core';
import {AuthService} from '../../services/auth.service';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  incorrectCredentials = false;

  constructor(private auth: AuthService, private formBuilder: FormBuilder, private router: Router) {
    this.loginForm = this.formBuilder.group({
      login: new FormControl('', [
        Validators.required
      ]),
      password: new FormControl('', [
        Validators.required
      ]),
    });
  }

  ngOnInit(): void {
  }

  public onSubmit(): void {
    if (!this.loginForm.valid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.auth.login({
      login: this.loginForm.value.login,
      password: this.loginForm.value.password,
    }).subscribe({
      next: _ => {
        this.router.navigate(['/']);
      },
      error: _ => {
        this.incorrectCredentials = true;
      }
    });
  }
}
