import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LojaLayoutComponent } from './loja-layout.component';

describe('LojaLayoutComponent', () => {
  let component: LojaLayoutComponent;
  let fixture: ComponentFixture<LojaLayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LojaLayoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LojaLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
