import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {ApiModule, BASE_PATH} from '../backend_api_client';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {PageNotFoundComponent} from './components/page-not-found/page-not-found.component';
import {PostsComponent} from './components/posts/posts.component';
import {CreatePostComponent} from './components/create-post/create-post.component';
import {LoginComponent} from './components/login/login.component';
import {RegisterComponent} from './components/register/register.component';
import {ReadPostComponent} from './components/read-post/read-post.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {JwtAuthInterceptor} from './interceptors/jwt-auth.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    PostsComponent,
    CreatePostComponent,
    LoginComponent,
    RegisterComponent,
    ReadPostComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ApiModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    {provide: BASE_PATH, useValue: 'https://localhost:5001'},
    {provide: HTTP_INTERCEPTORS, useClass: JwtAuthInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
