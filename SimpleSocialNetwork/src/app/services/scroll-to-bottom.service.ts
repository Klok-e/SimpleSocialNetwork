import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';

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
