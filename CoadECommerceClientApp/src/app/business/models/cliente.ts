import { ClienteEndereco } from './cliente-endereco';
import { TipoCliente } from './tipo-cliente';

export class Cliente {

  clI_ID: number;
  clI_NOME: string;
  tipO_CLI_ID: number;
  clI_CPF_CNPJ: string;
  tipoCliente: TipoCliente;

  enderecoCadastro: ClienteEndereco;
  enderecoEntrega: ClienteEndereco;
}

