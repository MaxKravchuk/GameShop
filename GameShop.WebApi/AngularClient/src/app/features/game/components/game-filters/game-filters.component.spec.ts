import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameFiltersComponent } from './game-filters.component';

describe('GameFiltersComponent', () => {
  let component: GameFiltersComponent;
  let fixture: ComponentFixture<GameFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GameFiltersComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
