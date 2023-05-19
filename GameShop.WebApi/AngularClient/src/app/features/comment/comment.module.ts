import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameCommentComponent } from './components/game-comment/game-comment.component';
import { MatButtonModule } from "@angular/material/button";
import { RouterLink } from "@angular/router";
import { CreateCommentComponent } from './components/create-comment/create-comment.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { CommentsListComponent } from './components/comments-list/comments-list.component';
import { RepliesPipe } from "../../core/pipes/replies.pipe";
import { DeleteCommentDialogComponent } from './components/delete-comment-dialog/delete-comment-dialog.component';
import { MatDialogModule } from "@angular/material/dialog";
import { BanCommentComponent } from './components/ban-comment/ban-comment.component';
import { MatRadioModule } from "@angular/material/radio";
import { CommentRoutingModule } from "./comment-routing.module";


@NgModule({
    declarations: [
        GameCommentComponent,
        CreateCommentComponent,
        CommentsListComponent,
        RepliesPipe,
        DeleteCommentDialogComponent,
        BanCommentComponent
    ],
    exports: [
        GameCommentComponent,
        CreateCommentComponent,
        CommentsListComponent,
        RepliesPipe
    ],
    imports: [
        CommonModule,
        MatButtonModule,
        RouterLink,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        MatDialogModule,
        MatRadioModule,
        CommentRoutingModule
    ]
})
export class CommentModule {
}
