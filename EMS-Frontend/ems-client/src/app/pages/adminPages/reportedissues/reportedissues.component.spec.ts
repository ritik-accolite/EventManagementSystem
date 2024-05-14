import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportedissuesComponent } from './reportedissues.component';

describe('ReportedissuesComponent', () => {
  let component: ReportedissuesComponent;
  let fixture: ComponentFixture<ReportedissuesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReportedissuesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ReportedissuesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
