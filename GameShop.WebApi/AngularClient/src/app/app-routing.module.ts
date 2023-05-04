import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MainComponent } from "./layout/main/main.component";

const routes: Routes = [
    {
      path: '',
      component: MainComponent
    },
    {
        path: 'games',
        loadChildren: () => import('./features/game/game.module').then(m => m.GameModule)
    },
    {
        path: 'publishers',
        loadChildren: () => import('./features/publisher/publisher.module').then(m => m.PublisherModule)
    },
    {
        path: 'cart',
        loadChildren: () => import('./features/cart/cart.module').then(m => m.CartModule)
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule {
}
