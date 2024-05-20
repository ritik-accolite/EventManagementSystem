import { TestBed } from '@angular/core/testing';
import { JwtDecodeService } from './jwtDecode.service';

describe('JwtDecodeService', () => {
  let service: JwtDecodeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JwtDecodeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
