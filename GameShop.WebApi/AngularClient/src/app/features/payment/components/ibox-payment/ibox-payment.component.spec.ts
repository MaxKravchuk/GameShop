import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IboxPaymentComponent } from './ibox-payment.component';

describe('IBoxPaymentComponent', () => {
  let component: IboxPaymentComponent;
  let fixture: ComponentFixture<IboxPaymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IboxPaymentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IboxPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
