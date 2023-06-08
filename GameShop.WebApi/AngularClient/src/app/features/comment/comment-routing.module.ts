import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BanCommentComponent } from "./components/ban-comment/ban-comment.component";
import { RoleGuard } from "../../core/guards/role/role.guard";

const routes: Routes = [
    {
        path: 'ban',
        component: BanCommentComponent,
        canActivate: [RoleGuard],
        data: {
            allowedRoles: ['Moderator']
        }
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class CommentRoutingModule {
}
