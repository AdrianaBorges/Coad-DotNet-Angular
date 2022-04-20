using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class PesquisaClienteDTO
    {
        public PesquisaClienteDTO()
        {
            this.pagina = 1;
            this.registroPorPagina = 50;
            this.pesquisaCpfCnpjPorIqualdade = true;
            this.buscarForaDaAgenda = false;
            this.excluidosDaValidacao = null;
        }

        public bool uenLogada { get; set; }
        public int? codigoCliente { get; set; }
        public string codigoAssinatura { get; set; }
        public string cpf_cnpj { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string dddTelefone { get; set; }
        public string telefone { get; set; }
        public bool pesquisaCpfCnpjPorIqualdade { get; set; }
        public int pagina { get; set; }
        public int registroPorPagina { get; set; }
        public IList<int?> lstClaCliId { get; set; }
        public int? repId { get; set; }        
        public int? uen_id { get; set; }
        public int? classificacaoClienteId { get; set; }
        public string CAR_ID { get; set; }
        public int? RG_ID { get; set; }    
        public int? REP_ID { get; set; }
        public int? AREA_ID { get; set; }
        public int? CMP_ID { get; set; }
        public bool buscarForaDaAgenda { get; set; }
        public bool BuscarTodos { get; set; }
        public int? origemId { get; set; }
        public bool? excluidosDaValidacao { get; set; }
        public bool buscarCarteiraAssinatara { get; set; }
    }
}
