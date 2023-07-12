import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription } from "rxjs";
import { AuthService } from "../../core/services/authService/auth.service";
import { GameService } from "../../core/services/gameService/game.service";
import { SharedService } from "../../core/services/helpers/sharedService/shared.service";
import { Game } from "../../core/models/Game";
import { NavigationExtras, Router } from "@angular/router";

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit, OnDestroy {

    numberOfGames!: number;

    private reloadSourceSub: Subscription = new Subscription();

    isAuthorized$!: Observable<boolean>;


    constructor(
        public authService: AuthService,
        private gameService: GameService,
        private sharedService: SharedService<Game>,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.isAuthorized$ = this.authService.getIsAuthorized$();

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
