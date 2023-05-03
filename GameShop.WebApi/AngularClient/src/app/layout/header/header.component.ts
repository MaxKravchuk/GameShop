import { Component, OnInit } from '@angular/core';
import { GameService } from "../../core/services/gameService/game.service";

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

    numberOfGames!: number;

    constructor(private gameService: GameService) {}

    ngOnInit(): void {
        this.getNumberOfGames();
    }

    private getNumberOfGames(): void {
        this.gameService.getNumberOfGames().subscribe((data: number) => this.numberOfGames = data);
    }
}
