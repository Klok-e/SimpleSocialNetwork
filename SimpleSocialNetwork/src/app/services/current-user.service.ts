import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {LimitedUserModel, UserModel, UserApiService} from '../../backend_api_client';
import {HttpErrorResponse} from '@angular/common/http';
import {AuthService} from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {


  constructor(private userService: UserApiService,
              private auth: AuthService) {
    this.userSubject = new BehaviorSubject<UserModel | LimitedUserModel | null>(null);
    this.user = this.userSubject.asObservable();
  }

  private userSubject: BehaviorSubject<UserModel | LimitedUserModel | null>;
  public user: Observable<UserModel | LimitedUserModel | null>;

  get currentUser(): UserModel | LimitedUserModel | null {
    return this.userSubject.value;
  }

  public getUser(userName: string): void {
    const isMyProfile = this.auth.getCurrentUserValue()?.login === userName;
    (() => {
      if (isMyProfile) {
        return this.userService.apiUserGet(userName);
      } else {
        return this.userService.apiUserLimitedGet(userName);
      }
    })().subscribe(u => {
      this.userSubject.next(u);
    });
  }
}
