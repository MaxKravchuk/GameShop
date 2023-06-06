import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Genre } from "../../../../../core/models/Genre";
import { PlatformType } from "../../../../../core/models/PlatformType";
import { Publisher } from "../../../../../core/models/Publisher";
import { GameService } from "../../../../../core/services/gameService/game.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ManagerMainComponent } from "../../manager-main/manager-main.component";
import { UtilsService } from "../../../../../core/services/helpers/utilsService/utils-service";
import { GenreService } from "../../../../../core/services/genreService/genre.service";
import { PlatformTypeService } from "../../../../../core/services/platformTypeService/platform-type.service";
import { PublisherService } from "../../../../../core/services/publisherService/publisher.service";
import { forkJoin } from "rxjs";
import { CreateGameModel } from "../../../../../core/models/CreateGameModel";
import { Game } from "../../../../../core/models/Game";

@Component({
  selector: 'app-game-crud',
  templateUrl: './game-crud.component.html',
  styleUrls: ['./game-crud.component.css']
})
export class GameCrudComponent implements OnInit {

    isAdding: boolean = false;

    form!: FormGroup;

    game!: Game;

    gameGenres: Genre[] = [];

    platformTypes: PlatformType[] = [];

    publishers: Publisher[] = [];

    constructor(
        @Inject(MAT_DIALOG_DATA) private data : {game: Game},
        private dialogRef: MatDialogRef<ManagerMainComponent>,
        private formBuilder: FormBuilder,
        private gameService: GameService,
        private genreService: GenreService,
        private platformTypeService: PlatformTypeService,
        private publisherService: PublisherService,
        private utilsService: UtilsService
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            Name: ['', Validators.required],
            Description: ['', [Validators.required,Validators.minLength(50)]],
            Key: ['', Validators.required],
            GenresId: [''],
            PlatformTypeId: [''],
            PublisherId: [''],
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

        if (this.data.game == null) {
            this.isAdding = true;
        }
        else {
            this.game = this.data.game;
            this.gameService.getGameDetailsByKey(this.game.Key!).subscribe(
                (game: Game): void => {

                }
            );
            this.form.patchValue(this.game);
            this.form.controls['GenresId'].setValue(this.game.Genres?.map(x => x.Id));
            this.form.controls['PlatformTypeId'].setValue(this.game.PlatformTypes?.map(x => x.Id));
            this.form.controls['PublisherId'].setValue(this.game.PublisherReadDTO?.Id);
        }
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onSaveClick(): void {
        if (!this.form.valid) {
            this.utilsService.openWithMessage('Please fill all the fields');
        }

        const data: CreateGameModel = this.form.value as CreateGameModel;
        this.gameService.createGame(data).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage('Game created successfully');
                this.dialogRef.close(true);
            }
        });
    }

    onEditClick(): void {
        const data: CreateGameModel = this.form.value as CreateGameModel;
        data.Id = this.game.Id;
        this.gameService.updateGame(data).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage('Game updated successfully');
                this.dialogRef.close(true);
            }
        });
    }

    onDeleteClick(): void {
        this.gameService.deleteGame(this.game.Key!).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage('Game deleted successfully');
                this.dialogRef.close(true);
            }
        });
    }
}
