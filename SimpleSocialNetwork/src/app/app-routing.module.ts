import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {PageNotFoundComponent} from './components/page-not-found/page-not-found.component';
import {PostsComponent} from './components/posts/posts.component';
import {CreatePostComponent} from './components/create-post/create-post.component';
import {LoginComponent} from './components/login/login.component';
import {RegisterComponent} from './components/register/register.component';
import {AuthGuard} from './guards/auth.guard';
import {ReadPostComponent} from './components/read-post/read-post.component';

const routes: Routes = [
  {path: '', component: PostsComponent, pathMatch: 'full'},
  {path: 'create-post', component: CreatePostComponent, canActivate: [AuthGuard]},
  {path: 'posts/:id', component: ReadPostComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: '**', component: PageNotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
