import { TestBed } from '@angular/core/testing';

import { JwtDecodeServiceService } from './jwtDecode.service';

describe('JwtDecodeServiceService', () => {
  let service: JwtDecodeServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JwtDecodeServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
