using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Validacoes
{
    public class ValidacaoClienteInadimplenteDTO
    {
        public ValidacaoClienteInadimplenteDTO()
        {
            this.ClientesSimilaresInadimplentes = new HashSet<ClienteInadimplenteItemDTO>();
        }

        //public bool Forcar { get; set; }

        public ClienteInadimplenteItemDTO ClienteInadimplente { get; set; }
        public ICollection<ClienteInadimplenteItemDTO> ClientesSimilaresInadimplentes { get; set; }

        public bool ExisteInadimplencia
        {
            get
            {
                bool inadimplencia = (ClienteInadimplente != null) ?
                        ClienteInadimplente.ExisteInadimplencia : false;
            
                if(ClientesSimilaresInadimplentes != null)
                {
                    foreach(var cliSimi in ClientesSimilaresInadimplentes)
                    {
                        inadimplencia = (inadimplencia || cliSimi.ExisteInadimplencia);
                    }
                }
                return inadimplencia;
            }

            set { }
        }

        public bool ExisteInadimplenciaPropria
        {
            get
            {
                return (ClienteInadimplente != null) ?
                        ClienteInadimplente.ExisteInadimplencia : false;

            }
            set { }
        }

        public bool ExisteInadimplenciaDeSimilar
        {
            get
            {
                bool inadimplencia = false;
                if (ClientesSimilaresInadimplentes != null)
                {
                    foreach (var cliSimi in ClientesSimilaresInadimplentes)
                    {
                        inadimplencia = (inadimplencia || cliSimi.ExisteInadimplencia);
                    }
                }
                return inadimplencia;

            }
            set { }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (ExisteInadimplenciaPropria)
            {
                sb.Append(string.Format("O Cliente {0} possui parcelas vencidas não pagas.", ClienteInadimplente.NomeCliente));
                sb.Append("<br />");
                sb.Append(ClienteInadimplente);

                if (ExisteInadimplenciaDeSimilar)
                {
                    string msg = string.Format("Além disso, existem existem {0} Clientes com algum campo (CPF/CNPJ, E-Mail, Telefone) igual ao deste cliente que está inadimplente.", ClientesSimilaresInadimplentes.Count());
                    sb.Append(msg);
                }
                sb.Append("<br />");
            }
            else if(ExisteInadimplenciaDeSimilar)
            {
                string msg = string.Format("O Cliente {0} não possui nenhum inadimplência. <br /> Porém, existem {1} Clientes com algum campo (CPF/CNPJ, E-Mail, Telefone) igual ao deste cliente que está inadimplente.", ClienteInadimplente.NomeCliente, ClientesSimilaresInadimplentes.Count());
                sb.Append(msg);
                sb.Append("<br />"); 
                sb.Append(ClienteInadimplente);
                sb.Append("<br />");
            }

            if (ExisteInadimplenciaDeSimilar)
            {
                sb.Append("Dados dos Clientes Similares");

                int? index = 0;
                foreach(var cliSimi in ClientesSimilaresInadimplentes)
                {
                    if (index <= 1)
                        sb.Append(cliSimi);
                    else
                        break;
                    index++;
                }

                if(ClientesSimilaresInadimplentes.Count > 2)
                {
                    sb.Append(string.Format("Mais {0} ....", ClientesSimilaresInadimplentes.Count - 2));
                }
            }

            return sb.ToString();
        }
    }
}
