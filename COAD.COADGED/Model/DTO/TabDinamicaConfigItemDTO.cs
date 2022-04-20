using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class TabDinamicaConfigItemDTO
    {
        public string TDC_ID { get; set; }
        public int TCI_ID { get; set; }
        public string TCI_TIPO_CAMPO { get; set; }
        public Nullable<int> TCI_TAMANHO_CAMPO { get; set; }
        public string TCI_ALINHAMENTO_CAMPO { get; set; }
        public int TCI_ORDEM_APRESENTACAO { get; set; }
        public string TCI_VALOR_PADRAO { get; set; }
        public string TCI_VALOR_ESPERADO { get; set; }
        public string TCI_NOME_CAMPODB { get; set; }
        public string TCI_NOME_CAMPO { get; set; }
        public string TCI_FORMULA { get; set; }
        public string USU_LOGIN { get; set; }
        public string TCI_TEXTO_HELP { get; set; }
        public string TDC_ID_TAB_REF { get; set; }
        public Nullable<bool> TCI_TEXTO_HELP_LINK { get; set; }
        public string TCI_PATH_HELP_LINK { get; set; }
        public string TCI_TARGET { get; set; }
        public Nullable<bool> TCI_CAMPO_PESQUISA { get; set; }
        public Nullable<bool> TCI_CAMPO_VISIVEL { get; set; }
        public string TCI_CAMPO_TIPO { get; set; }
        public Nullable<bool> TCI_CAMPO_CHAVE { get; set; }
        public Nullable<bool> TCI_CAMPO_LISTA { get; set; }
        public string TCI_NOME_CHAVEDB { get; set; }
        public string TCI_NOME_LISTADB { get; set; }
        public int TCI_DECIMAIS { get; set; }
        public string TCI_NOME_CAMPO_AUX { get; set; }
        public virtual TabDinamicaConfigDTO TAB_DINAMICA_CONFIG { get; set; }
    }
}
