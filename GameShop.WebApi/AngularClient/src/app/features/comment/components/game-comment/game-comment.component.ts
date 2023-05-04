import { Component, Input, OnInit } from '@angular/core';
import { Comment } from "../../../../core/models/Comment";
import { SharedCommentService } from "../../../../core/services/helpers/sharedCommentService/shared-comment.service";
import { CommentShared } from "../../../../core/models/helpers/CommentShared";
import { ActivatedRoute, Router } from "@angular/router";
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { catchError } from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

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
        private _route: ActivatedRoute,
        private utilsService: UtilsService
    ) {
    }

    ngOnInit(): void {
        this.sharedService.reloadComments$.subscribe({
            next: () =>{
                this.utilsService.goBack();
            }
        });
        if (this.gameKey! != null) {
            this.getCommentsByGameKey(this.gameKey!);
        }
    }

    onAnswerButtonClick(Name: string, Id: number): void {
        const model: CommentShared = { Name: Name, CommentId: Id };
        this.sharedService.sendData(model);
    }

    showAnswers(): void {
        this.answersIsDisplayed = !this.answersIsDisplayed;
    }

    goToParentComment(Id: number): void {
        this.route.navigate([], {relativeTo: this._route, fragment: `comment-${Id}`}).then(() => {
            const parent = document.getElementById(`comment-${Id}`);
            if (parent) {
                parent.scrollIntoView({behavior: 'smooth'});
            }
        });
    }

    getReplies(id: number): Comment[] {
        return this.comments.filter((comment: Comment) => comment.ParentId == id);
    }

    private getCommentsByGameKey(gameKey: string): void {
        this.commentService.getCommentsByGameKey(this.gameKey!).subscribe(
            (comment: Comment[]) => this.comments = comment
        );
    }
}
