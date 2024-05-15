import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventbylocationComponent } from './eventbylocation.component';

describe('EventbylocationComponent', () => {
  let component: EventbylocationComponent;
  let fixture: ComponentFixture<EventbylocationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EventbylocationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EventbylocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
