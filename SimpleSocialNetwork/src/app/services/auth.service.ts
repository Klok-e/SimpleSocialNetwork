import {Injectable} from '@angular/core';
import {CredentialsModel, LoggedInUser, UserModel, AuthService as GenAuthService} from '../../backend_api_client';
import {BehaviorSubject, Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<LoggedInUser | null>;
  public currentUser: Observable<LoggedInUser | null>;

  constructor(private auth: GenAuthService) {
    this.currentUserSubject = new BehaviorSubject<LoggedInUser | null>(JSON.parse(localStorage.getItem('currentUser') as string));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public getCurrentUserValue(): LoggedInUser | null {
    return this.currentUserSubject.value;
  }

  register(userRegister: CredentialsModel): Observable<UserModel> {
    return this.auth.apiAuthRegisterPost(userRegister, 'body');
  }

  login(userLogin: CredentialsModel): void {
    const obs = this.auth.apiAuthLoginPost(userLogin, 'body');
    localStorage.setItem('currentUser', JSON.stringify(obs));
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
