import { Component } from '@angular/core';
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { Publisher } from "../../../../core/models/Publisher";

@Component({
  selector: 'app-game-filters',
  templateUrl: './game-filters.component.html',
  styleUrls: ['./game-filters.component.css']
})
export class GameFiltersComponent {

    genres: Genre[] = [];

    platforms: PlatformType[] = [];

    publishers: Publisher[] = [];

    fromValue?: number;

    toValue?: number;



}
