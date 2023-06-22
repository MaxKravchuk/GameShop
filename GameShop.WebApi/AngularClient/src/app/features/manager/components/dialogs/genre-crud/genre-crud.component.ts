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
  styleUrls: ['./genre-crud.component.scss']
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
            Name: ['', [Validators.minLength(1), Validators.required]],
            ParentGenreId: [''],
        });

        this.genres = this.data.genres;

        if (this.data.genre == null) {
            this.isAdding = true;
        }
        else {
            this.genre = this.data.genre;
            this.form.patchValue(this.data.genre);
        }
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onSaveClick(): void {
        const newGenre: Genre = this.form.value as Genre;
        this.genreService.createGenre(newGenre).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage("Genre created successfully!");
                this.dialogRef.close(true);
            }
        });
    }

    onDeleteClick(): void {
        this.genreService.deleteGenre(this.genre.Id!).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage("Genre deleted successfully!");
                this.dialogRef.close(true);
            }
        });
    }

    onEditClick(): void {
        const newGenre: Genre = this.form.value as Genre;
        console.log(newGenre);
        newGenre.Id = this.genre.Id;
        this.genreService.updateGenre(newGenre).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage("Genre updated successfully!");
                this.dialogRef.close(true);
            }
        });
    }
}
