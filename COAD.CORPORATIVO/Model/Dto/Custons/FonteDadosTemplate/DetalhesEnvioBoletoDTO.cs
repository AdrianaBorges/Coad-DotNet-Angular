using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate
{
    public class DetalhesEnvioBoletoDTO
    {
        public int? CliId { get; set; }
        public string NomeCliente { get; set; }
        public string NomeProduto { get; set; }
        public string Assinatura { get; set; }
        public int? CodigoItem { get; set; }
        public decimal? ValorBoletoDecimal { get; set; }
        public DateTime? DataVenc { get; set; }
        public string CodigoBarras { get; set; }


        public string ValorBoleto
        {
            get
            {
                if (ValorBoletoDecimal != null)
                {
                    var valor = StringUtil.FormatarDinheiro(ValorBoletoDecimal, false);
                    return valor;
                }
                return null;
            }
            private set { }
        }


        public string DataVencimento
        {
            get
            {

                if (DataVenc != null)
                {
                    return DataVenc.Value.ToString("dd/MM/yyyy");
                }
                return null;
            }
            private set { }
        }

    }
}
