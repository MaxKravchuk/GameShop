import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlatformCreateComponent } from './platform-create.component';

describe('PlatformCreateComponent', () => {
  let component: PlatformCreateComponent;
  let fixture: ComponentFixture<PlatformCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlatformCreateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlatformCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
