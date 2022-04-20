using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class DadosClienteStDTO
    {
        public string login { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string cep { get; set; }
        public DateTime? dataExpiracao { get; set; }
        public DateTime? createdTime { get; set; }
        public int? quantidadeSessoes { get; set; }
        public string tipoUsuario { get; set; }
        public string planoNome { get; set; }
        public int? quantidadeCalculoIcmsSt { get; set; }
        public int? quantidadeConsultaAliquotaMva { get; set; }
        public int? quantidadeMonitoramentoNcm { get; set; }
        public int? acessoSuporteChat { get; set; }
        public int? perfil { get; set; }
        public Boolean acessoTraxo { get; set; }
        public int? quantidadeConsultaEmail { get; set; }
    }
}
