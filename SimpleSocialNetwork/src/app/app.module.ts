import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {ApiModule} from '../backend_api_client';
import {HttpClientModule} from '@angular/common/http';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {PageNotFoundComponent} from './components/page-not-found/page-not-found.component';
import {PostsComponent} from './components/posts/posts.component';
import {CreatePostComponent} from './components/create-post/create-post.component';
import {LoginComponent} from './components/login/login.component';
import {RegisterComponent} from './components/register/register.component';


@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    PostsComponent,
    CreatePostComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ApiModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
