using COAD.FISCAL.Model.NFSe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Exceptions
{
    public static class ExceptionFormatterNfse
    { 
        public static string FormatarMensagem(string mensagemBase, ListaMensagemRetornoNfse ListaMensagem)
        {
            string mensagemBase2 = (!string.IsNullOrWhiteSpace(mensagemBase)) ? mensagemBase : "O Retornou um erro.";

            if (ListaMensagem != null && ListaMensagem.MensagemRetorno != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var msg in ListaMensagem.MensagemRetorno)
                {
                    sb.Append($"Código de Retorno: {msg.Codigo} Mensagem Retorno: {msg.Mensagem} Correção: {msg.Correcao} <br />");
                }
                mensagemBase2 += sb.ToString();
            }

            return mensagemBase2;
        }

    }
}
