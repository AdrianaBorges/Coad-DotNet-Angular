using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Notificacoes
{
    public class NotificacaoRegistroLiberacaoDTO
    {
        public bool? Aceito { get; set; }
        public int? codProposta { get; set; }
        public int? codRegistroLiberacao { get; set; }
        public int? codItemProposta { get; set; }
        public string Email { get; set; }
        public string Assinatura { get; set; }
        public string NomeCliente { get; set; }
        public string Mensagem { get; set; }
        public string MensagemHTML { get; set; }
        public int? RepId { get; set; }
        public int? RepNomeSupervisor { get; set; }
        public int? CliId { get; set; }
    }
}
