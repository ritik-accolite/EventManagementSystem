import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserticketComponent } from './userticket.component';

describe('UserticketComponent', () => {
  let component: UserticketComponent;
  let fixture: ComponentFixture<UserticketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserticketComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserticketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
