import { TestBed, inject } from '@angular/core/testing';

import { CarrinhoComprasService } from './carrinho-compras.service';

describe('CarrinhoComprasService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CarrinhoComprasService]
    });
  });

  it('should be created', inject([CarrinhoComprasService], (service: CarrinhoComprasService) => {
    expect(service).toBeTruthy();
  }));
});
