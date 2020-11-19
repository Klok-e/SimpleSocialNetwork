import {Component, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {LimitedUserModel, UserModel, UserApiService, SubscriptionApiService} from '../../../backend_api_client';
import {AuthService} from '../../services/auth.service';
import {Observable, Subscription} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {CurrentUserService} from '../../services/current-user.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription = new Subscription();

  subscribedToCurrent = false;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private userApiService: UserApiService,
              public auth: AuthService,
              private userService: CurrentUserService,
              private subscribeService: SubscriptionApiService) {
  }

  ngOnInit(): void {
    this.subscriptions.add(this.route.paramMap.subscribe(params => {
      const userName = params.get('userName');
      if (userName === null) {
        this.navigateTo404();
        return;
      }

      this.userService.changeUserTo(userName);
    }));

    this.subscriptions.add(this.userService.user
      .subscribe({
        next: user => {
          if (user === null) {
            return;
          }

          this.updateIsSubscribed(user);
        },
        error: (e: HttpErrorResponse) => {
          if (e.status === 400 || e.status === 401 || e.status === 403) {
            this.navigateTo404();
          }
        }
      }));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  private updateIsSubscribed(user: UserModel | LimitedUserModel): void {
    if (!this.isViewingSelf && this.auth.isLoggedIn) {
      this.subscribeService.apiSubscriptionIsSubscribedToGet(user.login)
        .subscribe({
          next: res => {
            this.subscribedToCurrent = res;
          }
        });
    }
  }

  get user(): UserModel | LimitedUserModel | null {
    return this.userService.currentUser;
  }

  get pageBelongsToCurrentUser(): boolean {
    return this.user?.login === this.auth.getCurrentUserValue()?.login;
  }

  private navigateTo404(): void {
    this.router.navigate(['404-route-please-match-this-really-long-route'], {skipLocationChange: true});
  }

  get getUserModel(): UserModel | null {
    if ((this.user as UserModel).dateBirth !== undefined) {
      return this.user;
    }
    return null;
  }

  get isViewingSelf(): boolean {
    return this.userService.isCurrentSelf;
  }

  public subscribeToCurrent(): void {
    // console.log('sub', this.user);
    if (this.user === null) {
      return;
    }
    this.subscribeService.apiSubscriptionSubPost(this.user.login)
      .subscribe({
        next: _ => {
          if (this.user === null) {
            return;
          }
          this.updateIsSubscribed(this.user);
        }
      });
  }

  public unsubFromCurrent(): void {
    // console.log('unsub', this.user);

    if (this.user === null) {
      return;
    }
    this.subscribeService.apiSubscriptionUnsubPost(this.user.login)
      .subscribe({
        next: _ => {
          if (this.user === null) {
            return;
          }
          this.updateIsSubscribed(this.user);
        }
      });
  }
}
