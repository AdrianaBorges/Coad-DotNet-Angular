import { TestBed, inject } from '@angular/core/testing';

import { LayoutScriptsService } from './layout-scripts.service';

describe('LayoutScriptsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LayoutScriptsService]
    });
  });

  it('should be created', inject([LayoutScriptsService], (service: LayoutScriptsService) => {
    expect(service).toBeTruthy();
  }));
});
