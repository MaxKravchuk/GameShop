import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerGamesComponent } from './manager-games.component';

describe('ManagerMainComponent', () => {
  let component: ManagerGamesComponent;
  let fixture: ComponentFixture<ManagerGamesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManagerGamesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerGamesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
