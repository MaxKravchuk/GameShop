import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { RoleGuard } from "../../core/guards/role/role.guard";
import { ManagerMainComponent } from "./components/manager-main/manager-main.component";
import { OrdersMainComponent } from "./components/orders-main/orders-main.component";

const routes: Routes = [
    {
        path: 'managerPanel',
        component: ManagerMainComponent,
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
