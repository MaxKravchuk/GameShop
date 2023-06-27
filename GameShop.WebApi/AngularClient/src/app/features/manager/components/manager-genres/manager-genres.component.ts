import { Component, OnInit } from '@angular/core';
import { Genre } from "../../../../core/models/Genre";
import { GenreService } from "../../../../core/services/genreService/genre.service";
import { MatDialog } from "@angular/material/dialog";
import { PagedList } from "../../../../core/models/PagedList";
import { GenreCrudComponent } from "../dialogs/genre-crud/genre-crud.component";

@Component({
  selector: 'app-manager-genres',
  templateUrl: './manager-genres.component.html',
  styleUrls: ['./manager-genres.component.scss']
})
export class ManagerGenresComponent implements OnInit {

    genres?: Genre[] = [];

    pageSizeGenre!: number;

    totalCountGenre!: number;

    pageIndexGenre!: number;

    HasNextGenre!: boolean;

    HasPreviousGenre!: boolean;

    constructor(
        private genreService: GenreService,
        private dialog: MatDialog
    ) {}

    ngOnInit(): void {
        this.updateGenres();
    }

    pageSizeChangeGenre(value: number): void {
        this.pageSizeGenre = value;
        this.pageIndexGenre = 1;
        this.updateGenres();
    }

    pageIndexChangeGenre(value: number): void {
        this.pageIndexGenre = value;
        this.updateGenres();
    }

    updateGenres(): void {
        const pagedParams = {
            pageSize: this.pageSizeGenre,
            pageNumber: this.pageIndexGenre
        };
        this.genreService.getAllGenresPaged(pagedParams).subscribe((pagedResult: PagedList<Genre>):void => {
            this.genres = pagedResult.Entities;
            this.totalCountGenre = pagedResult.TotalCount;
            this.HasNextGenre = pagedResult.HasNext;
            this.HasPreviousGenre = pagedResult.HasPrevious;
        });
    }

    addGenre(): void {
        const dialogRef = this.dialog.open(GenreCrudComponent, {
            autoFocus: false,
            data: {
                genre: null,
                genres: this.genres
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updateGenres();
            }
        });
    }

    editDeleteGenre(genre: Genre): void {
        const dialogRef = this.dialog.open(GenreCrudComponent, {
            autoFocus: false,
            data: {
                genre: genre,
                genres: this.genres!.filter((g: Genre): boolean => g.Id !== genre.Id)
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updateGenres();
            }
        });
    }
}
