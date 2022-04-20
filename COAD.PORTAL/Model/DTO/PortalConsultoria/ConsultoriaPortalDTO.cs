using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTO.PortalConsultoria
{
    [Mapping(Source = typeof(consultoria))]
    public class ConsultoriaPortalDTO
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public int cod_consultor { get; set; }
        public int cod_supervisor { get; set; }
        public int cod_cq { get; set; }
        public string email { get; set; }
        public string estado { get; set; }
        public string colec { get; set; }
        public string gg { get; set; }
        public string verbete { get; set; }
        public string pergunta { get; set; }
        public string resposta_consultor { get; set; }
        public string resposta_supervisor { get; set; }
        public string resposta_cq { get; set; }
        public string status { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataRespConsultor { get; set; }
        public DateTime dataRespSupervisor { get; set; }
        public Nullable<DateTime> dataUltimoAcesso { get; set; }
        public string codFuncUltimoAcesso { get; set; }
        public Nullable<int> codFuncConsultEncam { get; set; }
    }
}
