import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {GameService} from "../../gameService/game.service";

@Component({
  selector: 'app-game-create',
  templateUrl: './game-create.component.html',
  styleUrls: ['./game-create.component.css']
})
export class GameCreateComponent implements OnInit {

  constructor(
    @Inject(FormBuilder) private formBuilder: FormBuilder,
    private gameService: GameService) { }

  form = new FormGroup({
    gameName: new FormControl("", Validators.required),
    gameDescription: new FormControl("", Validators.required),
    gameKey: new FormControl("", Validators.required),
    gameGenres: new FormControl("",Validators.required),
    platformTypes: new FormControl("",Validators.required),
  });

  gameGenres = ["Action","Adventure"]
  platformTypes = ["PC","Xbox","Playstation","Nintendo"]

  onNoClick() {
    this.gameService.goToPrevPage();
  }
  onSaveForm() {
    if(this.form.valid){
      const data = this.form.value;
      console.log(data);
    }
  }
  ngOnInit(): void {
  }
}
