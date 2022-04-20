
using Newtonsoft.Json;

namespace COAD.COBRANCA.Bancos.Model.DTO
{
    public class BradescoCobrancaRequest
    {
        [JsonProperty("nuCPFCNPJ")]
        public string nuCPFCNPJ { get; set; } = "0";

        [JsonProperty("filialCPFCNPJ")]
        public string filialCPFCNPJ { get; set; } = "";

        [JsonProperty("ctrlCPFCNPJ")]
        public string ctrlCPFCNPJ { get; set; } = "";

        [JsonProperty("cdTipoAcesso")]
        public const string cdTipoAcesso = "2";

        [JsonProperty("clubBanco")]
        public const string clubBanco = "0";
        /// <summary>
        /// Este campo não é obrigatório, mas quando informado segundo o manual o valor fixo é "48"
        /// </summary>
        [JsonProperty("cdTipoContrato", Required = Required.Always)]
        public  string cdTipoContrato = "0";

        [JsonProperty("nuSequenciaContrato", Required = Required.Always)]
        public string nuSequenciaContrato { get; set; } = "0";

        [JsonProperty("idProduto", Required = Required.Always)]
        public string idProduto { get; set; } = "09";

        [JsonProperty("nuNegociacao", Required = Required.Always)]
        public string nuNegociacao { get; set; } = "";

        [JsonProperty("cdBanco", Required = Required.Always)]
        public string cdBanco = "237";

        [JsonProperty("eNuSequenciaContrato", Required = Required.Always)]
        public string eNuSequenciaContrato { get; set; } = "0";

        [JsonProperty("tpRegistro", Required = Required.Always)]
        public const string tpRegistro = "1";

        [JsonProperty("cdProduto", Required = Required.Always)]
        public string cdProduto { get; set; } = "0";

        [JsonProperty("nuTitulo", Required = Required.Always)]
        public string nuTitulo { get; set; } = "0";

        [JsonProperty("nuCliente", Required = Required.Always)]
        public string nuCliente { get; set; } = "";

        [JsonProperty("dtEmissaoTitulo", Required = Required.Always)]
        public string dtEmissaoTitulo { get; set; } = "";

        [JsonProperty("dtVencimentoTitulo", Required = Required.Always)]
        public string dtVencimentoTitulo { get; set; } = "";

        [JsonProperty("tpVencimento", Required = Required.Always)]
        public const string tpVencimento = "0";

        [JsonProperty("vlNominalTitulo", Required = Required.Always)]
        public string vlNominalTitulo { get; set; } = "0";

        /// <summary>
        ///  01 CH  CHEQUE
        ///  02 DM  DUPLICATA DE VENDA MERCANTIL
        ///  03 DMI DUPLICATA MERCANTIL POR INDICACAO
        ///  04 DS  DUPLICATA DE PRESTACAO DE SERVICOS
        ///  05 DSI DUPLICATA PREST.SERVICOS POR INDICACAO
        ///  06 DR  DUPLICATA RURAL
        ///  07 LC  LETRA DE CAMBIO
        ///  08 NCC NOTA DE CREDITO COMERCIAL
        ///  09 NCE NOTA DE CREDITO EXPORTACAO
        ///  10 NCI NOTA DE CREDITO INDUSTRIAL
        ///  11 NCR NOTA DE CREDITO RURAL
        ///  12 NP  NOTA PROMISSORIA
        ///  13 NPR NOTA PROMISSORIA RURAL
        ///  14 TM  TRIPLICATA DE VENDA MERCANTIL
        ///  15 TS  TRIPLICATA DE PRESTACAO DE SERVICOS
        ///  16 NS  NOTA DE SERVICO
        ///  17 RC  RECIBO
        ///  18 FAT FATURA
        ///  19 ND  NOTA DE DEBITO
        ///  20 AP  APOLICE DE SEGURO
        ///  21 ME  MENSALIDADE ESCOLAR
        ///  22 PC  PARCELA DE CONSORCIO
        ///  23 DD  DOCUMENTO DE DIVIDA
        ///  24 CCB CEDULA DE CREDITO BANCARIO
        ///  25 FI  FINANCIAMENTO
        ///  26 RD  RATEIO DE DESPESAS
        ///  27 DRI DUPLICATA RURAL INDICACAO
        ///  28 EC  ENCARGOS CONDOMINIAIS
        ///  29 ECI ENCARGOS CONDOMINIAIS POR INDICACAO
        ///  31 CC  CARTAO DE CREDITO
        ///  32 BDP BOLETO DE PROPOSTA
        ///  99 OUT OUTROS
        /// </summary>
        [JsonProperty("cdEspecieTitulo", Required = Required.Always)]
        public string cdEspecieTitulo { get; set; } = "04";

        [JsonProperty("tpProtestoAutomaticoNegativacao", Required = Required.Always)]
        public string tpProtestoAutomaticoNegativacao { get; set; } = "0";

        [JsonProperty("prazoProtestoAutomaticoNegativacao", Required = Required.Always)]
        public string prazoProtestoAutomaticoNegativacao { get; set; } = "0";

        [JsonProperty("controleParticipante", Required = Required.Always)]
        public string controleParticipante { get; set; } = "";

        [JsonProperty("cdPagamentoParcial", Required = Required.Always)]
        public string cdPagamentoParcial { get; set; } = "";

        [JsonProperty("qtdePagamentoParcial", Required = Required.Always)]
        public string qtdePagamentoParcial { get; set; } = "0";

        [JsonProperty("percentualJuros", Required = Required.Always)]
        public string percentualJuros { get; set; } = "0";

        [JsonProperty("vlJuros", Required = Required.Always)]
        public string vlJuros { get; set; } = "0";

