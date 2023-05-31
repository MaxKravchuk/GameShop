import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDeleteEditComponent } from './user-delete-edit.component';

describe('UserEditComponent', () => {
  let component: UserDeleteEditComponent;
  let fixture: ComponentFixture<UserDeleteEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserDeleteEditComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserDeleteEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
