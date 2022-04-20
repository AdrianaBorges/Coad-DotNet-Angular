using COAD.FISCAL.Model.NFSe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Exceptions
{
    public class RetornoProcessamentoNfseException : Exception
    {
        public new string Message { get; set; }
        public ListaMensagemRetornoNfse ListaMensagem { get; set; }

        public void FormatarMensagem(string mensagemBase = null)
        {
            string mensagemBase2 = (!string.IsNullOrWhiteSpace(mensagemBase)) ? mensagemBase : "O Retornou um erro.";

            if (ListaMensagem != null && ListaMensagem.MensagemRetorno != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach(var msg in ListaMensagem.MensagemRetorno)
                {
                    sb.Append($"Código de Retorno: {msg.Codigo} Mensagem Retorno: {msg.Mensagem} Correção: {msg.Correcao} <br />");
                }
                mensagemBase2 += sb.ToString();
            }

            this.Message = mensagemBase2;
        }

        public RetornoProcessamentoNfseException()
        {
            FormatarMensagem();
        }

        public RetornoProcessamentoNfseException(string message) : base(message)
        {
            FormatarMensagem(message);
        }

        public RetornoProcessamentoNfseException(string message, Exception e) : base(message, e)
        {
            this.ListaMensagem = ListaMensagem;
        }

    }
}
