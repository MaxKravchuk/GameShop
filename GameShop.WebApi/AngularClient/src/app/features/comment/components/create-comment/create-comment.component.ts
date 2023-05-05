import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { Comment } from "../../../../core/models/Comment";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { CommentShared } from "../../../../core/models/helpers/CommentShared";
import { Subscription } from "rxjs";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
    selector: 'app-create-comment',
    templateUrl: './create-comment.component.html',
    styleUrls: ['./create-comment.component.css']
})
export class CreateCommentComponent implements OnInit, OnDestroy {

    @Input() gameKey?: string;

    receivedData: CommentShared = {Name: ''};

    form!: FormGroup;

    private receivedDataSub: Subscription = new Subscription();

    constructor(
        private formBuilder: FormBuilder,
        private commentService: CommentService,
        private sharedService: SharedService<CommentShared>,
        private utilsService: UtilsService
    ) {}

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            Name: new FormControl("", Validators.required),
            Body: new FormControl("", Validators.required)
        });

        this.receivedDataSub = this.sharedService.getData$().subscribe((data: CommentShared): void => {
            if (this.receivedData.Name == data.Name && this.receivedData.CommentId == data.CommentId) {
                this.receivedData.Name = "";
            } else {
                this.receivedData = data;
            }
        });
    }

    ngOnDestroy(): void {
        this.receivedDataSub.unsubscribe();
    }

    onSaveForm(): void {
        if (!this.form.valid) {
            this.utilsService.openWithMessage("Please fill all the fields");
        }

        const data: Comment = {
            ...this.form.value,
            GameKey: this.gameKey,
            ParentId: this.receivedData.CommentId
        } as Comment;

        this.commentService.createComment(data).subscribe({
            next: (): void => {
                this.sharedService.reloadSource();
            }
        });
    }
}
