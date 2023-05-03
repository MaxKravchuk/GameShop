import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublisherMainComponent } from './publisher-main.component';

describe('PublisherMainComponent', () => {
    let component: PublisherMainComponent;
    let fixture: ComponentFixture<PublisherMainComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [PublisherMainComponent]
        })
            .compileComponents();

        fixture = TestBed.createComponent(PublisherMainComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
