import {Injectable} from '@angular/core';
import {UserApiService} from '../../backend_api_client';
import {AuthService} from './auth.service';
import {BehaviorSubject, Observable} from 'rxjs';
import {UnionUserModel} from '../models/helper-types';

@Injectable({
  providedIn: 'root'
})
export class ScrollToBottomService {
  constructor() {
    // TODO: idk how to make an event here, so observable of nulls is used instead
    this.userSubject = new BehaviorSubject<null>(null);
    this.toBottom = this.userSubject.asObservable();
  }

  private userSubject: BehaviorSubject<null>;
  public toBottom: Observable<null>;

  public scrolledToBottom(): void {
    this.userSubject.next(null);
    // console.log('scrolled to bottom');
  }
}
