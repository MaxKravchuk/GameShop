import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {PublisherService} from "../../../../core/services/publisherService/publisher.service";
import {Publisher} from "../../../../core/models/Publisher";

@Component({
  selector: 'app-publisher-create',
  templateUrl: './publisher-create.component.html',
  styleUrls: ['./publisher-create.component.css']
})
export class PublisherCreateComponent implements OnInit {

    constructor(
      @Inject(FormBuilder) private formBuilder: FormBuilder,
      private publisherService: PublisherService
    ) { }

    ngOnInit(): void {
    }

    form = new FormGroup({
      CompanyName: this.formBuilder.control('',Validators.required),
      Description: this.formBuilder.control('',Validators.required),
      HomePage: this.formBuilder.control('',Validators.required),
    });

    onNoClick() {
      this.publisherService.goToPrevPage();
    }

    onSaveForm() {
      if(this.form.valid){
        const data: Publisher = this.form.value as Publisher;
        this.publisherService.createPublisher(data)
          .subscribe(()=>this.publisherService.goToPrevPage());
      }
    }
}
