import {Component, HostListener, OnInit} from '@angular/core';
import {AuthService} from './services/auth.service';
import {Router} from '@angular/router';
import {ScrollToBottomService} from './services/scroll-to-bottom.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  year = new Date().getFullYear();
  title = 'SimpleSocialNetwork';

  private scrolledToBottom = false;

  constructor(public auth: AuthService,
              private route: Router,
              private scroll: ScrollToBottomService) {
  }

  public loggedIn(): boolean {
    return this.auth.getCurrentUserValue() !== null;
  }

  public logout(): void {
    this.auth.logout();
    this.route.navigate(['']);
  }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll(): void {
    const max = document.body.scrollHeight;
    // console.log(max, window.scrollY + window.outerHeight);
    const pos = window.scrollY + window.outerHeight;
    if (pos >= max && !this.scrolledToBottom) {
      this.scroll.scrolledToBottom();
      this.scrolledToBottom = true;
    }
    if (pos <= max) {
      this.scrolledToBottom = false;
    }
  }
}
