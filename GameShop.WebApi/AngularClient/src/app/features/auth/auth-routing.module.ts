import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthMainComponent } from "./components/auth-main/auth-main.component";
import { LoginGuard } from "../../core/guards/login/login.guard";

const routes: Routes = [
    {
        path: 'login',
        component: AuthMainComponent,
        canActivate: [LoginGuard]
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
