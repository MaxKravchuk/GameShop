import { Component, OnInit } from '@angular/core';
import { GenreService } from "../../../../core/services/genreService/genre.service";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { Publisher } from "../../../../core/models/Publisher";
import { forkJoin } from "rxjs";
import { PlatformTypeService } from "../../../../core/services/platformTypeService/platform-type.service";
import { MatDialog } from "@angular/material/dialog";
import { GenreCrudComponent } from "../dialogs/genre-crud/genre-crud.component";
import { PlatformTypeCrudComponent } from "../dialogs/platform-type-crud/platform-type-crud.component";
import { PublisherCrudComponent } from "../dialogs/publisher-crud/publisher-crud.component";
import { GameCrudComponent } from "../dialogs/game-crud/game-crud.component";

@Component({
  selector: 'app-manager-main',
  templateUrl: './manager-main.component.html',
  styleUrls: ['./manager-main.component.css']
})
export class ManagerMainComponent implements OnInit {

    genres: Genre[] = [];

    platformTypes: PlatformType[] = [];

    publishers: Publisher[] = [];

    constructor(
        private genreService: GenreService,
        private platformTypeService: PlatformTypeService,
        private publisherService: PublisherService,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        forkJoin([
            this.genreService.getAllGenres(),
            this.platformTypeService.getAllPlatformTypes(),
            this.publisherService.getAllPublishers(),
        ]).subscribe((
            [genres, platformTypes, publishers] :
                [Genre[], PlatformType[], Publisher[]]): void => {
            this.genres = genres;
            this.platformTypes = platformTypes;
            this.publishers = publishers;
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
                this.ngOnInit();
            }
        });
    }

    editDeleteGenre(genre: Genre): void {
        const dialogRef = this.dialog.open(GenreCrudComponent, {
            autoFocus: false,
            data: {
                genre: genre,
                genres: this.genres
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.ngOnInit();
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
                this.ngOnInit();
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
                this.ngOnInit();
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
                this.ngOnInit();
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
                this.ngOnInit();
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
                this.ngOnInit();
            }
        });
    }
}
