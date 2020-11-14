import {Component} from '@angular/core';
import {AuthService} from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  year = new Date().getFullYear();
  title = 'SimpleSocialNetwork';

  constructor(private auth: AuthService) {
  }

  public loggedIn(): boolean {
    return this.auth.getCurrentUserValue() !== null;
  }
}
