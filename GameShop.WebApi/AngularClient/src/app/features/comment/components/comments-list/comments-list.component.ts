import { Component, Input, OnInit } from '@angular/core';
import { Comment } from "../../../../core/models/Comment";
import { CommentService } from "../../../../core/services/commentService/comment.service";

@Component({
    selector: 'app-comments-list',
    templateUrl: './comments-list.component.html',
    styleUrls: ['./comments-list.component.css']
})
export class CommentsListComponent implements OnInit {

    @Input() gameKey!: string;

    comments: Comment[] = [];

    constructor(private commentService: CommentService) {
    }

    ngOnInit(): void {
        this.getComments();
    }

    getRootComments(): Comment[] {
        return this.comments.filter((comment: Comment) => comment.ParentId == null);
    }

    getReplies(commentId: number): Comment[] {
        return this.comments.filter((comment: Comment) => comment.ParentId == commentId);
    }

    private getComments(): void {
        this.commentService.getCommentsByGameKey(this.gameKey).subscribe(
            (comments: Comment[]) => this.comments = comments
        );
    }
}
