import {Component, OnInit} from '@angular/core';
import {
  AbstractControl,
  AsyncValidatorFn,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {Router} from '@angular/router';
import {UserApiService} from '../../../backend_api_client';
import {mergeMap} from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  loginForm: FormGroup;

  constructor(private auth: AuthService,
              private formBuilder: FormBuilder,
              private router: Router,
              private users: UserApiService) {
    this.loginForm = this.formBuilder.group({
      login: new FormControl('', [
        Validators.required
      ], [
        this.userDoesntExistValidator()
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(6)
      ]),
      password_confirm: new FormControl('', [
        Validators.required,
        Validators.minLength(6)
      ]),
    }, {validators: [mustMatch('password', 'password_confirm')]});
  }

  ngOnInit(): void {
  }

  public onSubmit(): void {
    if (!this.loginForm.valid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const cred = {
      login: this.loginForm.value.login,
      password: this.loginForm.value.password,
    };
    this.auth.register(cred).pipe(
      mergeMap(_ => {
        return this.auth.login(cred);
      }),
      mergeMap(_ => {
        return this.router.navigate(['/']);
      })
    ).subscribe();
  }

  public userDoesntExistValidator(): AsyncValidatorFn {
    return async (control: AbstractControl): Promise<ValidationErrors | null> => {
      const login: string = control.value;
      return login && await this.users.apiUserExistsGet(login).toPromise() ? {userWithLoginExists: true} : null;
    };
  }
}

function mustMatch(name1: string, name2: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const n1 = control.value[name1];
    const n2 = control.value[name2];

    return n1 && n2 && n1 !== n2 ? {dontMatch: true} : null;
  };
}
