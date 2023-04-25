import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map} from "rxjs/operators";
import {Observable} from "rxjs";
import {GameService} from "../../core/services/gameService/game.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  numberOfGames: number = 0;

  constructor(
    private http : HttpClient,
    private gameService: GameService)
  {
  }

  async ngOnInit(): Promise<void> {
    await this.getNumberOfGames();
  }

  private async getNumberOfGames(): Promise<void>{
    this.gameService.getNumberOfGames().subscribe((data) => {
      this.numberOfGames = data;
    },(error) => {
      console.error('Error fetching number of games:', error);
    });
  }
}
