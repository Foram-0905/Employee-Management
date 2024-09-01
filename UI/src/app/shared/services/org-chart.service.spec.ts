import { TestBed } from '@angular/core/testing';

import { OrgChartService } from './org-chart.service';

describe('OrgChartService', () => {
  let service: OrgChartService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrgChartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
