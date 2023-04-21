import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(
    private http : HttpClient)
  {}

  ngOnInit(): void {
    this.getNumberOfGames();
  }

  private apiUrl = '/api/games/';
  numberOfGames: number = 0;

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  getNumberOfGames(): void{
    this.http.get<number>(`${this.apiUrl}numberOfGames`,this.httpOptions)
      .subscribe(
        (data) => {
          this.numberOfGames = data;
        }
      );
  }
}
