import {Component, OnInit} from '@angular/core';
import {PublisherService} from "../../../../core/services/publisherService/publisher.service";
import {Publisher} from "../../../../core/models/Publisher";
import {ActivatedRoute} from "@angular/router";
import {Game} from "../../../../core/models/Game";

@Component({
  selector: 'app-publisher-main',
  templateUrl: './publisher-main.component.html',
  styleUrls: ['./publisher-main.component.css']
})
export class PublisherMainComponent implements OnInit{

  publisher?: Publisher;
  companyName?:string | null;
  games?: Game[] = [];
  ngOnInit(): void {
    if (this.companyName != null){
      this.getPublisherByCompanyName();
    }
    else{
      this.publisherService.goToPrevPage();
    }
  }

  constructor(
    private publisherService: PublisherService,
    private activeRoute: ActivatedRoute) {
    this.companyName = this.activeRoute.snapshot.paramMap.get('CompanyName');
  }

  getPublisherByCompanyName(){
    this.publisherService.getPublisherByCompanyName(this.companyName!).subscribe(
      (data)=>{
        this.publisher = data;
        this.games = data.GameReadListDTOs;
      }
    );
  }

}
