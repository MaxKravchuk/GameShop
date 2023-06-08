import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GameDetailsComponent } from "./components/game-details/game-details.component";

const routes: Routes = [
    {
        path: 'details/:Key',
        component: GameDetailsComponent,
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class GameRoutingModule {
}
