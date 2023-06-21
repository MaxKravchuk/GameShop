import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { PublisherDetailsComponent } from "./components/publisher-details/publisher-details.component";
import { PublisherMainComponent } from "./components/publisher-main/publisher-main.component";
import { RoleGuard } from "../../core/guards/role/role.guard";

const routes: Routes = [
    {
        path: 'details/:CompanyName',
        component: PublisherDetailsComponent,
    },
    {
        path: 'main',
        component: PublisherMainComponent,
        canActivate: [RoleGuard],
        data: {
            allowedRoles: ['Publisher']
        }
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class PublisherRoutingModule {
}
