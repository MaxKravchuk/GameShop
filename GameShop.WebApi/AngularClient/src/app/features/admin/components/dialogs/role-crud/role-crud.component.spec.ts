import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleCrudComponent } from './role-crud.component';

describe('RoleEditComponent', () => {
  let component: RoleCrudComponent;
  let fixture: ComponentFixture<RoleCrudComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoleCrudComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoleCrudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
