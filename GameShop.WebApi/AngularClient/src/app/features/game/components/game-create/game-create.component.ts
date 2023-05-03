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
import { catchError, forkJoin, Observable } from "rxjs";

@Component({
    selector: 'app-game-create',
    templateUrl: './game-create.component.html',
    styleUrls: ['./game-create.component.css']
})
export class GameCreateComponent implements OnInit {

    gameGenres?: Genre[] = [];

    platformTypes?: PlatformType[] = [];

    publishers?: Publisher[] = [];

    form = new FormGroup({
        Name: new FormControl("", Validators.required),
        Description: new FormControl("", Validators.required),
        Key: new FormControl("", Validators.required),
        GenresId: new FormControl("", Validators.required),
        PlatformTypeId: new FormControl("", Validators.required),
        PublisherId: new FormControl("", Validators.required),
        Price: new FormControl("", Validators.required),
        UnitsInStock: new FormControl("", Validators.required),
    });

    constructor(
        @Inject(FormBuilder) private formBuilder: FormBuilder,
        private gameService: GameService,
        private genreService: GenreService,
        private platformTypeService: PlatformTypeService,
        private publisherService: PublisherService,
        private utilsService: UtilsService) {
    }

    ngOnInit(): void {
        this.getGenres();
        this.getPlatformTypes();
        this.getPublishers();
    }

    public onNoClick() {
        this.utilsService.goBack();
    }

    public onSaveForm() {
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

    private getGenres(): void {
        this.genreService.getAllGenres().subscribe(
            (data) => {
                this.gameGenres = data;
            }
        );
    }

    private getPlatformTypes(): void {
        this.platformTypeService.getAllPlatformTypes().subscribe(
            (data) => {
                this.platformTypes = data;
            }
        );
    }

    private getPublishers(): void {
        this.publisherService.getAllPublishers().subscribe(
            (data) => {
                this.publishers = data;
            }
        );
    }
}
