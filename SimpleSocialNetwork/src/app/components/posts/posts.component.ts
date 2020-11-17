import {Component, OnInit} from '@angular/core';
import {PostsService} from '../../services/posts.service';
import {OpMessageModel, VoteType} from '../../../backend_api_client';

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
      this.posts.votePost({postId, voteType: VoteType.NUMBER_1})
        .subscribe();
    }
  }

  public downvote(postId: number): void {
    const post = this.opMessages?.find((x) => {
      return x.id === postId;
    });
    if (typeof post?.points === 'number') {
      post.points -= 1;
      this.posts.votePost({postId, voteType: VoteType.NUMBER_2})
        .subscribe();
    }
  }

  public paragraphs(str: string): { pars: { p: string, last: boolean }[], wasTruncated: boolean } {
    let wasTruncated = false;
    let pars: string[];
    if (str.length > 500) {
      wasTruncated = true;
      pars = str.slice(0, 500).split('\n');
    } else {
      pars = str.split('\n');
    }

    return {
      pars: pars.map((x: string, ind: number) => {
        return {
          p: x,
          last: ind === pars.length - 1
        };
      }),
      wasTruncated
    };
  }
}
