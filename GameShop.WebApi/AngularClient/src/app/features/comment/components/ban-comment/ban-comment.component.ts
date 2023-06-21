import { Component, Inject, OnInit } from '@angular/core';
import { CommentService } from "../../../../core/services/commentService/comment.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { User } from "../../../../core/models/User";
import { UserService } from "../../../../core/services/userService/user.service";
import { GameCommentComponent } from "../game-comment/game-comment.component";

@Component({
  selector: 'app-ban-comment',
  templateUrl: './ban-comment.component.html',
  styleUrls: ['./ban-comment.component.css']
})
export class BanCommentComponent implements OnInit {

    form!: FormGroup;

    nickName!: string;

    constructor(
        @Inject(MAT_DIALOG_DATA) private data : {nickName: string},
        private dialogRef: MatDialogRef<GameCommentComponent>,
        private commentService: CommentService,
        private userService: UserService,
        private utilsService: UtilsService,
        private formBuilder: FormBuilder
    ) {}

    ngOnInit(): void {
        this.nickName = this.data.nickName;

        this.form = this.formBuilder.group({
            BanDuration: new FormControl('', [Validators.required])
        });
    }

    goBack(): void {
        this.dialogRef.close(true);
    }

    onBanClick(): void {
        if (this.form.invalid) {
            return;
        }

        const data: User = {
            NickName: this.nickName,
            BanOption: this.form.controls['BanDuration'].value
        };

        this.userService.banUser(data).subscribe({
            next: () => {
                this.utilsService.openWithMessage('User banned successfully');
                this.dialogRef.close(true);
            }
        });
    }
}
