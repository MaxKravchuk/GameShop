import { Component } from '@angular/core';
import { Subject } from "rxjs";
import { MatDialog } from "@angular/material/dialog";
import { GameCrudComponent } from "../dialogs/game-crud/game-crud.component";

@Component({
  selector: 'app-manager-games',
  templateUrl: './manager-games.component.html',
  styleUrls: ['./manager-games.component.scss']
})
export class ManagerGamesComponent {

    reloadGames: Subject<boolean> = new Subject<boolean>();

    constructor(private dialog: MatDialog) {}

    addGame(): void {
        const dialogRef = this.dialog.open(GameCrudComponent, {
            autoFocus: false,
            data: {
                game: null,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.reloadGames.next(true);
            }
        });
    }
}
