import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerPublisherComponent } from './manager-publisher.component';

describe('ManagerPublisherComponent', () => {
  let component: ManagerPublisherComponent;
  let fixture: ComponentFixture<ManagerPublisherComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManagerPublisherComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerPublisherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
