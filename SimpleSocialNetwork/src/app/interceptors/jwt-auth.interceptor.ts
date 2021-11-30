import {Injectable} from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import {Observable, of, throwError,from} from 'rxjs';
import {AuthService} from '../services/auth.service';
import {catchError, finalize, map, mergeMap, tap} from 'rxjs/operators';
import {Router} from '@angular/router';

@Injectable()
export class JwtAuthInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService, private router: Router) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const currentUser = this.auth.getCurrentUserValue();
    if (currentUser?.token) {
      request = request.clone({
        setHeaders: {
          'X-Authorization': `Bearer ` + currentUser.token,
        }
      });
    }
    // console.log(request);
    return next.handle(request).pipe(
      catchError(x => this.handleAuthError(x))
    );
  }

  // tslint:disable-next-line:no-any
  // https://stackoverflow.com/questions/46017245/how-to-handle-unauthorized-requestsstatus-with-401-or-403-with-new-httpclient/46017463
  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    // handle auth error or rethrow
    if (err.status === 401 || err.status === 403) {
      console.log('observe1', err);

      return from(this.router.navigate([''])).pipe(
        tap(_ => {
          this.auth.logout();
        }),
        map(_ => {
          return err.message;
        }));
    }
    return throwError(err);
  }
}
