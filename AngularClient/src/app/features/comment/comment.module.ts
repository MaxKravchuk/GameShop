import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameCommentComponent } from './components/game-comment/game-comment.component';
import {MatButtonModule} from "@angular/material/button";
import {RouterLink} from "@angular/router";



@NgModule({
  declarations: [
    GameCommentComponent
  ],
  exports:[
    GameCommentComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    RouterLink
  ]
})
export class CommentModule { }
