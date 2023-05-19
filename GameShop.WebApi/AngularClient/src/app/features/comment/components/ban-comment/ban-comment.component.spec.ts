import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BanCommentComponent } from './ban-comment.component';

describe('BanCommentComponent', () => {
  let component: BanCommentComponent;
  let fixture: ComponentFixture<BanCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BanCommentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BanCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
