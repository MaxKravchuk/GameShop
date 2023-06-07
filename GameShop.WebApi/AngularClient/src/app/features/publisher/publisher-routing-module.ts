import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { PublisherDetailsComponent } from "./components/publisher-details/publisher-details.component";
import { PublisherCreateComponent } from "./components/publisher-create/publisher-create.component";
import { PublisherMainComponent } from "./components/publisher-main/publisher-main.component";

const routes: Routes = [
    {
        path: 'details/:CompanyName',
        component: PublisherDetailsComponent
    },
    {
        path: 'create',
        component: PublisherCreateComponent
    },
    {
        path: 'main',
        component: PublisherMainComponent
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
