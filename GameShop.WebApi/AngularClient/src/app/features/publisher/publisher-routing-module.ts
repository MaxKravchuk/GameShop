import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { PublisherDetailsComponent } from "./components/publisher-details/publisher-details.component";
import { PublisherCreateComponent } from "./components/publisher-create/publisher-create.component";

const routes: Routes = [
    {
        path: 'details/:CompanyName',
        component: PublisherDetailsComponent
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
