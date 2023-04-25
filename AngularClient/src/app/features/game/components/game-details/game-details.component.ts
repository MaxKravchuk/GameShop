import {Component, OnInit} from '@angular/core';
import {GameService} from "../../../../core/services/gameService/game.service";
import {Game} from "../../../../core/models/Game";
import {ActivatedRoute} from "@angular/router";
import {saveAs} from "file-saver";
import {Genre} from "../../../../core/models/Genre";
import {PlatformType} from "../../../../core/models/PlatformType";
import {Publisher} from "../../../../core/models/Publisher";

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
  gameKey: string | null = null;
  constructor(
    private gameService: GameService,
    private activeRoute: ActivatedRoute){
    this.gameKey = this.activeRoute.snapshot.paramMap.get('Key');
  }

  private getGameDetailsByKey(Key:string): void{
    this.gameService.getGameDetailsByKey(Key).subscribe((data)=>{
      this.game = data;
      this.genres = data.Genres;
      this.platformTypes = data.PlatformTypes;
      this.publisher = data.PublisherReadDTO;
    });
  }


  ngOnInit(): void {
    if(this.gameKey != null){
      this.getGameDetailsByKey(this.gameKey);
    }
    else{
      this.gameService.goToPrevPage();
    }
  }

  downloadGame() {
    this.gameService.downloadGame(this.gameKey!).subscribe(
      (blob: Blob) => {
        saveAs(blob, `${this.gameKey}.bin`);
      },
      (error: any) => {
        console.error('Failed to download game:', error);
        // Handle error here, e.g., show an error message
      }
    );
  }
}
