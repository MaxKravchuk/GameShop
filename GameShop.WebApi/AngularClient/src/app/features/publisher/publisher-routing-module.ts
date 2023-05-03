import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { PublisherMainComponent } from "./components/publisher-main/publisher-main.component";
import { PublisherCreateComponent } from "./components/publisher-create/publisher-create.component";

const routes: Routes = [
    {
        path: 'details/:CompanyName',
        component: PublisherMainComponent
    },
    {
        path: 'create',
        component: PublisherCreateComponent
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
