using System;
using COAD.COBRANCA.Bancos.Service;
using COAD.SEGURANCA.Model;

namespace COAD.COBRANCA.Bancos.Model.DTO.Interfaces
{
    public interface IBanco
    {
        string CodigoBanco { get; set; }
        string NomeBanco { get; set; }
        string Logo { get; }
        string BancoDesc { get; }
        bool UsarConfigDoBanco { get; set; }
        CodigoBarrasDTO GerarCodigoBarras(DateTime? DataVencimento, string NossoNumero, decimal? Valor, ContaDTO Conta);
        string CalcularDigitoNossoNumero(string nossoNumero, string CodigoCarteira);
        string CalcularDVCodigoBarras(string codigoBarras);
        string CalcularDVLinhaDigitavel(string campoLinhaDigitavel);

        /// <summary>
        /// Recebe o Código de Barras e Retora a decomposição dos 3 campos da linha digitável que são variáveis de acordo com o banco
        /// </summary>
        /// <param name="cb">O código de barras há ser processado</param>
        /// <returns>Objeto conténdo os três primeiros campos. Se o retorno for null executa o processamento genérico.</returns>
        CamposLinhaDigitalDTO SepararCamposVariaveisLinhaDigitavel(CodigoBarrasDTO cb);

        string FormatarContaAgencia(ContaDTO conta);
        bool ExcluirDigitoNossoNumero { get; }
        
    }
}
