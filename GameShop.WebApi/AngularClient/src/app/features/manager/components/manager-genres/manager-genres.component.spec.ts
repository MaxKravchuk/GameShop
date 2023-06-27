import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerGenresComponent } from './manager-genres.component';

describe('ManagerGenresComponent', () => {
  let component: ManagerGenresComponent;
  let fixture: ComponentFixture<ManagerGenresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManagerGenresComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerGenresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
