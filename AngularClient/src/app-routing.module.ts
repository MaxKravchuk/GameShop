import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";

const routes: Routes = [
  {
    path: 'games',
    loadChildren: ()=> import('./app/features/game/game.module').then(m=>m.GameModule)
  },
  {
    path:'publishers',
    loadChildren: ()=>import('./app/features/publisher/publisher.module').then(m=>m.PublisherModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
