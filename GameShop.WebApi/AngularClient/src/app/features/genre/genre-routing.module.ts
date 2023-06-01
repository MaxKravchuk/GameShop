import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { GenreCreateComponent } from "./components/genre-create/genre-create.component";
import { RoleGuard } from "../../core/guards/role/role.guard";

const routes: Routes = [
    {
        path: 'create',
        component: GenreCreateComponent,
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
export class GenreRoutingModule {
}
