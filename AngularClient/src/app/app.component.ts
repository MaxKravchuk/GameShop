import { Component } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title:string = 'Game shop';
  games: any;

  constructor(private http: HttpClient) {}
  addGame(): void {
    this.games = this.http
      .get('/api/games/getAll')
      .subscribe((data) => {
        console.log((data));
        this.games = data;
      });
  }
}
