import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MainComponent } from "./layout/main/main.component";

const routes: Routes = [
    {
        path: '',
        component: MainComponent,
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
    },
    {
        path: 'order',
        loadChildren: () => import('./features/order/order.module').then(m => m.OrderModule)
    },
    {
        path: 'payment',
        loadChildren: () => import('./features/payment/payment.module').then(m => m.PaymentModule)
    },
    {
        path: 'comment',
        loadChildren: () => import('./features/comment/comment.module').then(m => m.CommentModule)
    },
    {
        path: 'auth',
        loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
    },
    {
        path: 'admin',
        loadChildren: () => import('./features/admin/admin.module').then(m => m.AdminModule)
    },
    {
        path: 'manager',
        loadChildren: () => import('./features/manager/manager.module').then(m => m.ManagerModule)
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule {
}
