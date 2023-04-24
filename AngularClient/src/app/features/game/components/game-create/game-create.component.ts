import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {GameService} from "../../../../core/services/gameService/game.service";
import {GenreService} from "../../../../core/services/genreService/genre.service";
import {PlatformTypeService} from "../../../../core/services/platformTypeService/platform-type.service";
import {Genre} from "../../../../core/models/Genre";
import {PlatformType} from "../../../../core/models/PlatformType";
import {CreateGameDTO} from "../../../../core/DTOs/GameDTOs/CreateGameDTO";

@Component({
  selector: 'app-game-create',
  templateUrl: './game-create.component.html',
  styleUrls: ['./game-create.component.css']
})
export class GameCreateComponent implements OnInit {

  constructor(
    @Inject(FormBuilder) private formBuilder: FormBuilder,
    private gameService: GameService,
    private genreService: GenreService,
    private platformTypeService: PlatformTypeService) { }

  async ngOnInit(): Promise<void> {
    await this.getGenres();
    await this.getPlatformTypes();
  }

  form = new FormGroup({
    Name: new FormControl("", Validators.required),
    Description: new FormControl("", Validators.required),
    Key: new FormControl("", Validators.required),
    GenresId: new FormControl("",Validators.required),
    PlatformTypeId: new FormControl("",Validators.required),
  });

  gameGenres?: Genre[] = [];
  platformTypes?: PlatformType[] = [];

  onNoClick() {
    this.gameService.goToPrevPage();
  }

  onSaveForm() {
    if(this.form.valid){
      const data:CreateGameDTO = this.form.value as CreateGameDTO
      this.gameService.createGame(data).subscribe(()=>this.gameService.goToPrevPage());
      console.log(data);
    }
  }

  private async getGenres(): Promise<void> {
    try {
      this.gameGenres = await this.genreService.getAllGenres().toPromise();
    } catch (error) {
      // Handle error here
      console.error('Error getting genres:', error);
    }
  }

  private async getPlatformTypes(): Promise<void> {
    try {
      this.platformTypes = await this.platformTypeService.getAllPlatformTypes().toPromise();
    } catch (error) {
      // Handle error here
      console.error('Error getting platform types:', error);
    }
  }

}
