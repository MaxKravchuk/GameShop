import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { Comment } from "../../../../core/models/Comment";
import { SharedCommentService } from "../../../../core/services/helpers/sharedCommentService/shared-comment.service";
import { CommentShared } from "../../../../core/models/helpers/CommentShared";
import { catchError } from "rxjs";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
    selector: 'app-create-comment',
    templateUrl: './create-comment.component.html',
    styleUrls: ['./create-comment.component.css']
})
export class CreateCommentComponent implements OnInit{

    receivedData: CommentShared = {Name: "", CommentId: undefined};

    @Input() gameKey?: string;

    form!: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private commentService: CommentService,
        private sharedService: SharedCommentService,
        private utilsService: UtilsService
    ) {}

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            Name: new FormControl("", Validators.required),
            Body: new FormControl("", Validators.required)
        });

        this.sharedService.getData().subscribe((data: CommentShared) => {
            if (this.receivedData.Name == data.Name && this.receivedData.CommentId == data.CommentId) {
                this.receivedData.Name = "";
            } else {
                this.receivedData = data;
            }
        });
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
            next: () => {
                this.sharedService.reloadComments();
            }
        });
    }
}
