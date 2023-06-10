import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {

    @Input()HasNext?: boolean;

    @Input()HasPrevious?: boolean;

    @Input()maxPageSize?: number;

    @Output()pageSizeEvent: EventEmitter<number> = new EventEmitter<number>();

    @Output()pageIndexEvent: EventEmitter<number> = new EventEmitter<number>();

    pageSize: number = 10;

    pageIndex: number = 1;

    pageSizeOptions: string[] = ['1', '10', '20', '50', '100', 'All'];

    constructor() { }

    ngOnInit(): void {
    }

    pageSizeChange(value: any): void {
        if (value.target.value === 'All') {
            this.pageSize = this.maxPageSize!;
        }
        else {
            this.pageSize = Number(value.target.value);
        }
        this.pageSizeEvent.emit(this.pageSize);
    }

    previousPage(): void {
        this.pageIndex--;
        this.pageIndexEvent.emit(this.pageIndex);
    }

    nextPage(): void {
        this.pageIndex++;
        this.pageIndexEvent.emit(this.pageIndex);
    }
}



