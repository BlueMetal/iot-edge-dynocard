import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DynoCardComponent } from './dynocard.component';

describe('DynoCardComponent', () => {
  let component: DynoCardComponent;
  let fixture: ComponentFixture<DynoCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DynoCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DynoCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
