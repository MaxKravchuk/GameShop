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

    rootComments: Comment[] = [];

    constructor(private commentService: CommentService) {
    }

    ngOnInit(): void {
        this.getComments();
    }

    private getComments(): void {
        this.commentService.getCommentsByGameKey(this.gameKey).subscribe(
            (comments: Comment[]) => {
                this.comments = comments;
                this.rootComments = this.getRootComments();
            }
        );
    }

    private getRootComments(): Comment[] {
        return this.comments.filter((comment: Comment) => comment.ParentId == null);
    }
}
