using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(datas_fat))]
    public class DatasFaturamentoDTO
    {   
        public string PERIODO { get; set; }
        public string SEMANA { get; set; }
        public string DATA_FAT { get; set; }
        public string ANO { get; set; }
        public string MES { get; set; }
        public string DIA { get; set; }
        private DateTime? data { get; set; }

        public string Descricao {
            get
            {
                string dataStr = null;

                if(Data != null)
                {
                    dataStr = Data.Value.ToString("dd/MM/yyyy");
                }

                string descricao = "{0} Período {1} Semana {2}";
                descricao = string.Format(descricao, dataStr, PERIODO, SEMANA);
                return descricao;
            }
        }

        public DateTime? Data { get
        {
            if (data == null)
            {
                int anoInt = 0;
                int mesInt = 0;
                int diaInt = 0;

                if (!string.IsNullOrWhiteSpace(ANO))
                {
                    int.TryParse(ANO, out anoInt);
                }

                if (!string.IsNullOrWhiteSpace(MES))
                {
                    int.TryParse(MES, out mesInt);
                }

                if (!string.IsNullOrWhiteSpace(DIA))
                {
                    int.TryParse(DIA, out diaInt);
                }

                data = new DateTime(anoInt, mesInt, diaInt);
            }

            return data;
        
        } set
            {
                data = value;
            } 
        }
    }
}
