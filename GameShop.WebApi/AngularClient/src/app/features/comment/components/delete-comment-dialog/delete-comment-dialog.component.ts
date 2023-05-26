import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { GameCommentComponent } from "../game-comment/game-comment.component";
import { CommentService } from "../../../../core/services/commentService/comment.service";

@Component({
  selector: 'app-delete-comment-dialog',
  templateUrl: './delete-comment-dialog.component.html',
  styleUrls: ['./delete-comment-dialog.component.css']
})
export class DeleteCommentDialogComponent implements OnInit {

    commentId!: number;
    constructor(
        @Inject(MAT_DIALOG_DATA) private data : {id: number},
        private dialogRef: MatDialogRef<GameCommentComponent>,
        private commentService: CommentService
    ) {}

    ngOnInit(): void {
        this.commentId = this.data.id;
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onDeleteClick(): void {
        this.commentService.deleteComment(this.commentId).subscribe({
            next: (): void => {
                this.dialogRef.close(true);
            }
        });
    }
}
