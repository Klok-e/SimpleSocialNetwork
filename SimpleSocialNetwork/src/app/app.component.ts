import {Component, OnInit} from '@angular/core';
import {AuthService} from './services/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  year = new Date().getFullYear();
  title = 'SimpleSocialNetwork';

  constructor(public auth: AuthService, private route: Router) {
  }

  public loggedIn(): boolean {
    return this.auth.getCurrentUserValue() !== null;
  }

  public logout(): void {
    this.auth.logout();
    this.route.navigate(['']);
  }
}