        [JsonProperty("qtdeDiasJuros", Required = Required.Always)]
        public string qtdeDiasJuros { get; set; } = "0";

        [JsonProperty("percentualMulta", Required = Required.Always)]
        public string percentualMulta { get; set; } = "0";

        [JsonProperty("vlMulta", Required = Required.Always)]
        public string vlMulta { get; set; } = "0";

        [JsonProperty("qtdeDiasMulta", Required = Required.Always)]
        public string qtdeDiasMulta { get; set; } = "0";

        [JsonProperty("percentualDesconto1", Required = Required.Always)]
        public string percentualDesconto1 { get; set; } = "0";

        [JsonProperty("vlDesconto1", Required = Required.Always)]
        public string vlDesconto1 { get; set; } = "0";

        [JsonProperty("dataLimiteDesconto1", Required = Required.Always)]
        public string dataLimiteDesconto1 { get; set; } = "";

        [JsonProperty("percentualDesconto2", Required = Required.Always)]
        public string percentualDesconto2 { get; set; } = "0";

        [JsonProperty("vlDesconto2", Required = Required.Always)]
        public string vlDesconto2 { get; set; } = "0";

        [JsonProperty("dataLimiteDesconto2", Required = Required.Always)]
        public string dataLimiteDesconto2 { get; set; } = "";

        [JsonProperty("percentualDesconto3", Required = Required.Always)]
        public string percentualDesconto3 { get; set; } = "0";

        [JsonProperty("vlDesconto3", Required = Required.Always)]
        public string vlDesconto3 { get; set; } = "0";

        [JsonProperty("dataLimiteDesconto3", Required = Required.Always)]
        public string dataLimiteDesconto3 { get; set; } = "";

        [JsonProperty("prazoBonificacao", Required = Required.Always)]
        public string prazoBonificacao { get; set; } = "0";

        [JsonProperty("percentualBonificacao", Required = Required.Always)]
        public string percentualBonificacao { get; set; } = "0";

        [JsonProperty("vlBonificacao", Required = Required.Always)]
        public string vlBonificacao { get; set; } = "0";

        [JsonProperty("dtLimiteBonificacao", Required = Required.Always)]
        public string dtLimiteBonificacao { get; set; } = "";

        [JsonProperty("vlAbatimento", Required = Required.Always)]
        public string vlAbatimento { get; set; } = "0";

        [JsonProperty("vlIOF", Required = Required.Always)]
        public string vlIOF { get; set; } = "0";

        [JsonProperty("nomePagador", Required = Required.Always)]
        public string nomePagador { get; set; }

        [JsonProperty("logradouroPagador", Required = Required.Always)]
        public string logradouroPagador { get; set; } = "";

        [JsonProperty("nuLogradouroPagador", Required = Required.Always)]
        public string nuLogradouroPagador { get; set; } = "";

        [JsonProperty("complementoLogradouroPagador", Required = Required.Always)]
        public string complementoLogradouroPagador { get; set; } = "";

        [JsonProperty("cepPagador", Required = Required.Always)]
        public string cepPagador { get; set; } = "0";

        [JsonProperty("complementoCepPagador", Required = Required.Always)]
        public string complementoCepPagador { get; set; } = "0";

        [JsonProperty("bairroPagador", Required = Required.Always)]
        public string bairroPagador { get; set; } = "";

        [JsonProperty("municipioPagador", Required = Required.Always)]
        public string municipioPagador { get; set; } = "";

        [JsonProperty("ufPagador", Required = Required.Always)]
        public string ufPagador { get; set; } = "";

        [JsonProperty("cdIndCpfcnpjPagador", Required = Required.Always)]
        public string cdIndCpfcnpjPagador { get; set; } = "";

        [JsonProperty("nuCpfcnpjPagador", Required = Required.Always)]
        public string nuCpfcnpjPagador { get; set; } = "";

        [JsonProperty("endEletronicoPagador", Required = Required.Always)]
        public string endEletronicoPagador { get; set; } = "";

        [JsonProperty("nomeSacadorAvalista", Required = Required.Always)]
        public string nomeSacadorAvalista { get; set; } = "";

        [JsonProperty("logradouroSacadorAvalista", Required = Required.Always)]
        public string logradouroSacadorAvalista { get; set; } = "";

        [JsonProperty("nuLogradouroSacadorAvalista", Required = Required.Always)]
        public string nuLogradouroSacadorAvalista { get; set; } = "";

        [JsonProperty("complementoLogradouroSacadorAvalista", Required = Required.Always)]
        public string complementoLogradouroSacadorAvalista { get; set; } = "";

        [JsonProperty("cepSacadorAvalista", Required = Required.Always)]
        public string cepSacadorAvalista { get; set; } = "0";

        [JsonProperty("complementoCepSacadorAvalista", Required = Required.Always)]
        public string complementoCepSacadorAvalista { get; set; } = "0";

        [JsonProperty("bairroSacadorAvalista", Required = Required.Always)]
        public string bairroSacadorAvalista { get; set; } = "";

        [JsonProperty("municipioSacadorAvalista", Required = Required.Always)]
        public string municipioSacadorAvalista { get; set; } = "";

        [JsonProperty("ufSacadorAvalista", Required = Required.Always)]
        public string ufSacadorAvalista { get; set; } = "";

        [JsonProperty("cdIndCpfcnpjSacadorAvalista", Required = Required.Always)]
        public string cdIndCpfcnpjSacadorAvalista { get; set; } = "0";

        [JsonProperty("nuCpfcnpjSacadorAvalista", Required = Required.Always)]
        public string nuCpfcnpjSacadorAvalista { get; set; } = "0";

        [JsonProperty("endEletronicoSacadorAvalista", Required = Required.Always)]
        public string endEletronicoSacadorAvalista { get; set; } = "";
        
    }
}