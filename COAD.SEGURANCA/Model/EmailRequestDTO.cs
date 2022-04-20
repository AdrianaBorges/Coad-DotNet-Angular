using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    public class EmailRequestDTO
    {
        [EmailAddress(ErrorMessage = "O E-Mail não é válido.")]
        [Required(ErrorMessage = "Informe o E-Mail de destino")]
        public string EmailDestino { get; set; }

        [Required(ErrorMessage = "Informe o assunto.")]
        public string Assunto { get; set; }

        [Required(ErrorMessage ="O corpo do E-Mail não pode ficar vazio.")]
        public string CorpoEmail { get; set; }

        
        public string Host { get; set; }

        
        public int Port { get; set; }
        public bool EnableSsl { get; set; }

       
        public string User { get; set; }

        
        public string Senha { get; set; }

        
        public string From {get; set;}

        public AlternateView AlternateView { get; set; }

        //public IList<byte[]> LstAnexos { get; set; }
        
        public bool EnviarAgendado {get; set;} 
        public string MetodoOrigem {get; set;} 
        public string ServicoOrigem {get; set;} 
        public string ActionName {get; set;}
        public string ActionArg { get; set; }
        public int? codNotificacaoSistema { get; set; }
        public bool? reporteDeErro { get; set; }
        public int? codRepresentante { get; set; }
        public string usuario { get; set; }
        public string CC { get; set; }
        public string CCO { get; set; }
        public string CC_ERRO { get; set; }
        public string CCO_ERRO { get; set; }
        public string pathAnexo { get; set; }
        public ICollection<EmailRequestAnexoDTO> Anexos { get; set; }
        public string SuccessCallback { get; set; }
        public string FailCallback { get; set; }
        public int? CallbackContextKey { get; set; }
        public string CallbackContextKeyStr { get; set; }

        public EmailRequestDTO()
        {
            EnviarAgendado = true;
            Host = "smtp.gmail.com";
            Port = 587;
            //User = "backup@coad.com.br";
            //Senha = "Br@z1l!2014";
            From = "coadcorp@coad.com.br";
            EnableSsl = true;
            //LstAnexos = new List<byte[]>();
            Anexos = new HashSet<EmailRequestAnexoDTO>();
            //EnviarAgendado = true;
            //Host = "smtp.gmail.com";
            //Port = 587;
            //User = "coadcorp@coad.com.br";
            //Senha = "C04dc0rp@";
            //From = "coadcorp@coad.com.br";
            //EnableSsl = true;
            //LstAnexos = new List<byte[]>();
        }

        public int? codSMTP { get; set; }
    }
}
