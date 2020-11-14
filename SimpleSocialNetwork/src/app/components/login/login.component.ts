import {Component, Input, OnInit} from '@angular/core';
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
    this.auth.login({
      login: this.loginForm.value.login,
      password: this.loginForm.value.password,
    }).subscribe((login) => {
      this.router.navigate(['/']);
    });
  }
}
