import {Component, OnInit} from '@angular/core';
import {CommentModel, CommentService, OpMessageModel} from '../../../backend_api_client';
import {ActivatedRoute, Router} from '@angular/router';
import {PostsService} from '../../services/posts.service';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {map, mergeMap} from 'rxjs/operators';
import {EMPTY, Observable} from 'rxjs';

@Component({
  selector: 'app-read-post',
  templateUrl: './read-post.component.html',
  styleUrls: ['./read-post.component.scss']
})
export class ReadPostComponent implements OnInit {
  post: OpMessageModel | null = null;
  comments: CommentModel[] = [];

  commentForm: FormGroup;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private posts: PostsService,
              private commentService: CommentService,
              private formBuilder: FormBuilder) {
    this.commentForm = formBuilder.group({
      content: new FormControl('', [
        Validators.required
      ])
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id !== null) {
      const postId = +id;
      this.posts.getPost(postId).pipe(
        mergeMap(post => {
          this.post = post;
          return this.updateComments(postId);
        })
      ).subscribe({
        error: _ => {
          this.router.navigate(['']);
        }
      });
    }
  }

  private updateComments(postId: number): Observable<void> {
    return this.posts.getComments(postId).pipe(
      map(comments => {
        this.comments = comments;
      }));
  }

  public upvote(): void {
    if (this.post !== null) {
      this.post.points += 1;
    }
  }

  public downvote(): void {
    if (this.post !== null) {
      this.post.points -= 1;
    }
  }

  public upvoteComment(id: number): void {
    const comment = this.comments.find((x) => {
      return x.messageId === id;
    });
    if (comment !== undefined) {
      comment.points += 1;
    }
  }

  public downvoteComment(id: number): void {
    const comment = this.comments.find((x) => {
      return x.messageId === id;
    });
    if (comment !== undefined) {
      comment.points -= 1;
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

    this.commentService.apiCommentPost({
      content: this.commentForm.value.content,
      opId: this.post.id,
    }).pipe(
      mergeMap(_ => {
        if (this.post !== null) {
          return this.updateComments(this.post.id);
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
