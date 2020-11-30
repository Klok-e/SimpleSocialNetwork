import {Component, OnDestroy, OnInit} from '@angular/core';
import {PostsService} from '../../services/posts.service';
import {OpMessageModel, UserApiService, VoteType} from '../../../backend_api_client';
import {OpMessageUserDeleted} from '../../models/helper-types';
import {ScrollToBottomService} from '../../services/scroll-to-bottom.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.scss']
})
export class PostsComponent implements OnInit, OnDestroy {
  private subs: Subscription = new Subscription();

  opMessages: OpMessageUserDeleted[] | null = null;

  constructor(private posts: PostsService,
              private usersApi: UserApiService,
              private scrollService: ScrollToBottomService) {
  }

  ngOnInit(): void {
    this.subs.add(
      this.scrollService.user.subscribe({
        next: _ => {
          this.addPosts(this.currentPage());
          console.log('add posts');
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private currentPage(): number {
    const count = this.opMessages?.length ?? 0;
    return Math.floor(count / 5);
  }

  public addPosts(page: number): void {
    this.posts.getAllPosts(page).subscribe((next) => {
      const posts = next.map(x => {
        const post = x as OpMessageUserDeleted;
        post.posterIsDeleted = false;
        if (post.posterId != null) {
          this.usersApi.apiUserDeletedGet(post.posterId)
            .subscribe({
              next: (deleted: boolean) => {
                post.posterIsDeleted = deleted;
              }
            });
        }
        return post;
      });
      if (this.opMessages === null) {
        this.opMessages = posts;
      } else {
        this.opMessages.push(...posts);
      }
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
