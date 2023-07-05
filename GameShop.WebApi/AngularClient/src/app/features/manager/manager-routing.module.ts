import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { RoleGuard } from "../../core/guards/role/role.guard";
import { ManagerGamesComponent } from "./components/manager-games/manager-games.component";
import { OrdersMainComponent } from "./components/orders-main/orders-main.component";
import { ManagerGenresComponent } from "./components/manager-genres/manager-genres.component";
import { ManagerPlatformTypeComponent } from "./components/manager-platform-type/manager-platform-type.component";
import { ManagerPublisherComponent } from "./components/manager-publisher/manager-publisher.component";

const routes: Routes = [
    {
        path: 'managerGames',
        component: ManagerGamesComponent,
        canActivate: [RoleGuard],
        data: {
            allowedRoles: ['Manager']
        }
    },
    {
        path: 'managerGenres',
        component: ManagerGenresComponent,
        canActivate: [RoleGuard],
        data: {
            allowedRoles: ['Manager']
        }
    },
    {
        path: 'managerPlatforms',
        component: ManagerPlatformTypeComponent,
        canActivate: [RoleGuard],
        data: {
            allowedRoles: ['Manager']
        }
    },
    {
        path: 'managerPublishers',
        component: ManagerPublisherComponent,
        canActivate: [RoleGuard],
        data: {
            allowedRoles: ['Manager']
        }
    },
    {
        path: 'orders',
        component: OrdersMainComponent,
        canActivate: [RoleGuard],
        data: {
            allowedRoles: ['Manager']
        }
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class ManagerRoutingModule {
}
