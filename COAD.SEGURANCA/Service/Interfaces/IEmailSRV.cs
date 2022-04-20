using System;
namespace COAD.SEGURANCA.Service.Interfaces
{
    public interface IEmailSRV
    {
        System.Collections.Generic.List<OpenPop.Mime.Message> BaixarEmails(string hostname, string username = null, string password = null, int porta = 995, bool useSsl = true);
        System.Text.Encoding CustomFallbackDecoder(string characterSet);
        void EnviarEmail(COAD.SEGURANCA.Model.EmailRequestDTO emailDTO);
        
        //void EnviarEmail(
        //    string emaildest, 
        //    string emailassunto, 
        //    string emailmenssagem, 
        //    bool enviarAgendado = true, 
        //    string metodoOrigem = null, 
        //    string servicoOrigem = null,
        //    string actionName = null,
        //    string actionArg = null);

        void EnviarEmailParaCliente(string emailDest,
            string assunto, 
            string bodyMessage, 
            string urlImagemCabecalho = null, 
           string actionName = null,
            string actionArg = null,
            string emailCCErro = null,
            int? codSMTP = null);

        void InsertCustomEncodings(System.Text.Encoding customEncoding);
        void LembrarSenha(string _login, string _email, string _sessionID, string _url);
        string GerarTemplateCliente(string bodyMessage, string urlImagemCabecalho = null);


    }
}
