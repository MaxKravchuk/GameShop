import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameCommentComponent } from './game-comment.component';

describe('GameCommentComponent', () => {
  let component: GameCommentComponent;
  let fixture: ComponentFixture<GameCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GameCommentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
