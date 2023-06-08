import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { Comment } from "../../../../core/models/Comment";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { forkJoin, Subscription } from "rxjs";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { Game } from "../../../../core/models/Game";
import { AuthService } from "../../../../core/services/authService/auth.service";
import { User } from "../../../../core/models/User";
import { UserService } from "../../../../core/services/userService/user.service";

@Component({
    selector: 'app-create-comment',
    templateUrl: './create-comment.component.html',
    styleUrls: ['./create-comment.component.css']
})
export class CreateCommentComponent implements OnInit, OnDestroy {

    @Input() game!: Game;

    parentComment?: Comment = undefined;

    action: string = 'new';

    form!: FormGroup;

    getCommentActionSubscription: Subscription = new Subscription();

    IsCommentable: boolean = true;

    user: User = {};

    isBanned?: boolean;

    constructor(
        private formBuilder: FormBuilder,
        private commentService: CommentService,
        private sharedService: SharedService<{ action: string, parentComment: Comment }>,
        private utilsService: UtilsService,
        private authService: AuthService,
        private userService: UserService
    ) {}

    ngOnInit(): void {

        this.user.Id = this.authService.getUserId();
        this.user.NickName = this.authService.getUserName();

        this.userService.IsAnExistingUserBannedAsync(this.user.NickName!).subscribe(
            (data: boolean): void => {
                this.isBanned = data;
                if (data) {
                    this.form.controls['Body'].disable();
                    this.form.controls['Body'].setValue('You are banned from commenting');
                }
            }
        );

        this.form = this.formBuilder.group({
            Name: [{value: this.user.NickName, disabled: true}, Validators.required],
            Body: ['', Validators.required]
        });

        if (this.authService.isInRole('User') && this.game.IsDeleted!) {
            this.IsCommentable = false;
        }

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
            Name: this.user.NickName,
            Body: this.form.controls['Body'].value,
            GameKey: this.game.Key!,
            ParentId: this.parentComment?.Id,
            HasQuotation: this.action === 'quote'
        } as Comment;

        this.commentService.createComment(data).subscribe({
            next: (): void => {
                this.form.reset();
                this.sharedService.reloadSource();
            }
        });

        this.action = 'new';
        this.parentComment = undefined;
    }
}
