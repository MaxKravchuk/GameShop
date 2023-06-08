import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameDetailsComponent } from "./components/game-details/game-details.component";
import { GameRoutingModule } from "./game-routing.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatDialogModule } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatSelectModule } from "@angular/material/select";
import { MatInputModule } from "@angular/material/input";
import { CommentModule } from "../comment/comment.module";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { GameListComponent } from './components/game-list/game-list.component';
import { GameFiltersComponent } from './components/game-filters/game-filters.component';
import { MatExpansionModule } from "@angular/material/expansion";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatSliderModule } from "@angular/material/slider";
import { MatRadioModule } from "@angular/material/radio";
import { MatPaginatorModule } from "@angular/material/paginator";
import { NgSelectModule } from "@ng-select/ng-select";

@NgModule({
    declarations: [
        GameDetailsComponent,
        GameListComponent,
        GameFiltersComponent,
    ],
    exports: [
        GameListComponent,
        GameFiltersComponent,
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
        MatSnackBarModule,
        MatExpansionModule,
        MatCheckboxModule,
        MatSliderModule,
        MatRadioModule,
        MatPaginatorModule,
        NgSelectModule
    ]
})
export class GameModule {
}
