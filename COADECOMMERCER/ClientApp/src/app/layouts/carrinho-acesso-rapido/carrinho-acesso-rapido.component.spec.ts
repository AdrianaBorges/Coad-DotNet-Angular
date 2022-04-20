import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CarrinhoAcessoRapidoComponent } from './carrinho-acesso-rapido.component';

describe('CarrinhoAcessoRapidoComponent', () => {
  let component: CarrinhoAcessoRapidoComponent;
  let fixture: ComponentFixture<CarrinhoAcessoRapidoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CarrinhoAcessoRapidoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CarrinhoAcessoRapidoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
