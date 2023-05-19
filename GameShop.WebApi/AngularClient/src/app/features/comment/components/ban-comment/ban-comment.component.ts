import { Component } from '@angular/core';
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
  selector: 'app-ban-comment',
  templateUrl: './ban-comment.component.html',
  styleUrls: ['./ban-comment.component.css']
})
export class BanCommentComponent {
    constructor(
        private commentService: CommentService,
        private utilsService: UtilsService
    ) {}
    handleClick($event: any): void {
        this.commentService.banComment($event.value).subscribe(
            (): void => {
                this.utilsService.openWithMessage('Comment was banned successfully!');
            }
        );
    }
    goBack(): void {
        this.utilsService.goBack();
    }
}
