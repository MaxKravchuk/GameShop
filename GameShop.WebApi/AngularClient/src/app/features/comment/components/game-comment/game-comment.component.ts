import { Component, Input, OnInit, Output } from '@angular/core';
import { Comment } from "../../../../core/models/Comment";
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { MatDialog } from "@angular/material/dialog";
import { DeleteCommentDialogComponent } from "../delete-comment-dialog/delete-comment-dialog.component";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";

@Component({
    selector: 'app-game-comment',
    templateUrl: './game-comment.component.html',
    styleUrls: ['./game-comment.component.css']
})
export class GameCommentComponent implements OnInit {

    @Input() comment!: Comment;

    @Input() replies!: Comment[];

    @Input() gameKey?: string;

    @Input() parentComment!: Comment;

    answersIsDisplayed: boolean = false;

    comments: Comment[] = [];

    action: string = '';

    isFormOpen: boolean = false;

    constructor(
        private commentService: CommentService,
        private dialog: MatDialog,
        private sharedService: SharedService<{ action: string, parentComment: Comment }>
    ) {}

    ngOnInit(): void {
        if (this.gameKey! != null) {
            this.getCommentsByGameKey(this.gameKey!);
        }
    }

    onAnswerButtonClick(): void {
        this.sharedService.sendData({action: 'answer',parentComment: this.comment});
        //this.isFormOpen = !this.isFormOpen;
        //this.action = 'answer';
    }

    onQuoteButtonClick(): void {
        this.sharedService.sendData({action: 'quote',parentComment: this.comment});
        // if (!this.isFormOpen) {
        //     this.isFormOpen = !this.isFormOpen;
        // }
        // this.action = 'quote';
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

    deleteComment(): void {
        const dialogRef = this.dialog.open(DeleteCommentDialogComponent, {
            autoFocus: false,
            data: {
                id: this.comment.Id
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean)=> {
            if(requireReload) {
                this.sharedService.reloadSource();
            }
        });
    }

    private getCommentsByGameKey(gameKey: string): void {
        this.commentService.getCommentsByGameKey(gameKey).subscribe(
            (comment: Comment[]) => this.comments = comment
        );
    }
}
