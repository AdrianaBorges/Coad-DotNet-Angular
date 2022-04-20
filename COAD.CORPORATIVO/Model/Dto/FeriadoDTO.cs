using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class FeriadoMesDTO
    {
        public FeriadoMesDTO()
        {
            this.MES_FERIADOS = new HashSet<FeriadoDTO>();
        }
        public int MES_ID { get; set; }
        public string MES_DESCRICAO { get; set; }
        public virtual ICollection<FeriadoDTO> MES_FERIADOS { get; set; }

    }

    public partial class FeriadoDTO
    {
        public int FER_ID { get; set; }
        public string FER_DESCRICAO { get; set; }
        public Nullable<System.DateTime> FER_DATA { get; set; }
        public Nullable<bool> FER_FIXO { get; set; }
        public string FER_TIPO { get; set; }
    }
}
