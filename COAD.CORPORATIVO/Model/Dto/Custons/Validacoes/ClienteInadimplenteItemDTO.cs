using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Validacoes
{
    public class ClienteInadimplenteItemDTO
    {
        public ClienteInadimplenteItemDTO()
        {
            this.Parcelas = new HashSet<ParcelasDTO>();
        }

        public int? CodigoCliente { get; set; }
        public string NomeCliente { get; set; }
        public string cpfCnpj { get; set; }
        public string assinatura { get; set; }
        public ICollection<ParcelasDTO> Parcelas { get; set; }

        public bool ExisteInadimplencia
        {
            get
            {
                return (Parcelas != null && Parcelas.Count() > 0);
            }
            set
            {

            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Cliente: [Código = ");
            sb.Append(CodigoCliente);
            sb.Append(", Nome = '");
            sb.Append(NomeCliente);
            sb.Append("', ");
            sb.Append("CPF/CNPJ = '");
            sb.Append(cpfCnpj);
            sb.Append("']");

            if (ExisteInadimplencia)
            {
                if(Parcelas != null)
                {
                    sb.Append("<br /> Existem (");
                    sb.Append(Parcelas.Count());
                    sb.Append(")");
                    sb.Append("parcelas não Pagas <br />");
                }
                
            }
            return sb.ToString();
        }
    }
}
