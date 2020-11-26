import {Component, OnDestroy, OnInit} from '@angular/core';
import {CurrentUserService} from '../../../services/current-user.service';
import {LimitedUserModel, UserApiService, UserModel} from '../../../../backend_api_client';
import {HttpErrorResponse} from '@angular/common/http';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {DatePipe} from '@angular/common';
import {Subscription} from 'rxjs';
import {UnionUserModel} from '../../../models/helper-types';

@Component({
  selector: 'app-change-info',
  templateUrl: './change-info.component.html',
  styleUrls: ['./change-info.component.scss']
})
export class ChangeInfoComponent implements OnInit, OnDestroy {
  subs: Subscription = new Subscription();

  changeInfoForm: FormGroup;

  constructor(private currentUser: CurrentUserService,
              private formBuilder: FormBuilder,
              private userApiService: UserApiService,
              private datePipe: DatePipe) {
    this.changeInfoForm = formBuilder.group({
      dateOfBirth: new FormControl(null),
      about: new FormControl(),
    });
  }

  get user(): UnionUserModel | null {
    return this.currentUser.currentUser;
  }

  ngOnInit(): void {
    this.subs.add(
      this.currentUser.user.subscribe(u => {
        if (u === null) {
          return;
        }
        if (u.modelType === 'full') {
          this.changeInfoForm.setValue({
            dateOfBirth: this.datePipe.transform(u.dateBirth, 'yyyy-MM-dd'),
            about: u.about,
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
    const dob = this.changeInfoForm.value.dateOfBirth === '' ? null :
      this.changeInfoForm.value.dateOfBirth as string;
    this.userApiService.apiUserInfoPut({
      about: this.changeInfoForm.value.about,
      dateBirth: dob,
    }).subscribe(_ => {
      if (this.user === null) {
        return;
      }
      this.currentUser.changeUserTo(this.user.login);
    });
  }
}
