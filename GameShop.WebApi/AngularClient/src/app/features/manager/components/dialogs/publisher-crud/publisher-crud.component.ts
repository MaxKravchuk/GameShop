import { Component, Inject, OnInit } from '@angular/core';
import { Publisher } from "../../../../../core/models/Publisher";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ManagerGamesComponent } from "../../manager-games/manager-games.component";
import { UtilsService } from "../../../../../core/services/helpers/utilsService/utils-service";
import { PublisherService } from "../../../../../core/services/publisherService/publisher.service";

@Component({
  selector: 'app-publisher-crud',
  templateUrl: './publisher-crud.component.html',
  styleUrls: ['./publisher-crud.component.scss']
})
export class PublisherCrudComponent implements OnInit {

    isAdding: boolean = false;

    publisher!: Publisher;

    form!: FormGroup;

    private urlRegex: RegExp = /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w.-]+)+[\w\-._~:/?#[\]@!$&'()*+,;=]+$/;

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {publisher: Publisher},
        private dialogRef: MatDialogRef<ManagerGamesComponent>,
        private publisherService: PublisherService,
        private utilsService: UtilsService
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            CompanyName: ['', [Validators.required, Validators.maxLength(40)]],
            Description: ['', Validators.required],
            HomePage: ['', [Validators.required, Validators.pattern(this.urlRegex)]]
        });

        if (this.data.publisher == null) {
            this.isAdding = true;
        }
        else {
            this.publisher = this.data.publisher;
            this.form.patchValue(this.data.publisher);
        }
    }


    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onSaveClick(): void{
        const newPublisher: Publisher = this.form.value as Publisher;

        this.publisherService.createPublisher(newPublisher).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage('Publisher created successfully!');
                this.dialogRef.close(true);
            }
        });
    }

    onEditClick(): void {
        const newPublisher: Publisher = this.form.value as Publisher;
        newPublisher.Id = this.publisher?.Id;

        this.publisherService.updatePublisher(newPublisher).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage('Publisher updated successfully!');
                this.dialogRef.close(true);
            }
        });
    }

    onDeleteClick(): void {
        this.publisherService.deletePublisher(this.publisher.Id!).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage('Publisher deleted successfully!');
                this.dialogRef.close(true);
            }
        });
    }
}
