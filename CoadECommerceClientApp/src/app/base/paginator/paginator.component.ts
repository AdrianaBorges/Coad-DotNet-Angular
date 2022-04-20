import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { Pagina } from '../models/page';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.css']
})
export class PaginatorComponent implements OnInit {


  @Input()
  paginatorClass: string;

  @Input()
  fragment: string;
  private _pagina: Pagina;
  @Output() paginaChange: EventEmitter<any> = new EventEmitter();
  lstPaginas: number[];

  @Input()
  set pagina(pagina: Pagina) {

    this._pagina = pagina;
    if (this._pagina) {
      this.montarLista(this._pagina.pagina, this._pagina.numeroPaginas);
    }
  }

  get pagina(): Pagina {

    return this._pagina;
  }

  ngOnInit() {
  }

  constructor(
    private router: Router,
    private activRouter: ActivatedRoute) {

  }

  private montarLista(paginaAtual: number, numeroPaginas: number): void {

    if (paginaAtual && this._pagina) {
      this._pagina.pagina = paginaAtual;
    }

    const lstPaginas = [] as number[];
    let index = 1;

    let ate = (numeroPaginas > 8) ? 9 : numeroPaginas;

    if (paginaAtual >= 8 && paginaAtual <= numeroPaginas) { // se a pagina atual passar o limite de 8 fragmenta a lista de paginação

      // inclui na lista as primeiras duas paginas
      lstPaginas.push(1);
      lstPaginas.push(2);
      // pagina negativa indica que será inserido um ... no link da paginação ou seja não será clicavel
      lstPaginas.push(-1);

      // pega pagina atual e mostra 3 páginas antes e 3 páginas depois da pagina atual
      index = paginaAtual - 3;
      ate = paginaAtual + 3;

      if (numeroPaginas <= ate) {

        ate = numeroPaginas;
      }
    }

    for (index; index <= ate; index++) {

      lstPaginas.push(index);
    }
    if (ate + 1 < numeroPaginas) {

      lstPaginas.push(-2);
      lstPaginas.push(numeroPaginas - 1);
      lstPaginas.push(numeroPaginas);
    }
    this.lstPaginas = lstPaginas;
  }

  voltarPagina($event): void {

    $event.preventDefault();
    if (this._pagina && this._pagina.pagina > 1) {
      this.goToPage(this._pagina.pagina - 1);
    }
  }

  avancarPagina($event): void {

    $event.preventDefault();
    if (this._pagina && this._pagina.pagina < this._pagina.numeroPaginas) {
      this.goToPage(this._pagina.pagina + 1);
    }

  }

  goToPage(page: number, $event?) {

    if ($event) {
        $event.preventDefault();
    }

    if (this.fragment) {

      this.router.navigate([''], {
        relativeTo: this.activRouter,
        fragment: this.fragment
      });
    }

    if (this._pagina) {
      this.montarLista(page, this._pagina.numeroPaginas);
      this.paginaChange.emit(page);
    }
  }
}
