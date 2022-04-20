using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    public partial class ender_fatDTO
    {
        public string CODIGO { get; set; }
        public string TIPO_FAT { get; set; }
        public string END_FAT { get; set; }
        public string NUM_FAT { get; set; }
        public string TP_COMPL_FAT { get; set; }
        public string COMPL_FAT { get; set; }
        public string BAIRRO_FAT { get; set; }
        public string MUNIC_FAT { get; set; }
        public string UF_FAT { get; set; }
        public string CEP_FAT { get; set; }
        public Nullable<int> id_ceps { get; set; }
        public string DH_SUBIR { get; set; }
        public string DH_SUBIU { get; set; }
    }
}
