import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { GameService } from "../../../../core/services/gameService/game.service";
import { GenreService } from "../../../../core/services/genreService/genre.service";
import { PlatformTypeService } from "../../../../core/services/platformTypeService/platform-type.service";
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { CreateGameModel } from "../../../../core/models/CreateGameModel";
import { Publisher } from "../../../../core/models/Publisher";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { forkJoin } from "rxjs";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { Game } from "../../../../core/models/Game";

@Component({
    selector: 'app-game-create',
    templateUrl: './game-create.component.html',
    styleUrls: ['./game-create.component.css']
})
export class GameCreateComponent implements OnInit {

    gameGenres!: Genre[];

    platformTypes!: PlatformType[];

    publishers!: Publisher[];

    form!: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private gameService: GameService,
        private genreService: GenreService,
        private platformTypeService: PlatformTypeService,
        private publisherService: PublisherService,
        private utilsService: UtilsService,
        private sharedService: SharedService<Game>
    ) {}

    ngOnInit(): void {

        this.form = this.formBuilder.group({
            Name: ['', Validators.required],
            Description: ['', [Validators.required,Validators.minLength(50)]],
            Key: ['', Validators.required],
            GenresId: ['', Validators.required],
            PlatformTypeId: ['', Validators.required],
            PublisherId: ['', Validators.required],
            Price: ['', [Validators.required, Validators.min(0)]],
            UnitsInStock: ['', [Validators.required, Validators.min(1)]],
        });

        forkJoin([
            this.genreService.getAllGenres(),
            this.platformTypeService.getAllPlatformTypes(),
            this.publisherService.getAllPublishers()
        ]).subscribe(([genres, platformTypes, publishers]) => {
            this.gameGenres = genres;
            this.platformTypes = platformTypes;
            this.publishers = publishers;
        });
    }

    onNoClick(): void {
        this.utilsService.goBack();
    }

    onSaveForm(): void {
        if (!this.form.valid) {
            this.utilsService.openWithMessage('Please fill all the fields');
        }

        const data: CreateGameModel = this.form.value as CreateGameModel;
        this.gameService.createGame(data).subscribe({
            next: (): void => {
                this.sharedService.reloadSource();
                this.utilsService.goBack();
            }
        });
    }
}
