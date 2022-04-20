using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(ASSINATURA))]
    public class AssinaturaLegadoDTO
    {
        public AssinaturaLegadoDTO()
        {
            this.EMAILS = new HashSet<EmailsDTO>();
            this.TELEFONES2 = new HashSet<Telefones2DTO>();
        }

        public string CODIGO_UNIX { get; set; }
        public string CODIGO { get; set; }
        public string ANO_COAD { get; set; }
        public string ATV_REM { get; set; }
        public string CORTESIA { get; set; }
        public string A_C { get; set; }
        public string E_MAIL { get; set; }
        public string EX_BTC { get; set; }
        public string PART_BTC { get; set; }
        public string MAT_ADIC { get; set; }
        public string REMESSA { get; set; }
        public string ANO_REMESSA { get; set; }
        public string DT_SUSP_REM_DIASC { get; set; }
        public string MALA_OFERTA { get; set; }
        public string ULT_PASTA { get; set; }
        public string REENVIO_PASTA_SN { get; set; }
        public string NUM_TP_ENVIO { get; set; }
        public string DT_GRAV_PASTA { get; set; }
        public string DATA_ASSINATURA { get; set; }
        public string MAT_ADIC2 { get; set; }
        public string DT_DEVOL_PASTA { get; set; }
        public string ENTREGADOR { get; set; }
        public string RETIRAR_DE_MAOS { get; set; }
        public string COD_CURSO { get; set; }
        public string COD_LIVRO { get; set; }
        public Nullable<int> MES_REFERENCIA { get; set; }
        public string PROSPECTADO { get; set; }
        public int AUTOID { get; set; }
        public string CNPJ_CPF { get; set; }
        public string DH_SUBIR { get; set; }
        public string DH_SUBIU { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<System.DateTime> DATA_INSERT { get; set; }

        public virtual ICollection<EmailsDTO> EMAILS { get; set; }
        public virtual ICollection<Telefones2DTO> TELEFONES2 { get; set; }
    }
}
