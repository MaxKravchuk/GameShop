import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BanCommentComponent } from "./components/ban-comment/ban-comment.component";

const routes: Routes = [
    {
        path: 'ban',
        component: BanCommentComponent,
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
