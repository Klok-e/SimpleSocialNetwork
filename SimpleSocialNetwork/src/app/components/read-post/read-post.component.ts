import {Component, OnDestroy, OnInit} from '@angular/core';
import {CommentModel, CommentApiService, OpMessageModel, VoteType, UserApiService, OpMessageApiService} from '../../../backend_api_client';
import {ActivatedRoute, Router} from '@angular/router';
import {PostsService} from '../../services/posts.service';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {map, mergeMap} from 'rxjs/operators';
import {EMPTY, Observable, Subscription, throwError} from 'rxjs';
import {CommentUserDeleted, OpMessageUserDeleted} from '../../models/helper-types';
import {AuthService} from '../../services/auth.service';
import {ScrollToBottomService} from '../../services/scroll-to-bottom.service';


@Component({
  selector: 'app-read-post',
  templateUrl: './read-post.component.html',
  styleUrls: ['./read-post.component.scss']
})
export class ReadPostComponent implements OnInit, OnDestroy {
  post: OpMessageUserDeleted | null = null;
  comments: CommentUserDeleted[] = [];

  commentForm: FormGroup;

  subs: Subscription = new Subscription();

  constructor(private route: ActivatedRoute,
              private router: Router,
              private posts: PostsService,
              private postsApi: OpMessageApiService,
              private commentApi: CommentApiService,
              private usersApi: UserApiService,
              private formBuilder: FormBuilder,
              public auth: AuthService,
              private scroll: ScrollToBottomService) {
    this.commentForm = formBuilder.group({
      content: new FormControl('', [
        Validators.required
      ])
    });
  }


  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id === null) {
      this.navigateTo404();
      return;
    }
    const postId = +id;
    if (isNaN(postId)) {
      this.navigateTo404();
      return;
    }
    this.subs.add(
      this.posts.postExists(postId).pipe(
        mergeMap(exists => {
          if (!exists) {
            this.navigateTo404();
            return throwError(new Error('Post doesn\'t exist'));
          }
          return this.posts.getPost(postId);
        }),
        mergeMap(x => {
          const post1 = x as OpMessageUserDeleted;
          post1.posterIsDeleted = false;
          if (post1.posterId != null) {
            this.usersApi.apiUserDeletedGet(post1.posterId)
              .subscribe({
                next: deleted => {
                  post1.posterIsDeleted = deleted;
                }
              });
          }
          this.post = post1;

          return this.scroll.user.pipe(
            mergeMap(_ => {
              return this.updateComments(postId, this.currentCommentPage());
            })
          );
        })
      ).subscribe({
        error: e => {
          console.log(e);
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private navigateTo404(): void {
    this.router.navigate(['404-route-please-match-this-really-long-route'], {skipLocationChange: true});
  }

  private currentCommentPage(): number {
    const count = this.comments.length;
    return Math.floor(count / 5);
  }

  private updateComments(postId: number, page: number): Observable<void> {
    const currPg = this.currentCommentPage();
    return this.posts.getComments(postId, page).pipe(
      map(comments => {
          if (page < currPg) {
            this.comments = [];
          }
          this.comments.push(...comments.map(x => {
              const comment = x as CommentUserDeleted;
              comment.commenterIsDeleted = false;
              if (comment.posterId != null) {
                this.usersApi.apiUserDeletedGet(comment.posterId)
                  .subscribe({
                    next: deleted => {
                      comment.commenterIsDeleted = deleted;
                    }
                  });
              }

              return comment;
            })
          );
        }
      )
    );
  }

  public upvote(): void {
    if (this.post !== null) {
      this.post.points += 1;
      this.posts.votePost({postId: this.post.id, voteType: VoteType.NUMBER_1})
        .subscribe();
    }
  }

  public downvote(): void {
    if (this.post !== null) {
      this.post.points -= 1;
      this.posts.votePost({postId: this.post.id, voteType: VoteType.NUMBER_2})
        .subscribe();
    }
  }

  deletePost(): void {
    if (this.post === null) {
      return;
    }
    this.postsApi.apiOpMessageDelete(this.post.id).subscribe({
      next: _ => {
        this.router.navigate(['']);
      }
    });
  }

  deleteComment(id: number): void {
    const comment = this.comments.find((x) => {
      return x.messageId === id;
    });
    if (comment === undefined) {
      return;
    }
    // console.log('delete', comment);
    this.commentApi.apiCommentDelete(comment.opId, comment.messageId).pipe(
      mergeMap(_ => {
          // console.log('update');
          if (this.post === null) {
            return EMPTY;
          }
          return this.updateComments(this.post.id, 0);
        }
      )
    ).subscribe();
  }

  public upvoteComment(id: number): void {
    const comment = this.comments.find((x) => {
      return x.messageId === id;
    });
    if (comment !== undefined) {
      comment.points += 1;
      this.commentApi.apiCommentVotePost({
        commentId: {
          messageId: comment.messageId,
          opId: comment.opId,
        },
        voteType: VoteType.NUMBER_1
      }).subscribe();
    }
  }

  public downvoteComment(id: number): void {
    const comment = this.comments.find((x) => {
      return x.messageId === id;
    });
    if (comment !== undefined) {
      comment.points -= 1;
      this.commentApi.apiCommentVotePost({
        commentId: {
          messageId: comment.messageId,
          opId: comment.opId,
        },
        voteType: VoteType.NUMBER_2
      }).subscribe();
    }
  }

  public paragraphs(str: string): string[] {
    return str.split('\n');
  }

  public async sendComment(): Promise<void> {
    if (this.post === null) {
      return;
    }
    if (this.commentForm.invalid) {
      this.commentForm.markAllAsTouched();
      return;
    }

    this.commentApi.apiCommentPost({
      content: this.commentForm.value.content,
      opId: this.post.id,
    }).pipe(
      mergeMap(_ => {
        if (this.post !== null) {
          return this.updateComments(this.post.id, 0);
        }
        return EMPTY;
      })
    ).subscribe(_ => {
      this.commentForm.setValue({
        content: ''
      });
      this.commentForm.markAsPristine();
      this.commentForm.markAsUntouched();
    });
  }
}
