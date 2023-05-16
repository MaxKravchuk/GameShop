import { Component, OnInit } from '@angular/core';
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { Publisher } from "../../../../core/models/Publisher";
import { GenreService } from "../../../../core/services/genreService/genre.service";
import { PlatformTypeService } from "../../../../core/services/platformTypeService/platform-type.service";
import { GameService } from "../../../../core/services/gameService/game.service";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";

@Component({
  selector: 'app-game-filters',
  templateUrl: './game-filters.component.html',
  styleUrls: ['./game-filters.component.css']
})
export class GameFiltersComponent implements OnInit {

    genres: Genre[] = [];

    platforms: PlatformType[] = [];

    publishers: Publisher[] = [];

    fromValue?: number;

    toValue?: number;

    constructor(
        private genreService: GenreService,
        private platformTypeService: PlatformTypeService,
        private publisherService: PublisherService,
        private gameService: GameService
    ) { }

    ngOnInit(): void {
        this.genreService.getAllGenres().subscribe(
            (genres: Genre[]) => this.genres = genres
        );

        this.platformTypeService.getAllPlatformTypes().subscribe(
            (platforms: PlatformType[]) => this.platforms = platforms
        );

        this.publisherService.getAllPublishers().subscribe(
            (publishers: Publisher[]) => this.publishers = publishers
        );
    }

}
