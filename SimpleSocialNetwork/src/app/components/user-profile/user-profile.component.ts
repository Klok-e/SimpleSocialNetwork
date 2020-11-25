import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {LimitedUserModel, UserModel, UserApiService, SubscriptionApiService} from '../../../backend_api_client';
import {AuthService} from '../../services/auth.service';
import {Observable, Subscription} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {CurrentUserService} from '../../services/current-user.service';
import {UnionUserModel} from '../../models/UnionUserModel';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit, OnDestroy {

  constructor(private route: ActivatedRoute,
              private router: Router,
              private userApiService: UserApiService,
              public auth: AuthService,
              private currentUser: CurrentUserService,
              private subscribeService: SubscriptionApiService,
              private datePipe: DatePipe) {
  }

  get user(): UnionUserModel | null {
    return this.currentUser.currentUser;
  }

  get isViewingSelf(): boolean {
    return this.currentUser.isCurrentSelf;
  }

  private subscriptions: Subscription = new Subscription();

  subscribedToCurrent = false;

  isCurrentBanned = false;

  @Input() banExpirationDate = this.datePipe.transform(new Date(Date.now()), 'yyyy-MM-dd') ?? '';

  ngOnInit(): void {
    this.subscriptions.add(
      this.route.paramMap.subscribe(params => {
        const userName = params.get('userName');
        if (userName === null) {
          this.navigateTo404();
          return;
        }

        this.currentUser.changeUserTo(userName);
      })
    );

    this.subscriptions.add(
      this.currentUser.user
        .subscribe({
          next: user => {
            if (user === null) {
              return;
            }

            this.updateIsSubscribed(user);
            this.updateIsBanned(user);
          },
          error: (e: HttpErrorResponse) => {
            if (e.status === 400 || e.status === 401 || e.status === 403) {
              this.navigateTo404();
            }
          }
        })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  private updateIsBanned(newUser: UnionUserModel): void {
    if (!this.auth.isAdmin) {
      return;
    }
    this.userApiService.apiUserBannedGet(newUser.login)
      .subscribe(banned => {
        this.isCurrentBanned = banned;
      });
  }

  private updateIsSubscribed(newUser: UnionUserModel): void {
    if (!this.isViewingSelf && this.auth.isLoggedIn) {
      this.subscribeService.apiSubscriptionIsSubscribedToGet(newUser.login)
        .subscribe({
          next: res => {
            this.subscribedToCurrent = res;
          }
        });
    }
  }

  private navigateTo404(): void {
    this.router.navigate(['404-route-please-match-this-really-long-route'], {skipLocationChange: true});
  }

  getUserModel(user: UnionUserModel | null): UserModel | null {
    if (user === null) {
      return null;
    }
    if (user.modelType === 'full') {
      return user;
    }
    return null;
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

  banUser(banExpiration: string): void {
    if (this.user === null) {
      return;
    }
    const ban = {
      expirationDate: banExpiration,
      login: this.user.login,
    };
    console.log(ban);
    this.userApiService.apiUserBanPost(ban).subscribe({
      next: _ => {
        if (this.user === null) {
          return;
        }
        this.updateIsBanned(this.user);
      }
    });
  }

  liftBan(): void {
    if (this.user === null) {
      return;
    }
    this.userApiService.apiUserUnbanPost(this.user.login)
      .subscribe({
        next: _ => {
          if (this.user === null) {
            return;
          }
          this.updateIsBanned(this.user);
        }
      });
  }

  elevateUser(): void {
    if (this.user === null) {
      return;
    }
    this.userApiService.apiUserElevatePut(this.user.login)
      .subscribe({
        next: _ => {
          if (this.user === null) {
            return;
          }
          this.currentUser.changeUserTo(this.user.login);
        }
      });
  }
}
