using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ParcelaAlocadaUpdateDTO
    {
        public Nullable<int> ALO_NOSSO_NUMERO { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public string PAR_REMESSA { get; set; }
        public Nullable<int> CTA_ID { get; set; }
        public Nullable<System.DateTime> ALO_DATA_TRANSMISSAO { get; set; }
        public string OCM_CODIGO { get; set; }
        public Nullable<System.DateTime> ALO_REM_DATA_OCORRENCIA { get; set; }
        public string OCT_CODIGO { get; set; }
        public Nullable<System.DateTime> ALO_RET_DATA_OCORRENCIA { get; set; }
        public string OCE_CODIGO { get; set; }
        public string BAN_ID { get; set; }
        public Nullable<System.DateTime> ALO_DATA_ALOCACAO { get; set; }
        public Nullable<System.DateTime> ALO_DATA_DESALOCACAO { get; set; }
        public Nullable<int> REM_ID { get; set; }
    }
}

