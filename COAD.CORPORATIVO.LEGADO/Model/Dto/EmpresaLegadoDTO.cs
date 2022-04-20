using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(empresas))]
    public class EmpresaLegadoDTO
    {
        public int id { get; set; }
        public string razao { get; set; }
        public string fantasia { get; set; }
        public string cnpj { get; set; }
        public string ie { get; set; }
        public string im { get; set; }
        public string suframa { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string pais { get; set; }
        public string cep { get; set; }
        public string telefones { get; set; }
        public string email { get; set; }
        public string site { get; set; }
        public string area { get; set; }
        public Nullable<int> ultima_nfe { get; set; }
        public string cnr_agcedente { get; set; }
        public int SEQ_DIARIO { get; set; }
        public string perfil { get; set; }
        public Nullable<int> atividade { get; set; }
        public string inf_complementar_nfe { get; set; }
        public string inf_complementar_cod { get; set; }
        public Nullable<int> primeira_nfe { get; set; }
        
    }
}
