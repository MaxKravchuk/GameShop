import {Component, Input, OnInit} from '@angular/core';
import {CommentService} from "../../../../core/services/commentService/comment.service";
import {Comment} from "../../../../core/models/Comment";
import {SharedCommentService} from "../../../../core/services/commentService/shared/shared-comment.service";
import {CommentShared} from "../../../../core/models/Helpers/CommentShared";

@Component({
  selector: 'app-game-comment',
  templateUrl: './game-comment.component.html',
  styleUrls: ['./game-comment.component.css']
})
export class GameCommentComponent implements OnInit {

  comments : Comment[] = [];
  answersIsDisplayed: boolean = false;
  @Input() gameKey?: string;
  @Input() commentId?: number;

  constructor(
    private commentService: CommentService,
    private sharedService: SharedCommentService)
  {
  }

  ngOnInit(): void {
    this.updateList();
    this.sharedService.reloadComments$.subscribe(() => {
      window.location.reload();
    });
  }

  private updateList():void{
    if (this.commentId != 0){
      this.commentService.getAllAnswers(this.commentId!).subscribe((data) => {
        this.comments = data;
      });
      return;
    }
    this.commentService.getCommentsByGameKey(this.gameKey!).subscribe((data)=> {
      this.comments = data;
    });
  }
  onAnswerButtonClick(Name: string, Id: number) {
    const model = new CommentShared(Name, Id);
    this.sharedService.sendData(model);
  }

  showAnswers() {
    if (this.answersIsDisplayed){
      this.answersIsDisplayed = false;
      return;
    }
    this.answersIsDisplayed = true;
  }
}
