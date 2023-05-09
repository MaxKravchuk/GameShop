import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { Publisher } from "../../../../core/models/Publisher";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
    selector: 'app-publisher-create',
    templateUrl: './publisher-create.component.html',
    styleUrls: ['./publisher-create.component.css']
})
export class PublisherCreateComponent implements OnInit {

    private urlRegex = /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w.-]+)+[\w\-._~:/?#[\]@!$&'()*+,;=]+$/;

    form!: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private publisherService: PublisherService,
        private utilsService: UtilsService
    ) {}

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            CompanyName: ['', [Validators.required, Validators.maxLength(40)]],
            Description: ['', Validators.required],
            HomePage: ['', [Validators.required, Validators.pattern(this.urlRegex)]]
        });
    }

    onNoClick(): void {
        this.utilsService.goBack();
    }

    onSaveForm(): void {
        if (!this.form.valid) {
            this.utilsService.openWithMessage('Form is invalid!');
        }

        const data: Publisher = this.form.value as Publisher;
        this.publisherService.createPublisher(data).subscribe({
            next: (): void => {
                this.utilsService.goBack();
            }
        });
    }
}
