import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { RoleGuard } from "../../core/guards/role/role.guard";
import { PlatformCreateComponent } from "./components/platform-create/platform-create.component";

const routes: Routes = [
    {
        path: 'create',
        component: PlatformCreateComponent,
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
export class PlatformRoutingModule {
}
