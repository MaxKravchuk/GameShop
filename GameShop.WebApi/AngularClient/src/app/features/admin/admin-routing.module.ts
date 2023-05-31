import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AdminMainComponent } from "./components/admin-main/admin-main.component";
import { RoleGuard } from "../../core/guards/role/role.guard";

const routes: Routes = [
    {
        path: 'main',
        component: AdminMainComponent,
        canActivate: [RoleGuard],
        data: {
            allowedRoles: ['Administrator']
        }
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class AdminRoutingModule {
}
