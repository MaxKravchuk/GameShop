import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Role } from "../../../../../core/models/Role";
import { ManagerMainComponent } from "../../manager-main/manager-main.component";
import { Genre } from "../../../../../core/models/Genre";

@Component({
  selector: 'app-genre-crud',
  templateUrl: './genre-crud.component.html',
  styleUrls: ['./genre-crud.component.css']
})
export class GenreCrudComponent implements OnInit {

    form!: FormGroup;

    genre!: Genre;

    isAdding: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {role: Role},
        private dialogRef: MatDialogRef<ManagerMainComponent>
    ) { }

    ngOnInit(): void {

    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onSaveClick(): void {

    }

    onDeleteClick(): void {

    }
}
