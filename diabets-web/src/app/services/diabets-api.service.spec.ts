import { TestBed } from '@angular/core/testing';

import { DiabetsApiService } from './diabets-api.service';

describe('DiabetsApiService', () => {
  let service: DiabetsApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DiabetsApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
