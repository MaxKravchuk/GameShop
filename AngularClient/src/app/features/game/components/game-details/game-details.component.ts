import {Component, OnInit} from '@angular/core';
import {GameService} from "../../gameService/game.service";
import {Game} from "../../../../core/models/Game";
import {ActivatedRoute} from "@angular/router";
import {saveAs} from "file-saver";

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {

  game?: Game;
  gameKey: string | null = null;
  constructor(
    private gameService: GameService,
    private activeRoute: ActivatedRoute){
    this.gameKey = this.activeRoute.snapshot.paramMap.get('Key');
  }

  private getGameDetailsByKey(Key:string): void{
    console.log("Key: " + Key)
    this.gameService.getGameDetailsByKey(Key).subscribe((data)=>{
      console.log("Game Details");
      console.log(data);
      this.game = data;
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
