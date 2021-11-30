import {Component, OnDestroy, OnInit} from '@angular/core';
import {OpMessageApiService, OpMessageModel} from '../../../../backend_api_client';
import {CurrentUserService} from '../../../services/current-user.service';
import {Subscription} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';

@Component({
  selector: 'app-profile-user-posts',
  templateUrl: './profile-user-posts.component.html',
  styleUrls: ['./profile-user-posts.component.scss']
})
export class ProfileUserPostsComponent implements OnInit, OnDestroy {
  subs: Subscription = new Subscription();

  opMessages: OpMessageModel[] = [];

  constructor(private currentUser: CurrentUserService,
              private postsApi: OpMessageApiService) {
  }

  ngOnInit(): void {
    this.subs.add(
      this.currentUser.user
        .subscribe(u => {
          if (u === null) {
            return;
          }
          // console.log(u);
          this.postsApi.apiOpMessageFromUserGet(u.login)
            .subscribe({
              next: p => {
                this.opMessages = p;
              },
              error: (err: HttpErrorResponse) => {
                console.log('prev error handled'); // no
              }
            });
        })
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  public paragraphs(str: string): { pars: { p: string, last: boolean }[], wasTruncated: boolean } {
    let wasTruncated = false;
    let pars: string[];
    if (str.length > 500) {
      wasTruncated = true;
      pars = str.slice(0, 500).split('\n');
    } else {
      pars = str.split('\n');
    }

    return {
      pars: pars.map((x: string, ind: number) => {
        return {
          p: x,
          last: ind === pars.length - 1
        };
      }),
      wasTruncated
    };
  }
}
