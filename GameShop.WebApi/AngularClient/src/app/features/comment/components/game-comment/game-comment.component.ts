import { Component, Input, OnInit } from '@angular/core';
import { Comment } from "../../../../core/models/Comment";
import { SharedCommentService } from "../../../../core/services/helpers/sharedCommentService/shared-comment.service";
import { CommentShared } from "../../../../core/models/helpers/CommentShared";
import { ActivatedRoute, Router } from "@angular/router";
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { catchError } from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";

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
        private sharedService: SharedCommentService,
        private route: Router,
        private _route: ActivatedRoute
    ) {
    }

    ngOnInit(): void {
        this.sharedService.reloadComments$.subscribe(() => {
            window.location.reload();
        });
        if (this.gameKey! != null) {
            this.getCommentsByGameKey(this.gameKey!);
        }
    }

    public onAnswerButtonClick(Name: string, Id: number): void {
        const model: CommentShared = {Name: Name, CommentId: Id};
        this.sharedService.sendData(model);
    }

    public showAnswers(): void {
        this.answersIsDisplayed = !this.answersIsDisplayed;
    }

    public goToParentComment(Id: number): void {
        this.route.navigate([], {relativeTo: this._route, fragment: `comment-${Id}`}).then(() => {
            const parent = document.getElementById(`comment-${Id}`);
            if (parent) {
                parent.scrollIntoView({behavior: 'smooth'});
            }
        });
    }

    public getReplies(id: number): Comment[] {
        return this.comments.filter(x => x.ParentId == id);
    }

    private getCommentsByGameKey(gameKey: string): void {
        this.commentService.getCommentsByGameKey(this.gameKey!).subscribe(
            (comment) => this.comments = comment
        );
    }
}
