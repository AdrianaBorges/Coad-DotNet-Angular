using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class OpcaoAtendimentoDTO
    {
        public OpcaoAtendimentoDTO()
        {
            this.ASSINATURA_EMAIL = new HashSet<AssinaturaEmailDTO>();
            this.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();
            //this.PRE_PEDIDO_EMAIL = new HashSet<PRE_PEDIDO_EMAIL>();
            //this.PRE_PEDIDO_TELEFONE = new HashSet<PRE_PEDIDO_TELEFONE>();
        }
    
        public int OPC_ID { get; set; }
        public string OPC_DESCRICAO { get; set; }
        public Nullable<int> OPC_ATIVO_TELEFONE { get; set; }

        public virtual ICollection<AssinaturaEmailDTO> ASSINATURA_EMAIL { get; set; }
        public virtual ICollection<AssinaturaTelefoneDTO> ASSINATURA_TELEFONE { get; set; }
        //public virtual ICollection<PRE_PEDIDO_EMAIL> PRE_PEDIDO_EMAIL { get; set; }
        //public virtual ICollection<PRE_PEDIDO_TELEFONE> PRE_PEDIDO_TELEFONE { get; set; }
    }
}
