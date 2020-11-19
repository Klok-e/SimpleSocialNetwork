import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileUserPostsComponent } from './profile-user-posts.component';

describe('ProfileUserPostsComponent', () => {
  let component: ProfileUserPostsComponent;
  let fixture: ComponentFixture<ProfileUserPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfileUserPostsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileUserPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
