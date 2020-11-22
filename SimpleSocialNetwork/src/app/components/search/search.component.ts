import {Component, OnInit} from '@angular/core';
import {LimitedUserModel, UserApiService, UserModel} from '../../../backend_api_client';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  matchedUsers: LimitedUserModel[] = [];

  searchForm: FormGroup;

  constructor(private formBuilder: FormBuilder,
              private users: UserApiService,
              private route: ActivatedRoute,
              private router: Router) {
    this.searchForm = formBuilder.group({
      name: new FormControl(''),
      about: new FormControl(''),
    });
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(param => {
      const name = param.get('name') ?? undefined;
      const about = param.get('about') ?? undefined;
      // console.log('search', name, about);
      this.users.apiUserSearchGet(name, about)
        .subscribe(users => {
          this.matchedUsers = users;
        });

      this.searchForm.setValue({
        name: name ?? '',
        about: about ?? '',
      });
    });
  }

  onSearch(): void {
    const name = this.searchForm.value.name;
    const about = this.searchForm.value.about;
    this.router.navigate([], {
      queryParams: {
        name: name === '' ? undefined : name,
        about: about === '' ? undefined : about
      }
    });
  }
}
