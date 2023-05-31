import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthMainComponent } from "./components/auth-main/auth-main.component";
import { UnAuthGuard } from "../../core/guards/unauth/unauth.guard";

const routes: Routes = [
    {
        path: 'login',
        component: AuthMainComponent,
        canActivate: [UnAuthGuard]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class AuthRoutingModule {
}
