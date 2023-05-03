import { Component, OnInit } from '@angular/core';
import { Game } from "../../../../core/models/Game";
import { GameService } from "../../../../core/services/gameService/game.service";

@Component({
  selector: 'app-game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.css']
})
export class GameListComponent implements OnInit {

    games: Game[] = [];

    constructor(
        private gameService: GameService
    ) {

    }

    ngOnInit(): void {
        this.getGames();
    }

    private getGames(): void {
        this.gameService.getAllGames().subscribe((data) => {
            this.games = data;
        });
    }

}
