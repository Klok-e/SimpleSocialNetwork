import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {PageNotFoundComponent} from './components/page-not-found/page-not-found.component';
import {PostsComponent} from './components/posts/posts.component';
import {CreatePostComponent} from './components/create-post/create-post.component';
import {LoginComponent} from './components/login/login.component';
import {RegisterComponent} from './components/register/register.component';
import {AuthGuard} from './guards/auth.guard';
import {ReadPostComponent} from './components/read-post/read-post.component';
import {UserProfileComponent} from './components/user-profile/user-profile.component';
import {ProfileSubscriptionsComponent} from './components/user-profile/profile-subscriptions/profile-subscriptions.component';
import {ChangeInfoComponent} from './components/user-profile/change-info/change-info.component';
import {ProfileUserPostsComponent} from './components/user-profile/profile-user-posts/profile-user-posts.component';
import {SearchComponent} from './components/search/search.component';

const routes: Routes = [
  {path: '', component: PostsComponent, pathMatch: 'full'},
  {path: 'create-post', component: CreatePostComponent, canActivate: [AuthGuard]},
  {path: 'posts/:id', component: ReadPostComponent},
  {
    path: 'u/:userName',
    component: UserProfileComponent,
    children: [
      {path: '', redirectTo: 'posts', pathMatch: 'full'},
      {path: 'posts', component: ProfileUserPostsComponent},
      {path: 'subs', component: ProfileSubscriptionsComponent},
      {path: 'change-info', component: ChangeInfoComponent, canActivate: [AuthGuard]},
    ]
  },
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'search', component: SearchComponent},
  {path: '**', component: PageNotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
