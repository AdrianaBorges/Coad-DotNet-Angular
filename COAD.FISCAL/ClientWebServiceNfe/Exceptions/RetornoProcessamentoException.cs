using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Exceptions
{
    public class RetornoProcessamentoException : Exception
    {
        public new string Message { get; set; }
        public int? CodRetorno { get; set; }
        public string MensagemRetorno { get; set; }

        public void FormatarMensagem(string mensagemBase = null)
        {
            string mensagemBase2 = (!string.IsNullOrWhiteSpace(mensagemBase)) ? mensagemBase : "O Serviço não retornou o código esperado.";
            string mensagem = string.Format("{0}. Código Retornado: {1}, Mensagem Retornada: {2}", mensagemBase2, CodRetorno, MensagemRetorno);
            this.Message = mensagem;
        }

        public RetornoProcessamentoException(int? codRetorno, string mensagemRetorno)
        {
            this.CodRetorno = codRetorno;
            this.MensagemRetorno = mensagemRetorno;
            FormatarMensagem();
        }

        public RetornoProcessamentoException(string message, int? codRetorno, string mensagemRetorno) : base(message)
        {
            this.CodRetorno = codRetorno;
            this.MensagemRetorno = mensagemRetorno;
            FormatarMensagem(message);
        }

        public RetornoProcessamentoException(string message, int? codRetorno, string mensagemRetorno, Exception e) : base(message, e)
        {
            this.CodRetorno = codRetorno;
            this.MensagemRetorno = mensagemRetorno;
            FormatarMensagem(mensagemRetorno);
        }

    }
}
