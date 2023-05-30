import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthMainComponent } from "./components/auth-main/auth-main.component";

const routes: Routes = [
    {
        path: 'login',
        component: AuthMainComponent
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
