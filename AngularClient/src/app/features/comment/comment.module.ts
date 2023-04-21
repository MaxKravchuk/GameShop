import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameCommentComponent } from './components/game-comment/game-comment.component';
import {MatButtonModule} from "@angular/material/button";
import {RouterLink} from "@angular/router";
import { CreateCommentComponent } from './components/create-comment/create-comment.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";



@NgModule({
  declarations: [
    GameCommentComponent,
    CreateCommentComponent
  ],
  exports: [
    GameCommentComponent,
    CreateCommentComponent
  ],
    imports: [
        CommonModule,
        MatButtonModule,
        RouterLink,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule
    ]
})
export class CommentModule { }
