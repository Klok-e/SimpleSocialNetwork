import {Component, OnInit} from '@angular/core';
import {OpMessageModel} from '../../../backend_api_client';
import {ActivatedRoute, Router} from '@angular/router';
import {PostsService} from '../../services/posts.service';

@Component({
  selector: 'app-read-post',
  templateUrl: './read-post.component.html',
  styleUrls: ['./read-post.component.scss']
})
export class ReadPostComponent implements OnInit {
  post: OpMessageModel | null = null;

  constructor(private route: ActivatedRoute,
              private posts: PostsService) {
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id !== null) {
      const postId = +id;
      this.posts.getPost(postId).subscribe(post => {
        this.post = post;
      });
    }
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

  public paragraphs(str: string): string[] {
    return str.split('\n');
  }
}
