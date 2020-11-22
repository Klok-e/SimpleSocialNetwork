import {Component, OnDestroy, OnInit} from '@angular/core';
import {CurrentUserService} from '../../../services/current-user.service';
import {LimitedUserModel, UserApiService, UserModel} from '../../../../backend_api_client';
import {HttpErrorResponse} from '@angular/common/http';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {DatePipe} from '@angular/common';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-change-info',
  templateUrl: './change-info.component.html',
  styleUrls: ['./change-info.component.scss']
})
export class ChangeInfoComponent implements OnInit, OnDestroy {
  subs: Subscription = new Subscription();

  changeInfoForm: FormGroup;

  constructor(private userService: CurrentUserService,
              private formBuilder: FormBuilder,
              private userApiService: UserApiService,
              private datePipe: DatePipe) {
    this.changeInfoForm = formBuilder.group({
      dateOfBirth: new FormControl(null),
      about: new FormControl(),
    });
  }

  get user(): UserModel | LimitedUserModel | null {
    return this.userService.currentUser;
  }

  ngOnInit(): void {
    this.subs.add(
      this.userService.user.subscribe(u => {
        if (u === null) {
          return;
        }
        if (this.getUserModel(u) !== null) {
          this.changeInfoForm.setValue({
            dateOfBirth: this.datePipe.transform(this.getUserModel(u)?.dateBirth, 'yyyy-MM-dd'),
            about: u?.about,
          });
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  onSave(): void {
    console.log(this.changeInfoForm.value);
    const dbo = this.changeInfoForm.value.dateOfBirth === '' ? null :
      this.changeInfoForm.value.dateOfBirth;
    this.userApiService.apiUserInfoPut({
      about: this.changeInfoForm.value.about,
      dateBirth: dbo,
    }).subscribe(_ => {
      if (this.user?.login !== null && this.user?.login !== undefined) {
        this.userService.changeUserTo(this.user?.login);
      }
    });
  }

  getUserModel(user: UserModel | LimitedUserModel | null): UserModel | null {
    if (user === null) {
      return null;
    }
    if ((user as UserModel).dateBirth !== undefined) {
      return user;
    }
    return null;
  }
}
