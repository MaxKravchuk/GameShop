import { Component, OnInit } from '@angular/core';
import { GameService } from "../../../../core/services/gameService/game.service";
import { Game } from "../../../../core/models/Game";
import { ActivatedRoute } from "@angular/router";
import { saveAs } from "file-saver";
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { Publisher } from "../../../../core/models/Publisher";
import { CartService } from "../../../../core/services/cartService/cart.service";
import { CartItem } from "../../../../core/models/CartItem";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { concatMap, forkJoin, Observable } from "rxjs";
import { AuthService } from "../../../../core/services/authService/auth.service";
import { compareSegments } from "@angular/compiler-cli/src/ngtsc/sourcemaps/src/segment_marker";

@Component({
    selector: 'app-game-details',
    templateUrl: './game-details.component.html',
    styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {

    game!: Game;

    IsDeleted!: boolean;

    genres?: Genre[] = [];

    platformTypes?: PlatformType[] = [];

    publisher?: Publisher | null;

    gameKey?: string | null;

    isAvailable: boolean = false;

    gamesInCart?: number;

    constructor(
        private gameService: GameService,
        private shoppingCartService: CartService,
        private utilsService: UtilsService,
        private activeRoute: ActivatedRoute,
        private authService: AuthService
    ) {}

    ngOnInit(): void {
        this.gameKey = this.activeRoute.snapshot.paramMap.get('Key');

        if (this.gameKey != null) {
            this.getGameDetailsByKey(this.gameKey);
        } else {
            this.utilsService.goBack();
        }
    }

    downloadGame(): void {
        this.gameService.downloadGame(this.gameKey!).subscribe(
            (blob: Blob): void => {
                saveAs(blob, `${this.gameKey}.bin`);
            });
    }

    buyGame(): void {
        this.isAvailable = false;

        let cartItem: CartItem = {
            GameKey: this.game.Key!,
            GameName: this.game.Name!,
            GamePrice: this.game.Price!,
        }

        this.shoppingCartService.addToCart(cartItem)
            .pipe(
                concatMap(() => this.shoppingCartService.getNumberOfGamesInCart(this.gameKey!))
            ).subscribe({
            next: (data: number): void => {
                this.gamesInCart = data;
                this.utilsService.openWithMessage('Game has been added to your cart.');
                if (data === this.game.UnitsInStock!) {
                    this.utilsService.openWithMessage('Game is out of stock.');
                    this.isAvailable = false;
                }
                else {
                    this.isAvailable = true;
                }
            }
        });
    }

    private getGameDetailsByKey(Key: string): void {
        const gameDetails$: Observable<Game> = this.gameService.getGameDetailsByKey(Key);
        const numberOfGames$: Observable<number> = this.shoppingCartService.getNumberOfGamesInCart(this.gameKey!);

        forkJoin([gameDetails$, numberOfGames$]).subscribe(
            ([gameDetails, numberOfGames]: [Game, number]) => {
                this.game = gameDetails;
                this.genres = gameDetails.Genres;
                this.platformTypes = gameDetails.PlatformTypes;
                this.publisher = gameDetails.PublisherReadDTO;
                this.isAvailable = gameDetails.UnitsInStock! > 0;
                this.isAvailable = numberOfGames < gameDetails.UnitsInStock!;
                this.IsDeleted = gameDetails.IsDeleted!;
                if (this.authService.isInRole('User') && gameDetails.IsDeleted) {
                    this.isAvailable = false;
                }
            }
        );
    }
}
