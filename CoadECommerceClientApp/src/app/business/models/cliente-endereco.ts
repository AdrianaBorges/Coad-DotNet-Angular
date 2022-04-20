import { Municipio } from './municipio';

export class ClienteEndereco {

    clI_ID: number;
    enD_TIPO: number;
    enD_LOGRADOURO: string;
    enD_COMPLEMENTO: string;
    enD_NUMERO: string;
    enD_BAIRRO: string;
    enD_CEP: string;
    enD_MUNICIPIO: string;
    muN_ID: number;
    enD_UF: string;
    telefone: string;
    celular: string;
    municipio: Municipio;
}
