import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {PostsService} from '../../services/posts.service';
import {map, mergeMap} from 'rxjs/operators';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss']
})
export class CreatePostComponent implements OnInit {
  createPostForm: FormGroup;

  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private posts: PostsService) {
    this.createPostForm = formBuilder.group({
      title: new FormControl('', [
        Validators.required
      ]),
      content: new FormControl('')
    });
  }

  ngOnInit(): void {
  }

  public onSubmit(): void {
    if (!this.createPostForm.valid) {
      this.createPostForm.markAllAsTouched();
      return;
    }

    this.posts.createPost({
      content: this.createPostForm.value.content as string,
      title: this.createPostForm.value.title as string,
      tags: []
    }).pipe(
      mergeMap(postId => {
        return this.router.navigate([`posts/${postId}`]);
      })
    ).subscribe();
  }
}
