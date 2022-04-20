using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(bloqueia_consulta_individual))]
    public class BloqueiaConsultaIndividualDTO
    {
        public string data { get; set; }
        public string hora { get; set; }
        public string usuario { get; set; }
        public string assinatura { get; set; }
        public Nullable<int> qtd_consulta_sem { get; set; }
        public string ativo_sn { get; set; }
        public Nullable<int> qtd_consulta_total { get; set; }
        public Nullable<int> qtd_consulta_usou { get; set; }
        public Nullable<int> qtd_disponibilizar { get; set; }
        public Nullable<int> qtd_consulta_acum { get; set; }
        public string per_disponibilizar { get; set; }
        public string atualizou { get; set; }
        public Nullable<System.DateTime> DATA_INSERT { get; set; }
        public string USU_LOGIN { get; set; }
        public int AUTOID { get; set; }
        
    }
}
