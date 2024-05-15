import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserstatComponent } from './userstat.component';

describe('UserstatComponent', () => {
  let component: UserstatComponent;
  let fixture: ComponentFixture<UserstatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserstatComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserstatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
