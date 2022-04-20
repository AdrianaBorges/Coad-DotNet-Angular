import { Component, OnInit } from '@angular/core';
import { ClienteCadastro } from 'src/app/business/models/cliente-cadastro';
import { ClienteService } from 'src/app/business/services/cliente.service';
import { MessageService } from 'src/app/base/services/message.service';
import { TipoCliente } from 'src/app/business/models/tipo-cliente';

@Component({
  selector: 'app-cadastro',
  templateUrl: './cadastro.component.html',
  styleUrls: ['./cadastro.component.css']
})
export class CadastroComponent implements OnInit {

  cliente: ClienteCadastro;
  lstTiposCliente: TipoCliente[];

  constructor(
      private clienteService: ClienteService,
      private message: MessageService
    ) { }

  ngOnInit() {
    this._initCliente();
    this.listarTiposCliente();
  }

  private _initCliente() {

    this.cliente = new ClienteCadastro();
    this.cliente.tipO_CLI_ID = null;
  }

  listarTiposCliente() {

    const resultTipoCliente = this.clienteService.listarTiposCliente();

    if (resultTipoCliente) {

      resultTipoCliente.subscribe(result => {

        if (result.success) {

          this.lstTiposCliente = result.result;
        } else {
          this.message.adicionarMensagem(result.message);
        }
      });
    }
  }

}
