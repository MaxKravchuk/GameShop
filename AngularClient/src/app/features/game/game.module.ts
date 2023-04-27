import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {GameDetailsComponent} from "./components/game-details/game-details.component";
import {GameRoutingModule} from "./game-routing.module";
import { GameCreateComponent } from './components/game-create/game-create.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatButtonModule} from "@angular/material/button";
import {MatDialogModule} from "@angular/material/dialog";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatSelectModule} from "@angular/material/select";
import {MatInputModule} from "@angular/material/input";
import {CommentModule} from "../comment/comment.module";
import {MatSnackBarModule} from "@angular/material/snack-bar";
@NgModule({
  declarations: [
    GameDetailsComponent,
    GameCreateComponent,
  ],
  imports: [
    CommonModule,
    GameRoutingModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatInputModule,
    CommentModule,
    MatSnackBarModule
  ]
})
export class GameModule { }
