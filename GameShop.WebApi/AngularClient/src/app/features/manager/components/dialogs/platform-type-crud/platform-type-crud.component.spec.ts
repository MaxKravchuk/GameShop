import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlatformTypeCrudComponent } from './platform-type-crud.component';

describe('PlatformTypeCrudComponent', () => {
  let component: PlatformTypeCrudComponent;
  let fixture: ComponentFixture<PlatformTypeCrudComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlatformTypeCrudComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlatformTypeCrudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
