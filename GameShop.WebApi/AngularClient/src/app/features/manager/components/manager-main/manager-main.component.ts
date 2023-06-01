import { Component, OnInit } from '@angular/core';
import { GenreService } from "../../../../core/services/genreService/genre.service";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { Publisher } from "../../../../core/models/Publisher";
import { forkJoin } from "rxjs";
import { PlatformTypeService } from "../../../../core/services/platformTypeService/platform-type.service";
import { MatDialog } from "@angular/material/dialog";
import { UserCrudComponent } from "../../../admin/components/dialogs/user-crud/user-crud.component";
import { GenreCrudComponent } from "../dialogs/genre-crud/genre-crud.component";

@Component({
  selector: 'app-manager-main',
  templateUrl: './manager-main.component.html',
  styleUrls: ['./manager-main.component.css']
})
export class ManagerMainComponent implements OnInit {

    genres: Genre[] = [];

    platforms: PlatformType[] = [];

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
            this.publisherService.getAllPublishers()
        ]).subscribe(([genres, platforms, publishers] : [Genre[], PlatformType[], Publisher[]]): void => {
            this.genres = genres;
            this.platforms = platforms;
            this.publishers = publishers;
        });
    }

    onAddPublisher(): void {

    }

    onAddPlatformType(): void {

    }

    onAddGenre(): void {
        const dialogRef = this.dialog.open(GenreCrudComponent, {
            autoFocus: false,
            data: {
                user: user
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.ngOnInit();
            }
        });
    }
}
