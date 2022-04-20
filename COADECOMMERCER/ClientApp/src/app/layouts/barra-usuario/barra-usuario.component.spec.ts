import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BarraUsuarioComponent } from './barra-usuario.component';

describe('BarraUsuarioComponent', () => {
  let component: BarraUsuarioComponent;
  let fixture: ComponentFixture<BarraUsuarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BarraUsuarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BarraUsuarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
