import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ManagerMainComponent } from "../../manager-main/manager-main.component";
import { Genre } from "../../../../../core/models/Genre";
import { GenreService } from "../../../../../core/services/genreService/genre.service";
import { UtilsService } from "../../../../../core/services/helpers/utilsService/utils-service";

@Component({
  selector: 'app-genre-crud',
  templateUrl: './genre-crud.component.html',
  styleUrls: ['./genre-crud.component.css']
})
export class GenreCrudComponent implements OnInit {

    form!: FormGroup;

    genre!: Genre;

    genres: Genre[] = [];

    isAdding: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {genre: Genre, genres: Genre[]},
        private dialogRef: MatDialogRef<ManagerMainComponent>,
        private genreService: GenreService,
        private utilsService: UtilsService
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            Name: ['', Validators.minLength(1)],
            ParentId: [''],
        });

        this.genres = this.data.genres;

        if (this.data.genre == null) {
            this.isAdding = true;
            this.form.controls['ParentId'].hasValidator(Validators.required);
        }
        else {
            this.genre = this.data.genre;
        }
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onSaveClick(): void {
        const newGenre: Genre = this.form.value as Genre;
        newGenre.ParentId = this.genre.Id;
        this.genreService.createGenre(newGenre).subscribe((genre: Genre): void => {
            this.utilsService.openWithMessage("Genre created successfully!");
            this.dialogRef.close(true);
        });
    }

    onDeleteClick(): void {
        this.genreService.deleteGenre(this.genre.Id!).subscribe((genre: Genre): void => {
            this.utilsService.openWithMessage("Genre deleted successfully!");
            this.dialogRef.close(true);
        });
    }

    onEditClick(): void {
        console.log(this.genre);
        const newGenre: Genre = this.form.value as Genre;
        newGenre.Id = this.genre.Id;
        if (this.form.value['ParentId'] === '')
        {
            newGenre.ParentId = this.genre.ParentId;
        }

        this.genreService.updateGenre(newGenre).subscribe((genre: Genre): void => {
            this.utilsService.openWithMessage("Genre updated successfully!");
            this.dialogRef.close(true);
        });
    }
}
