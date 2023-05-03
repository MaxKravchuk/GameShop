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

    async ngOnInit(): Promise<void> {
        await this.getGenres();
        await this.getPlatformTypes();
        await this.getPublishers();
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

    private async getGenres(): Promise<void> {
        this.gameGenres = await this.genreService.getAllGenres().toPromise();
    }

    private async getPlatformTypes(): Promise<void> {
        this.platformTypes = await this.platformTypeService.getAllPlatformTypes().toPromise();
    }

    private async getPublishers(): Promise<void> {
        this.publishers = await this.publisherService.getAllPublishers().toPromise();
    }
}
