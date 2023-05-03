import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { Publisher } from "../../../../core/models/Publisher";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
    selector: 'app-publisher-create',
    templateUrl: './publisher-create.component.html',
    styleUrls: ['./publisher-create.component.css']
})
export class PublisherCreateComponent {

    form = new FormGroup({
        CompanyName: this.formBuilder.control('', Validators.required),
        Description: this.formBuilder.control('', Validators.required),
        HomePage: this.formBuilder.control('', Validators.required),
    });

    constructor(
        @Inject(FormBuilder) private formBuilder: FormBuilder,
        private publisherService: PublisherService,
        private utilsService: UtilsService
    ) {}

    public onNoClick() {
        this.utilsService.goBack();
    }

    public onSaveForm() {
        if (!this.form.valid) {
            this.utilsService.openWithMessage("Form is invalid!");
        }

        const data: Publisher = this.form.value as Publisher;
        this.publisherService.createPublisher(data).subscribe({
            next: () => {
                this.utilsService.goBack();
            }
        });
    }
}
