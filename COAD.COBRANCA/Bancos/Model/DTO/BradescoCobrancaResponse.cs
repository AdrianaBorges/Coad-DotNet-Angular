using Newtonsoft.Json;

namespace COAD.COBRANCA.Bancos.Model.DTO
{
    public class BradescoCobrancaResponse
    {
        [JsonProperty]
        public string cdErro { get; set; }

        [JsonProperty]
        public string msgErro { get; set; }

        [JsonProperty]
        public string idProduto { get; set; }

        [JsonProperty]
        public string negociação { get; set; }

        [JsonProperty]
        public string clubBanco { get; set; }

        [JsonProperty]
        public string tpContrato { get; set; }

        [JsonProperty]
        public string nuSequenciaContrato { get; set; }

        [JsonProperty]
        public string cdProduto { get; set; }

        [JsonProperty]
        public string nuTituloGerado { get; set; }

        [JsonProperty]
        public string agenciaCreditoBeneficiario { get; set; }

        [JsonProperty]
        public string contaCreditoBeneficiario { get; set; }

        [JsonProperty]
        public string digCreditoBeneficiario { get; set; }

        [JsonProperty]
        public string cdCipTitulo { get; set; }

        [JsonProperty]
        public string statusTitulo { get; set; }

        [JsonProperty]
        public string descStatusTitulo { get; set; }

        [JsonProperty]
        public string nomeBeneficiario { get; set; }

        [JsonProperty]
        public string logradouroBeneficiario { get; set; }

        [JsonProperty]
        public string nuLogradouroBeneficiario { get; set; }

        [JsonProperty]
        public string complementoLogradouroBeneficiario { get; set; }

        [JsonProperty]
        public string cepBeneficiario { get; set; }

        [JsonProperty]
        public string cepComplementoBeneficiario { get; set; }

        [JsonProperty]
        public string municipioBeneficiario { get; set; }

        [JsonProperty]
        public string ufBeneficiario { get; set; }

        [JsonProperty]
        public string razaoContaBeneficiario { get; set; }

        [JsonProperty]
        public string nomePagador { get; set; }

        [JsonProperty]
        public string cpfcnpjPagador { get; set; }

        [JsonProperty]
        public string enderecoPagador { get; set; }

        [JsonProperty]
        public string bairroPagador { get; set; }

        [JsonProperty]
        public string municipioPagador { get; set; }

        [JsonProperty]
        public string ufPagador { get; set; }

        [JsonProperty]
        public string cepPagador { get; set; }

        [JsonProperty]
        public string cepComplementoPagador { get; set; }

        [JsonProperty]
        public string endEletronicoPagador { get; set; }

        [JsonProperty]
        public string nomeSacadorAvalista { get; set; }

        [JsonProperty]
        public string cpfcnpjSacadorAvalista { get; set; }

        [JsonProperty]
        public string enderecoSacadorAvalista { get; set; }

        [JsonProperty]
        public string municipioSacadorAvalista { get; set; }

        [JsonProperty]
        public string ufSacadorAvalista { get; set; }

        [JsonProperty]
        public string cepSacadorAvalista { get; set; }

        [JsonProperty]
        public string cepComplementoSacadorAvalista { get; set; }

        [JsonProperty]
        public string numeroTitulo { get; set; }

        [JsonProperty]
        public string dtRegistro { get; set; }

        [JsonProperty]
        public string especieDocumentoTitulo { get; set; }

        [JsonProperty]
        public string descEspecie { get; set; }

        [JsonProperty]
        public string vlIOF { get; set; }

        [JsonProperty]
        public string dtEmissao { get; set; }

        [JsonProperty]
        public string dtVencimento { get; set; }

        [JsonProperty]
        public string vlTitulo { get; set; }

        [JsonProperty]
        public string vlAbatimento { get; set; }

        [JsonProperty]
        public string dtInstrucaoProtestoNegativação { get; set; }

        [JsonProperty]
        public string diasInstrucaoProtestoNegativação { get; set; }

        [JsonProperty]
        public string dtMulta { get; set; }

        [JsonProperty]
        public string vlMulta { get; set; }

        [JsonProperty]
        public string qtdeCasasDecimaisMulta { get; set; }

        [JsonProperty]
        public string cdValorMulta { get; set; }

        [JsonProperty]
        public string descCdMulta { get; set; }

        [JsonProperty]
        public string dtJuros { get; set; }

        [JsonProperty]
        public string vlJurosAoDia { get; set; }
        [JsonProperty]
        public string dtDesconto1Bonificacao { get; set; }
        [JsonProperty]
        public string vlDesconto1Bonificacao { get; set; }
        [JsonProperty]
        public string qtdeCasasDecimaisDesconto1Bonificacao { get; set; }
        [JsonProperty]
        public string cdValorDesconto1Bonificacao { get; set; }
        [JsonProperty]
        public string descCdDesconto1Bonificacao { get; set; }
        [JsonProperty]
        public string dtDesconto2 { get; set; }

        [JsonProperty]
        public string vlDesconto2 { get; set; }
        [JsonProperty]
        public string qtdeCasasDecimaisDesconto2 { get; set; }
        [JsonProperty]
        public string cdValorDesconto2 { get; set; }
        [JsonProperty]
        public string descCdDesconto2 { get; set; }
        [JsonProperty]
        public string dtDesconto3 { get; set; }
        [JsonProperty]
        public string vlDesconto3 { get; set; }
        [JsonProperty]
        public string qtdeCasasDecimaisDesconto3 { get; set; }
        [JsonProperty]
        public string cdValorDesconto3 { get; set; }
        [JsonProperty]
        public string descCdDesconto3 { get; set; }
        [JsonProperty]
        public string diasDispensaMulta { get; set; }
        [JsonProperty]
        public string diasDispensaJuros { get; set; }
        [JsonProperty]
        public string cdBarras { get; set; }
        [JsonProperty]
        public string linhaDigitavel { get; set; }
        [JsonProperty]
        public string cdAcessorioEscrituralEmpresa { get; set; }
        [JsonProperty]
        public string tpVencimento { get; set; }
        [JsonProperty]
        public string indInstrucaoProtesto { get; set; }
        [JsonProperty]
        public string tipoAbatimentoTitulo { get; set; }
        [JsonProperty]
        public string cdValorJuros { get; set; }
        [JsonProperty]
        public string tpDesconto1 { get; set; }
        [JsonProperty]
        public string tpDesconto2 { get; set; }
        [JsonProperty]
        public string tpDesconto3 { get; set; }
        [JsonProperty]
        public string nuControleParticipante { get; set; }
        [JsonProperty]
        public string diasJuros { get; set; }
        [JsonProperty]
        public string cdJuros { get; set; }
        [JsonProperty]
        public string vlJuros { get; set; }
        [JsonProperty]
        public string cpfcnpjBeneficiário { get; set; }
        [JsonProperty]
        public string vlTituloEmitidoBoleto { get; set; }
        [JsonProperty]
        public string dtVencimentoBoleto { get; set; }
        [JsonProperty]
        public string indTituloPertenceBaseTitulos { get; set; }
        [JsonProperty]
        public string dtLimitePagamentoBoleto { get; set; }
        [JsonProperty]
        public string cdIdentificacaoTituloDDACIP { get; set; }
        [JsonProperty]
        public string indPagamentoParcial { get; set; }
        [JsonProperty]
        public string qtdePagamentoParciais { get; set; }
    }
  }


