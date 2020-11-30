import {Injectable} from '@angular/core';
import {CommentModel, CreateOpMessageModel, OpMessageModel, OpMessageApiService, VotePost} from '../../backend_api_client';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostsService {

  constructor(private posts: OpMessageApiService) {
  }

  public getAllPosts(page: number): Observable<OpMessageModel[]> {
    return this.posts.apiOpMessageGet(page);
  }

  public getPost(id: number): Observable<OpMessageModel> {
    return this.posts.apiOpMessageIdGet(id);
  }

  public createPost(message: CreateOpMessageModel): Observable<number> {
    return this.posts.apiOpMessagePost(message);
  }

  public getComments(postId: number, page: number): Observable<CommentModel[]> {
    return this.posts.apiOpMessageCommentsPostIdPageGet(postId, page);
  }

  public postExists(postId: number): Observable<boolean> {
    return this.posts.apiOpMessageExistsPostIdGet(postId);
  }

  public votePost(votePost: VotePost): Observable<void> {
    return this.posts.apiOpMessageVotePost(votePost);
  }
}
