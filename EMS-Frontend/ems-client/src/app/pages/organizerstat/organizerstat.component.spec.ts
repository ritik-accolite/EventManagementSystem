import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizerstatComponent } from './organizerstat.component';

describe('OrganizerstatComponent', () => {
  let component: OrganizerstatComponent;
  let fixture: ComponentFixture<OrganizerstatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrganizerstatComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OrganizerstatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
