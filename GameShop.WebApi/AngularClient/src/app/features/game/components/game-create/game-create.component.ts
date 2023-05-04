import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { GameService } from "../../../../core/services/gameService/game.service";
import { GenreService } from "../../../../core/services/genreService/genre.service";
import { PlatformTypeService } from "../../../../core/services/platformTypeService/platform-type.service";
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { CreateGameModel } from "../../../../core/models/CreateGameModel";
import { Publisher } from "../../../../core/models/Publisher";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { catchError, combineLatest, combineLatestAll, forkJoin, mergeMap, Observable } from "rxjs";
import { map } from "rxjs/operators";

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
        private utilsService: UtilsService
    ) {}

    ngOnInit(): void {

        this.form = this.formBuilder.group({
            Name: new FormControl("", Validators.required),
            Description: new FormControl("", Validators.required),
            Key: new FormControl("", Validators.required),
            GenresId: new FormControl("", Validators.required),
            PlatformTypeId: new FormControl("", Validators.required),
            PublisherId: new FormControl("", Validators.required),
            Price: new FormControl("", [Validators.required, Validators.min(0)]),
            UnitsInStock: new FormControl("", [Validators.required, Validators.min(1)]),
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

    onNoClick() {
        this.utilsService.goBack();
    }

    onSaveForm() {
        if (!this.form.valid) {
            this.utilsService.openWithMessage("Please fill all the fields");
        }

        const data: CreateGameModel = this.form.value as CreateGameModel;
        this.gameService.createGame(data).subscribe({
            next: () => {
                this.utilsService.goBack();
            }
        });
    }
}
