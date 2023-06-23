import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Game } from "../../../../core/models/Game";
import { GameService } from "../../../../core/services/gameService/game.service";
import { PagedList } from "../../../../core/models/PagedList";
import { Observable, Subscription, switchMap } from "rxjs";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { Router } from "@angular/router";
import { FilterModel } from "../../../../core/models/FilterModel";
import { MatDialog } from "@angular/material/dialog";
import { GameCrudComponent } from "../../../manager/components/dialogs/game-crud/game-crud.component";

@Component({
    selector: 'app-game-list',
    templateUrl: './game-list.component.html',
    styleUrls: ['./game-list.component.scss']
})
export class GameListComponent implements OnInit, OnDestroy {

    @Input() IsManager?: boolean;

    @Input() reloadGames!: Observable<boolean>;

    @Output() gameListStatus: EventEmitter<boolean> = new EventEmitter<boolean>();

    games?: Game[] = [];

    pageSize!: number;

    pageIndex!: number;

    HasNext!: boolean;

    HasPrevious!: boolean;

    MaxPageSize!: number;

    resultSub: Subscription = new Subscription();

    reloadGameSub: Subscription = new Subscription();

    receivedData: FilterModel = { gameFiltersDTO : {} };

    constructor(
        private gameService: GameService,
        private sharedService: SharedService<FilterModel>,
        private router: Router,
        private dialog: MatDialog
    ) {}

    ngOnInit(): void {
        this.updateGames(this.receivedData!);
        this.resultSub = this.sharedService.getData$().subscribe((data: FilterModel): void => {
            this.receivedData.gameFiltersDTO = data.gameFiltersDTO;
            this.pageIndex = 1;
            this.updateGames(data);
        });
        if (this.reloadGames !== undefined) {
            this.reloadGameSub = this.reloadGames.subscribe((reload: boolean): void => {
                if (reload) {
                    this.updateGames(this.receivedData!);
                }
            });
        }
        this.gameListStatus.emit(true);
    }

    ngOnDestroy(): void {
        this.resultSub.unsubscribe();
        this.reloadGameSub.unsubscribe();
        this.gameListStatus.emit(false);
    }

    pageSizeChange(value: number): void {
        this.pageSize = value;
        this.pageIndex = 1;
        this.updateGames(this.receivedData!);
    }

    pageIndexChanged(value: number): void {
        this.pageIndex = value;
        this.updateGames(this.receivedData!);
    }

    editDeleteGame(gameKey: string): void {
        this.gameService.getGameDetailsByKey(gameKey).pipe(
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

    private updateGames(filterParams: FilterModel): void {
        this.receivedData.gameFiltersDTO.pageNumber = this.pageIndex;
        this.receivedData.gameFiltersDTO.pageSize = this.pageSize;

         this.gameService.getAllGames(filterParams.gameFiltersDTO).subscribe(
            (pagedList: PagedList<Game>): void => {
                this.games = pagedList.Entities;
                this.HasNext = pagedList.HasNext;
                this.HasPrevious = pagedList.HasPrevious;
                this.MaxPageSize = pagedList.TotalCount;
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
