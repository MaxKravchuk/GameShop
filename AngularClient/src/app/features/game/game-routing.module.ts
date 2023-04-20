import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {GameDetailsComponent} from "./components/game-details/game-details.component";
import {GameCreateComponent} from "./components/game-create/game-create.component";

const routes: Routes = [
  {
    path: 'details/:Key',
    component: GameDetailsComponent,
  },
  {
    path: 'create',
    component: GameCreateComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class GameRoutingModule { }
