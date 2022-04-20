using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    public class ContabilistaModel
    {
        public ContabilistaModel()
        {
            this.EMPRESA = new HashSet<EmpresaModel>();
        }
    
        public int CNT_ID { get; set; }
        public string CNT_NOME { get; set; }
        public string CNT_CPF_CNPJ { get; set; }
        public string CNT_CRC_UF { get; set; }
        public string CTR_CRC_NUMERO { get; set; }
        public string CTR_CRC_DIGITO { get; set; }
        public string CTR_CEP { get; set; }
        public string CTR_LOGRADOURO { get; set; }
        public string CTR_NUMERO { get; set; }
        public string CTR_COMPLEMENTO { get; set; }
        public string CTR_BAIRRO { get; set; }
        public string CTR_TEL { get; set; }
        public string CTR_FAX { get; set; }
        public string CTR_CEL { get; set; }
        public string CTR_EMAIL { get; set; }
        public string IBGE_COD_COMPLETO { get; set; }

        public virtual ICollection<EmpresaModel> EMPRESA { get; set; }
    }
}
