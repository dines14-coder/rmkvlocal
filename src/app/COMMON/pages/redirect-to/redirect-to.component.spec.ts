import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { RedirectToComponent } from './redirect-to.component';

describe('RedirectToComponent', () => {
  let component: RedirectToComponent;
  let fixture: ComponentFixture<RedirectToComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RedirectToComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RedirectToComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
