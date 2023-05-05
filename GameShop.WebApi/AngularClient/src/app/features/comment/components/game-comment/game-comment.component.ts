import { Component, Input, OnInit } from '@angular/core';
import { Comment } from "../../../../core/models/Comment";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { CommentShared } from "../../../../core/models/helpers/CommentShared";
import { CommentService } from "../../../../core/services/commentService/comment.service";

@Component({
    selector: 'app-game-comment',
    templateUrl: './game-comment.component.html',
    styleUrls: ['./game-comment.component.css']
})
export class GameCommentComponent implements OnInit {

    @Input() comment!: Comment;

    @Input() replies!: Comment[];

    @Input() gameKey?: string;

    @Input() parentCommentId!: number;

    @Input() parentCommentName!: string;

    answersIsDisplayed: boolean = false;

    comments: Comment[] = [];

    constructor(
        private commentService: CommentService,
        private sharedService: SharedService<CommentShared>
    ) {}

    ngOnInit(): void {
        if (this.gameKey! != null) {
            this.getCommentsByGameKey(this.gameKey!);
        }
    }

    onAnswerButtonClick(Name: string, Id: number): void {
        const model: CommentShared = {Name: Name, CommentId: Id};
        this.sharedService.sendData(model);
    }

    showAnswers(): void {
        this.answersIsDisplayed = !this.answersIsDisplayed;
    }

    goToParentComment(Id: number): void {
        const parent: HTMLElement | null = document.getElementById(`comment-${Id}`);
        if (parent) {
            parent.scrollIntoView({behavior: 'smooth'});
        }
    }

    private getCommentsByGameKey(gameKey: string): void {
        this.commentService.getCommentsByGameKey(this.gameKey!).subscribe(
            (comment: Comment[]) => this.comments = comment
        );
    }
}
