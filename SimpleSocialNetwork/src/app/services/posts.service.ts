import {Injectable} from '@angular/core';
import {CreateOpMessageModel, OpMessageModel, OpMessageService} from '../../backend_api_client';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostsService {

  constructor(private posts: OpMessageService) {
  }

  public getAllPosts(): Observable<OpMessageModel[]> {
    return this.posts.apiOpMessageGet('body');
  }

  public createPost(message: CreateOpMessageModel): Observable<OpMessageModel> {
    return this.posts.apiOpMessagePost(message, 'body');
  }
}
