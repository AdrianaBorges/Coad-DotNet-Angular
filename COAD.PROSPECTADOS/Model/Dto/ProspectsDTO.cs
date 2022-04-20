using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Model.Dto
{
    [Mapping(Source = typeof(prospects))]
    public class ProspectsDTO
    {
        public string CODIGO { get; set; }
        public string FUNC_IND { get; set; }
        public string CART { get; set; }
        public string DRIVE_CDROM { get; set; }
        public string DATA_EMI_FICHA { get; set; }
        public string AREA { get; set; }
        public string DATA_ATRIBUICAO { get; set; }
        public string MANTER { get; set; }
        public Nullable<int> NUM_ENVIOS_ADNRJ { get; set; }
        public string INTERNET { get; set; }
        public string PFIS_PJUR { get; set; }
        public string CPF_CNPJ { get; set; }
        public string REGIAO { get; set; }
        public string MALA_ADV_SN { get; set; }
        public string INSCRICAO { get; set; }

        [IgnoreMemberMapping(MappingDirection.Both)]
        public virtual CartCoadDTO cart_coad { get; set; }
    }
}
