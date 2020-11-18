import {Injectable} from '@angular/core';
import {CommentModel, CreateOpMessageModel, OpMessageModel, OpMessageApiService, VotePost} from '../../backend_api_client';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostsService {

  constructor(private posts: OpMessageApiService) {
  }

  public getAllPosts(): Observable<OpMessageModel[]> {
    return this.posts.apiOpMessageGet('body');
  }

  public getPost(id: number): Observable<OpMessageModel> {
    return this.posts.apiOpMessageIdGet(id, 'body');
  }

  public createPost(message: CreateOpMessageModel): Observable<number> {
    return this.posts.apiOpMessagePost(message, 'body');
  }

  public getComments(postId: number): Observable<CommentModel[]> {
    return this.posts.apiOpMessageCommentsPostIdGet(postId, 'body');
  }

  public postExists(postId: number): Observable<boolean> {
    return this.posts.apiOpMessageExistsPostIdGet(postId);
  }

  public votePost(votePost: VotePost): Observable<any> {
    return this.posts.apiOpMessageVotePost(votePost);
  }
}
