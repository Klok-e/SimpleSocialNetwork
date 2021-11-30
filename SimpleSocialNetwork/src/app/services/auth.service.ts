import {Injectable} from '@angular/core';
import {AuthApiService, CredentialsModel, LoggedInUser, UserModel} from '../../backend_api_client';
import {BehaviorSubject, Observable} from 'rxjs';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<LoggedInUser | null>;
  public currentUser: Observable<LoggedInUser | null>;

  constructor(private auth: AuthApiService) {
    this.currentUserSubject = new BehaviorSubject<LoggedInUser | null>(JSON.parse(localStorage.getItem('currentUser') as string));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public getCurrentUserValue(): LoggedInUser | null {
    return this.currentUserSubject.value;
  }

  get isLoggedIn(): boolean {
    return this.getCurrentUserValue() !== null;
  }

  get userLogin(): string | null {
    const user = this.getCurrentUserValue();
    if (user === null) {
      return null;
    }
    return user.login;
  }

  get isAdmin(): boolean {
    const user = this.getCurrentUserValue();
    if (user === null) {
      return false;
    }
    return user.role === 'admin';
  }

  public register(userRegister: CredentialsModel): Observable<UserModel> {
    return this.auth.apiAuthRegisterPost(userRegister, 'body');
  }

  public login(userLogin: CredentialsModel): Observable<LoggedInUser> {
    return this.auth.apiAuthLoginPost(userLogin, 'body').pipe(
      map((user) => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      })
    );
  }

  public logout(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
