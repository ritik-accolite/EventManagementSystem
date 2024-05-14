import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrackeventComponent } from './trackevent.component';

describe('TrackeventComponent', () => {
  let component: TrackeventComponent;
  let fixture: ComponentFixture<TrackeventComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrackeventComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TrackeventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
