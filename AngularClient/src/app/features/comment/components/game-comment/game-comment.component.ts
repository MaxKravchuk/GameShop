import {Component, Input, OnInit} from '@angular/core';
import {CommentService} from "../../commentService/comment.service";
import {Comment} from "../../../../core/models/Comment";

@Component({
  selector: 'app-game-comment',
  templateUrl: './game-comment.component.html',
  styleUrls: ['./game-comment.component.css']
})
export class GameCommentComponent implements OnInit {

  comments : Comment[] = [];
  @Input() gameKey?: string;
  constructor(
    private commentService: CommentService)
  { }

  ngOnInit(): void {
    this.updateList();
  }

  private updateList():void{
    this.commentService.getCommentsByGameKey(this.gameKey!).subscribe((data)=> {
      this.comments = data;
    });
  }

}
