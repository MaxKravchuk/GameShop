import { Component, OnDestroy, OnInit } from '@angular/core';
import { GameService } from "../../core/services/gameService/game.service";
import { Subscription } from "rxjs";
import { SharedService } from "../../core/services/helpers/sharedService/shared.service";
import { Game } from "../../core/models/Game";
import { AuthService } from "../../core/services/authService/auth.service";
import { Router } from "@angular/router";

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {

    numberOfGames!: number;

    private reloadSourceSub: Subscription = new Subscription();

    constructor(
        public authService: AuthService,
        private gameService: GameService,
        private sharedService: SharedService<Game>,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.reloadSourceSub = this.sharedService.reloadSource$.subscribe({
            next: (): void => {
                this.getNumberOfGames();
            }
        });

        this.getNumberOfGames();
    }

    ngOnDestroy(): void {
        this.reloadSourceSub.unsubscribe();
    }

    logOut(): void {
        this.authService.logout();
        this.router.navigate(['/']);
    }

    private getNumberOfGames(): void {
        this.gameService.getNumberOfGames().subscribe((data: number) => this.numberOfGames = data);
    }
}
