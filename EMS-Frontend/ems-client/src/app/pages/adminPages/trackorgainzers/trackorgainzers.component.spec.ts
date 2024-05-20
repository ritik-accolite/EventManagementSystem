import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrackorgainzersComponent } from './trackorgainzers.component';

describe('TrackorgainzersComponent', () => {
  let component: TrackorgainzersComponent;
  let fixture: ComponentFixture<TrackorgainzersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrackorgainzersComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TrackorgainzersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
