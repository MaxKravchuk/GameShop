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
  },
  {
    path:'cart',
    loadChildren: ()=>import('./app/features/shopping-cart/shopping-cart.module').then(m=>m.ShoppingCartModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
