import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {LimitedUserModel, UserModel, UserApiService} from '../../backend_api_client';
import {AuthService} from './auth.service';
import {UnionUserModel} from '../models/helper-types';
import {map, tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
  constructor(private userService: UserApiService,
              private auth: AuthService) {
    this.userSubject = new BehaviorSubject<UnionUserModel | null>(null);
    this.user = this.userSubject.asObservable();
  }

  private userSubject: BehaviorSubject<UnionUserModel | null>;
  public user: Observable<UnionUserModel | null>;

  get currentUser(): UnionUserModel | null {
    return this.userSubject.value;
  }

  get isCurrentSelf(): boolean {
    return this.auth.getCurrentUserValue()?.login === this.currentUser?.login;
  }

  public changeUserTo(userName: string): Observable<UnionUserModel> {
    const isMyProfile = this.auth.getCurrentUserValue()?.login === userName;
    return ((): Observable<UnionUserModel> => {
      if (isMyProfile || this.auth.isAdmin) {
        return this.userService.apiUserGet(userName).pipe(
          map(us => {
            return {
              modelType: 'full',
              about: us.about,
              login: us.login,
              dateBirth: us.dateBirth,
              isAdmin: us.isAdmin,
              isDeleted: us.isDeleted,
            };
          })
        );
      } else {
        return this.userService.apiUserLimitedGet(userName).pipe(
          map(us => {
            return {
              modelType: 'limited',
              about: us.about,
              login: us.login,
              isDeleted: us.isDeleted,
            };
          })
        );
      }
    })().pipe(
      tap(x => {
        this.userSubject.next(x);
      })
    );
  }
}
