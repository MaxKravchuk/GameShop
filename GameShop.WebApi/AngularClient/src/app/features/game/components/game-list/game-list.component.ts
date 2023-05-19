import { Component, OnDestroy, OnInit } from '@angular/core';
import { Game } from "../../../../core/models/Game";
import { GameService } from "../../../../core/services/gameService/game.service";
import { PagedList } from "../../../../core/models/PagedList";
import { Subscription } from "rxjs";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { Router } from "@angular/router";
import { FilterModel } from "../../../../core/models/FilterModel";

@Component({
    selector: 'app-game-list',
    templateUrl: './game-list.component.html',
    styleUrls: ['./game-list.component.css']
})
export class GameListComponent implements OnInit, OnDestroy {

    games?: Game[] = [];

    pageSize: number = 10;

    pageIndex: number = 1;

    pageSizeOptions: string[] = ['1', '10', '20', '50', '100', 'All'];

    HasNext?: boolean;

    HasPrevious?: boolean;

    resultSub: Subscription = new Subscription();

    receivedData: FilterModel = { gameFiltersDTO : {} };

    constructor(
        private gameService: GameService,
        private sharedService: SharedService<FilterModel>,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.updateGames(this.receivedData!);
        this.resultSub = this.sharedService.getData$().subscribe((data: FilterModel): void => {
            this.receivedData.gameFiltersDTO = data.gameFiltersDTO;
            this.pageIndex = 1;
            this.updateGames(data);
        });
    }

    ngOnDestroy(): void {
        this.resultSub.unsubscribe();
    }

    pageSizeChange(value: any): void {
        this.pageSize = Number(value.target.value);
        this.updateGames(this.receivedData!);
    }

    previousPage(): void {
        this.pageIndex--;
        this.updateGames(this.receivedData!);
    }

    nextPage(): void {
        this.pageIndex++;
        this.updateGames(this.receivedData!);
    }

    private updateGames(filterParams: FilterModel): void {
        this.receivedData.gameFiltersDTO.pageNumber = this.pageIndex;
        this.receivedData.gameFiltersDTO.pageSize = this.pageSize;

         this.gameService.getAllGames(filterParams.gameFiltersDTO).subscribe(
            (pagedList: PagedList<Game>): void => {
                this.games = pagedList.Entities;
                this.HasNext = pagedList.HasNext;
                this.HasPrevious = pagedList.HasPrevious;
            }
        );
         this.setUrl();
    }

    private setUrl(): void {
        this.router.navigate([],{
            queryParams: {
                page: this.pageIndex
            }
        })
    }
}
