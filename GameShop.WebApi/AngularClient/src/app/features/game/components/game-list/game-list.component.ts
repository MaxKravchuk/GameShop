import { Component, OnDestroy, OnInit } from '@angular/core';
import { Game } from "../../../../core/models/Game";
import { GameService } from "../../../../core/services/gameService/game.service";
import { PagedList } from "../../../../core/models/PagedList";
import { Subscription } from "rxjs";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { FilterShared } from "../../../../core/models/helpers/FilterShared";

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

    receivedData: FilterShared = { FilterString: '' };

    constructor(
        private gameService: GameService,
        private sharedService: SharedService<FilterShared>
    ) {}

    ngOnInit(): void {
        this.updateGames(this.receivedData.FilterString!);

        this.resultSub = this.sharedService.getData$().subscribe((data: FilterShared): void => {
            if (data.FilterString != '') {
                console.log(data.FilterString);
                this.receivedData.FilterString = data.FilterString;
                this.updateGames(this.receivedData.FilterString!);
            }
        });
    }

    ngOnDestroy(): void {
        this.resultSub.unsubscribe();
    }

    pageSizeChange(value: any): void {
        this.pageSize = Number(value.target.value);
        this.updateGames(this.receivedData.FilterString!);
    }

    previousPage(): void {
        this.pageIndex--;
        this.updateGames(this.receivedData.FilterString!);
    }

    nextPage(): void {
        this.pageIndex++;
        this.updateGames(this.receivedData.FilterString!);
    }

    private updateGames(filterString: string): void {
        console.log("uP" + filterString);
        let filterParams;
        if (filterString != ''){
            filterParams =
                `${filterString}?gameFiltersDTO.pageNumber=${this.pageIndex}&gameFiltersDTO.pageSize=${this.pageSize}`;
        }
        else {
            filterParams =
                `?gameFiltersDTO.pageNumber=${this.pageIndex}&gameFiltersDTO.pageSize=${this.pageSize}`;
        }
         this.gameService.getAllGames(filterParams).subscribe(
            (pagedList: PagedList<Game>): void => {
                console.log(pagedList);
                this.games = pagedList.Entities;
                this.HasNext = pagedList.HasNext;
                this.HasPrevious = pagedList.HasPrevious;
            }
        );
    }
}
