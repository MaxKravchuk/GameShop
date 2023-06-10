import { Component, OnInit } from '@angular/core';
import { GenreService } from "../../../../core/services/genreService/genre.service";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { Publisher } from "../../../../core/models/Publisher";
import { Subject } from "rxjs";
import { PlatformTypeService } from "../../../../core/services/platformTypeService/platform-type.service";
import { MatDialog } from "@angular/material/dialog";
import { GenreCrudComponent } from "../dialogs/genre-crud/genre-crud.component";
import { PlatformTypeCrudComponent } from "../dialogs/platform-type-crud/platform-type-crud.component";
import { PublisherCrudComponent } from "../dialogs/publisher-crud/publisher-crud.component";
import { GameCrudComponent } from "../dialogs/game-crud/game-crud.component";
import { PagedList } from "../../../../core/models/PagedList";

@Component({
  selector: 'app-manager-main',
  templateUrl: './manager-main.component.html',
  styleUrls: ['./manager-main.component.css']
})
export class ManagerMainComponent implements OnInit {

    genres?: Genre[] = [];

    platformTypes?: PlatformType[] = [];

    publishers?: Publisher[] = [];

    pageSizeGenre!: number;

    pageSizePlt!: number;

    pageSizePub!: number;

    totalCountGenre!: number;

    totalCountPlt!: number;

    totalCountPub!: number;

    pageIndexGenre!: number;

    pageIndexPlt!: number;

    pageIndexPub!: number;

    HasNextGenre!: boolean;

    HasNextPlt!: boolean;

    HasNextPub!: boolean;

    HasPreviousGenre!: boolean;

    HasPreviousPlt!: boolean;

    HasPreviousPub!: boolean;

    reloadGames: Subject<boolean> = new Subject<boolean>();

    constructor(
        private genreService: GenreService,
        private platformTypeService: PlatformTypeService,
        private publisherService: PublisherService,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.updateGenres();
        this.updatePlatformTypes();
        this.updatePublishers();
    }

    pageSizeChangeGenre(value: number): void {
        this.pageSizeGenre = value;
        this.pageIndexGenre = 1;
        this.updateGenres();
    }

    pageSizeChangePlt(value: number): void {
        this.pageSizePlt = value;
        this.pageIndexPlt = 1;
        this.updatePlatformTypes();
    }

    pageSizeChangePub(value: number): void {
        this.pageSizePub = value;
        this.pageIndexPub = 1;
        this.updatePublishers();
    }

    pageIndexChangeGenre(value: number): void {
        this.pageIndexGenre = value;
        this.updateGenres();
    }

    pageIndexChangePlt(value: number): void {
        this.pageIndexPlt = value;
        this.updatePlatformTypes();
    }

    pageIndexChangePub(value: number): void {
        this.pageIndexPub = value;
        this.updatePublishers();
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

    updatePlatformTypes(): void {
        const pagedParams = {
            pageSize: this.pageSizePlt,
            pageNumber: this.pageIndexPlt
        };
        this.platformTypeService.getAllPlatformTypesPaged(pagedParams).subscribe((pagedResult: PagedList<PlatformType>):void => {
            this.platformTypes = pagedResult.Entities;
            this.totalCountPlt = pagedResult.TotalCount;
            this.HasNextPlt = pagedResult.HasNext;
            this.HasPreviousPlt = pagedResult.HasPrevious;
        });
    }

    updatePublishers(): void {
        const pagedParams = {
            pageSize: this.pageSizePub,
            pageNumber: this.pageIndexPub
        };
        this.publisherService.getAllPublishersPaged(pagedParams).subscribe((pagedResult: PagedList<Publisher>):void => {
            this.publishers = pagedResult.Entities;
            this.totalCountPub = pagedResult.TotalCount;
            this.HasNextPub = pagedResult.HasNext;
            this.HasPreviousPub = pagedResult.HasPrevious;
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

    addPlatformType(): void {
        const dialogRef = this.dialog.open(PlatformTypeCrudComponent, {
            autoFocus: false,
            data: {
                platformType: null,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updatePlatformTypes();
            }
        });
    }

    editDeletePlatformType(platformType: PlatformType): void {
        const dialogRef = this.dialog.open(PlatformTypeCrudComponent, {
            autoFocus: false,
            data: {
                platformType: platformType,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updatePlatformTypes();
            }
        });
    }

    addPublisher(): void {
        const dialogRef = this.dialog.open(PublisherCrudComponent, {
            autoFocus: false,
            data: {
                publisher: null,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updatePublishers();
            }
        });
    }

    editDeletePublisher(publisher: Publisher): void {
        const dialogRef = this.dialog.open(PublisherCrudComponent, {
            autoFocus: false,
            data: {
                publisher: publisher,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updatePublishers();
            }
        });
    }

    addGame(): void {
        const dialogRef = this.dialog.open(GameCrudComponent, {
            autoFocus: false,
            data: {
                game: null,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.reloadGames.next(true);
            }
        });
    }
}
