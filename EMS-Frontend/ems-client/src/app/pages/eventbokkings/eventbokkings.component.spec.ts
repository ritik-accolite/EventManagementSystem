import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventbokkingsComponent } from './eventbokkings.component';

describe('EventbokkingsComponent', () => {
  let component: EventbokkingsComponent;
  let fixture: ComponentFixture<EventbokkingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EventbokkingsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EventbokkingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
