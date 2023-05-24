import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { Comment } from "../../../../core/models/Comment";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { Subscription } from "rxjs";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
    selector: 'app-create-comment',
    templateUrl: './create-comment.component.html',
    styleUrls: ['./create-comment.component.css']
})
export class CreateCommentComponent implements OnInit, OnDestroy {

    @Input() gameKey?: string;

    parentComment?: Comment = undefined;

    action: string = 'new';

    form!: FormGroup;

    getCommentActionSubscription: Subscription = new Subscription();

    constructor(
        private formBuilder: FormBuilder,
        private commentService: CommentService,
        private sharedService: SharedService<{ action: string, parentComment: Comment }>,
        private utilsService: UtilsService
    ) {}

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            Name: ['', Validators.required],
            Body: ['', Validators.required]
        });

        this.getCommentActionSubscription = this.sharedService.getData$().subscribe({
            next: (data: { action: string, parentComment: Comment }): void => {
                if (this.action === data['action'] && this.parentComment === data['parentComment']) {
                    this.action = 'new';
                    this.parentComment = undefined;
                    return;
                }
                this.action = data['action'];
                this.parentComment = data['parentComment'];
            }
        });
    }

    ngOnDestroy(): void {
        this.getCommentActionSubscription.unsubscribe();
    }

    onSaveForm(): void {
        if (!this.form.valid) {
            this.utilsService.openWithMessage('Please fill all the fields');
        }

        const data: Comment = {
            ...this.form.value,
            GameKey: this.gameKey,
            ParentId: this.parentComment?.Id,
            HasQuotation: this.action === 'quote'
        } as Comment;

        this.commentService.createComment(data).subscribe({
            next: (): void => {
                this.form.reset();
                this.sharedService.reloadSource();
            }
        });
    }
}
