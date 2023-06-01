import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagerMainComponent } from './components/manager-main/manager-main.component';
import { MatButtonModule } from "@angular/material/button";
import { RouterLink } from "@angular/router";
import { GenreCrudComponent } from './components/dialogs/genre-crud/genre-crud.component';
import { MatDialogModule } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { ReactiveFormsModule } from "@angular/forms";



@NgModule({
  declarations: [
    ManagerMainComponent,
    GenreCrudComponent
  ],
    imports: [
        CommonModule,
        MatButtonModule,
        RouterLink,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule
    ]
})
export class ManagerModule { }
