import {Component, OnInit} from '@angular/core';
import {LimitedUserModel, UserModel} from '../../../backend_api_client';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  matchedUsers: LimitedUserModel[] | null = null;

  searchForm: FormGroup;

  constructor(private formBuilder: FormBuilder) {
    this.searchForm = formBuilder.group({
      name: new FormControl(''),
      about: new FormControl(''),
    });
  }

  ngOnInit(): void {
    this.matchedUsers = [
      {login: 'abcde', isDeleted: false, about: '123456789'},
      {login: 'karl_marx', isDeleted: false, about: 'workers of the world unite'},
      {login: 'engels', isDeleted: false, about: '123456789sdv ter er eger egeger'},
    ];
  }

  onSearch(): void {
    console.log('search');
  }
}
