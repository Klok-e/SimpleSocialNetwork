import {Component, OnInit} from '@angular/core';
import {PostsService} from '../../services/posts.service';
import {OpMessageModel} from '../../../backend_api_client';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.scss']
})
export class PostsComponent implements OnInit {
  opMessages: OpMessageModel[] | null = null;

  constructor(private posts: PostsService) {
  }

  ngOnInit(): void {
    this.updatePostList();
  }

  public updatePostList(): void {
    this.posts.getAllPosts().subscribe((next) => {
      this.opMessages = next;
    });
  }

  public upvote(postId: number): void {
    const post = this.opMessages?.find((x) => {
      return x.id === postId;
    });
    if (typeof post?.points === 'number') {
      post.points += 1;
    }
  }

  public downvote(postId: number): void {
    const post = this.opMessages?.find((x) => {
      return x.id === postId;
    });
    if (typeof post?.points === 'number') {
      post.points -= 1;
    }
  }

  public paragraphs(str: string): string[] {
    return str.split('\n');
  }
}
