import { Component, OnInit } from '@angular/core';
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Publisher } from "../../../../core/models/Publisher";
import { AuthService } from "../../../../core/services/authService/auth.service";
import { forkJoin, switchMap } from "rxjs";
import { Game } from "../../../../core/models/Game";
import { GameService } from "../../../../core/services/gameService/game.service";
import { GameCrudComponent } from "../../../manager/components/dialogs/game-crud/game-crud.component";
import { MatDialog } from "@angular/material/dialog";

@Component({
  selector: 'app-publisher-main',
  templateUrl: './publisher-main.component.html',
  styleUrls: ['./publisher-main.component.scss']
})
export class PublisherMainComponent implements OnInit {

    publisher?: Publisher;

    form!: FormGroup;

    isEditing: boolean = false;

    games: Game[] = [];

    private urlRegex: RegExp = /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w.-]+)+[\w\-._~:/?#[\]@!$&'()*+,;=]+$/;

    constructor(
        private publisherService: PublisherService,
        private authService: AuthService,
        private gameService: GameService,
        private formBuilder: FormBuilder,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            CompanyName: [{ value: '', disabled: true }, [Validators.required, Validators.maxLength(40)]],
            Description: [{ value: '', disabled: true }, Validators.required],
            HomePage: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(this.urlRegex)]]
        });

        forkJoin([
            this.publisherService.getPublisherByUserId(this.authService.getUserId()),
            this.gameService.getGamesByPublisherId(this.authService.getUserId())
        ]).subscribe(
            ([publisher, games]: [Publisher, Game[]]): void => {
                this.publisher = publisher;
                this.games = games;
                this.form.patchValue(this.publisher);
            }
        );
        this.publisherService.getPublisherByUserId(this.authService.getUserId()).pipe(
            switchMap((data: Publisher) => {
                this.publisher = data;
                this.form.patchValue(this.publisher);
                return this.gameService.getGamesByPublisherId(this.publisher!.Id!);
            })
        ).subscribe(
            (data: Game[]) => {
                this.games = data;
            }
        );

    }

    onEdit(): void {
        this.isEditing = true;
        this.form.enable();
    }

    onCancel(): void {
        this.isEditing = false;
        this.form.disable();
        this.form.patchValue(this.publisher!);
    }

    onSave(): void {
        if (!this.form.valid) {
            return;
        }

        const data: Publisher = this.form.value as Publisher;
        data.Id = this.publisher!.Id;
         this.publisherService.updatePublisher(data).subscribe({
             next: (): void => {
                 this.isEditing = false;
                 this.form.disable();
                 this.ngOnInit();
             }
         });
    }

    onGameEdit(game: Game): void {
        this.gameService.getGameDetailsByKey(game.Key!).pipe(
            switchMap((gameData: Game) => {
                const dialogRef = this.dialog.open(GameCrudComponent, {
                    autoFocus: false,
                    data: {
                        game: gameData,
                    }
                });

                return dialogRef.afterClosed();
            })
        ).subscribe((requireReload: boolean): void => {
            if (requireReload) {
                this.ngOnInit();
            }
        });
    }
}
