import {Component, Input, OnInit} from '@angular/core';
import {Comment} from "../../../../core/models/Comment";
import {SharedCommentService} from "../../../../core/services/commentService/shared/shared-comment.service";
import {CommentShared} from "../../../../core/models/Helpers/CommentShared";
import {ActivatedRoute, Router} from "@angular/router";
import {CommentService} from "../../../../core/services/commentService/comment.service";
import {combineLatestAll} from "rxjs";

@Component({
  selector: 'app-game-comment',
  templateUrl: './game-comment.component.html',
  styleUrls: ['./game-comment.component.css']
})
export class GameCommentComponent implements OnInit {

  @Input() comment! : Comment;
  @Input() replies! : Comment[];
  @Input() gameKey?: string;
  @Input() parentCommentId!:number;
  @Input() parentCommentName!:string;

  answersIsDisplayed: boolean = false;
  comments: Comment[] = [];
  trackById(index: number, item: Comment): number {
    return item.Id!; // replace with the actual identifier property of your Comment model
  }

  constructor(
    private commentService: CommentService,
    private sharedService: SharedCommentService,
    private route: Router,
    private _route: ActivatedRoute) {}

  ngOnInit(): void {
    this.sharedService.reloadComments$.subscribe(() => {
      window.location.reload();
    });
    if(this.gameKey! != null){
      this.commentService.getCommentsByGameKey(this.gameKey!).subscribe(
        (comment) => this.comments = comment
      );
    }
  }

  onAnswerButtonClick(Name: string, Id: number) {
    const model = new CommentShared(Name, Id);
    this.sharedService.sendData(model);
  }

  showAnswers() {
    this.answersIsDisplayed = !this.answersIsDisplayed;
  }

  goToParentComment(Id:number) {
    this.route.navigate([],{relativeTo: this._route, fragment: `comment-${Id}`}).then(() =>{
      const parent = document.getElementById(`comment-${Id}`);
      if (parent){
        parent.scrollIntoView({behavior:'smooth'});
      }
    });
  }

  getReplies(id: number){
    return this.comments.filter(x=>x.ParentId==id);
  }
}
