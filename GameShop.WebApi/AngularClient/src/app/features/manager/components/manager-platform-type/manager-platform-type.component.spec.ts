import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerPlatformTypeComponent } from './manager-platform-type.component';

describe('ManagerPlatformTypeComponent', () => {
  let component: ManagerPlatformTypeComponent;
  let fixture: ComponentFixture<ManagerPlatformTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManagerPlatformTypeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerPlatformTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
