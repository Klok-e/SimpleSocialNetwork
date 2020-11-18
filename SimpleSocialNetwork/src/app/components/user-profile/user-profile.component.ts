import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {LimitedUserModel, UserModel, UserApiService} from '../../../backend_api_client';
import {AuthService} from '../../services/auth.service';
import {Observable} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {CurrentUserService} from '../../services/current-user.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  constructor(private route: ActivatedRoute,
              private router: Router,
              private userApiService: UserApiService,
              private auth: AuthService,
              private userService: CurrentUserService) {
  }

  ngOnInit(): void {
    const userName = this.route.snapshot.paramMap.get('userName');
    if (userName === null) {
      this.navigateTo404();
      return;
    }
    this.userService.getUser(userName);
    this.userService.user
      .subscribe({
        error: (e: HttpErrorResponse) => {
          if (e.status === 400 || e.status === 401 || e.status === 403) {
            this.navigateTo404();
          }
        }
      });
  }

  get user(): UserModel | LimitedUserModel | null {
    return this.userService.currentUser;
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

  get getLimitedUserModel(): LimitedUserModel | null {
    if ((this.user as UserModel).dateBirth !== undefined) {
      return null;
    }
    return this.user;
  }
}
