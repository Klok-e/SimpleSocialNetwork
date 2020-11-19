import {Component, OnDestroy, OnInit} from '@angular/core';
import {SubscriptionApiService, SubscriptionModel, UserApiService} from '../../../../backend_api_client';
import {CurrentUserService} from '../../../services/current-user.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-profile-subscriptions',
  templateUrl: './profile-subscriptions.component.html',
  styleUrls: ['./profile-subscriptions.component.scss']
})
export class ProfileSubscriptionsComponent implements OnInit, OnDestroy {
  subscriptionsObserv: Subscription = new Subscription();

  subscriptions: SubscriptionModel[] | null = null;

  constructor(private subscriptionsApi: SubscriptionApiService,
              private currentUser: CurrentUserService) {
  }

  ngOnInit(): void {
    this.subscriptionsObserv.add(this.currentUser.user.subscribe(user => {
      if (user === null) {
        return;
      }
      this.subscriptionsApi.apiSubscriptionSubscribedToGet(user.login)
        .subscribe(subs => {
          this.subscriptions = subs;
        });
    }));
  }

  ngOnDestroy(): void {
    this.subscriptionsObserv.unsubscribe();
  }

}
