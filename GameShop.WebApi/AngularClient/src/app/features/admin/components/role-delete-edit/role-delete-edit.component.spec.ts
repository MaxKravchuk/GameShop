import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleDeleteEditComponent } from './role-delete-edit.component';

describe('RoleEditComponent', () => {
  let component: RoleDeleteEditComponent;
  let fixture: ComponentFixture<RoleDeleteEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoleDeleteEditComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoleDeleteEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
