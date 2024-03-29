import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Comment } from "../../../../core/models/Comment";
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { Subscription } from "rxjs";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { Game } from "../../../../core/models/Game";

@Component({
    selector: 'app-comments-list',
    templateUrl: './comments-list.component.html',
    styleUrls: ['./comments-list.component.scss']
})
export class CommentsListComponent implements OnInit, OnDestroy {

    @Input() game!: Game;

    comments: Comment[] = [];

    rootComments: Comment[] = [];

    private reloadCommentsSub: Subscription = new Subscription();

    constructor(
        private commentService: CommentService,
        private sharedService: SharedService<boolean>
    ) {}

    ngOnInit(): void {
        this.reloadCommentsSub = this.sharedService.reloadSource$.subscribe({
            next: (): void => {
                this.getComments();
            }
        });
        this.getComments();
    }

    ngOnDestroy(): void {
        this.reloadCommentsSub.unsubscribe();
    }

    private getComments(): void {
        this.commentService.getCommentsByGameKey(this.game.Key!).subscribe(
            (comments: Comment[]): void => {
                this.comments = comments;
                this.rootComments = this.getRootComments();
            }
        );
    }

    private getRootComments(): Comment[] {
        return this.comments.filter((comment: Comment): boolean => comment.ParentId == null);
    }
}
