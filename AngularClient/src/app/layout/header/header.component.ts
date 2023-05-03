import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {first, map} from "rxjs/operators";
import {Observable} from "rxjs";
import {GameService} from "../../core/services/gameService/game.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  numberOfGames!: number;

  constructor(
    private http : HttpClient,
    private gameService: GameService)
  {
  }

  ngOnInit(): void{
      this.getNumberOfGames();
  }

  private getNumberOfGames(): void {
    try {
      const data = this.gameService.getNumberOfGames().subscribe(
        (data: number) => this.numberOfGames = data
      );
    } catch (error) {
      console.error('Error fetching number of games:', error);
    }
  }
}
