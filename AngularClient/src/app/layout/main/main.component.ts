import {Component, OnInit} from '@angular/core';
import {Game} from "../../core/models/Game";
import {MainPageService} from "./MainService/main-page.service";

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  games: Game[] = [];

  constructor(
    private mainPageService: MainPageService
  ) { }

  private updateList():void{
    this.mainPageService.getGames().subscribe((data)=> {
      this.games = data;
    });
  }

  ngOnInit(): void {
    this.updateList();
  }
}
