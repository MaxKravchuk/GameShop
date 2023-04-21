import {Component, Input, OnInit} from '@angular/core';
import {CommentService} from "../../../../core/services/commentService/comment.service";
import {Comment} from "../../../../core/models/Comment";
import {SharedCommentService} from "../../../../core/services/commentService/shared/shared-comment.service";

@Component({
  selector: 'app-game-comment',
  templateUrl: './game-comment.component.html',
  styleUrls: ['./game-comment.component.css']
})
export class GameCommentComponent implements OnInit {

  comments : Comment[] = [];
  @Input() gameKey?: string;

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
    this.commentService.getCommentsByGameKey(this.gameKey!).subscribe((data)=> {
      this.comments = data;
    });
  }
  onAnswerButtonClick(Name?: string) {
    this.sharedService.sendData(Name!);
  }
}
