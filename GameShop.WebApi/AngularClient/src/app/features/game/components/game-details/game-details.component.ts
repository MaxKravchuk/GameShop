import { Component, OnInit } from '@angular/core';
import { GameService } from "../../../../core/services/gameService/game.service";
import { Game } from "../../../../core/models/Game";
import { ActivatedRoute } from "@angular/router";
import { saveAs } from "file-saver";
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { Publisher } from "../../../../core/models/Publisher";
import { ShoppingCartService } from "../../../../core/services/cartService/shopping-cart.service";
import { CartItem } from "../../../../core/models/CartItem";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
    selector: 'app-game-details',
    templateUrl: './game-details.component.html',
    styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {

    game!: Game;

    genres?: Genre[] = [];

    platformTypes?: PlatformType[] = [];

    publisher?: Publisher;

    gameKey?: string | null;

    constructor(
        private gameService: GameService,
        private shoppingCartService: ShoppingCartService,
        private utilsService: UtilsService,
        private activeRoute: ActivatedRoute
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
        let cartItem: CartItem = {
            GameKey: this.game.Key!,
            GameName: this.game.Name!,
            GamePrice: this.game.Price!,
        }

        this.shoppingCartService.addToCart(cartItem).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage('Game has been added to your cart.');
            },
        });
    }

    private getGameDetailsByKey(Key: string): void {
        this.gameService.getGameDetailsByKey(Key).subscribe((data: Game): void => {
            this.game = data;
            this.genres = data.Genres;
            this.platformTypes = data.PlatformTypes;
            this.publisher = data.PublisherReadDTO;
        });
    }
}
