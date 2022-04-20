using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(IMPORTACAO))]
    public class ImportacaoDTO
    {
        public ImportacaoDTO()
        {
            this.IMPORTACAO_SUSPECT = new HashSet<ImportacaoSuspectDTO>();
            this.IMPORTACAO_HISTORICO = new HashSet<ImportacaoHistoricoDTO>();
            this.IMPORTACAO_RESULTADO_RODIZIO = new HashSet<ImportacaoResultadoRodizioDTO>();
        }

        public int? IMP_ID { get; set; }
        public Nullable<System.DateTime> IMP_DATA { get; set; }
        public Nullable<System.DateTime> IMP_DATA_CANCELAMENTO { get; set; }
        public Nullable<System.DateTime> IMP_DATA_CONCLUSAO { get; set; }
        public Nullable<System.DateTime> IMP_DATA_INICIO { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public string IMP_PASSO_BATCH_DESCRICAO { get; set; }
        public Nullable<int> IMP_PASSO_BATCH { get; set; }
        public Nullable<int> IMS_ID { get; set; }
        public string IMS_PATH_ARQUIVO { get; set; }
        public Nullable<int> IMP_QTD_PROC_SUCCESSO { get; set; }
        public Nullable<int> IMP_QTD_PROC_FALHA { get; set; }
        public Nullable<int> IMP_QTD_SUS_TOTAL { get; set; }
        public Nullable<int> IMP_QTD_SUS_DUPLICADA { get; set; }
        public Nullable<int> IMP_QTD_REAL_SUS { get; set; }
        public Nullable<System.DateTime> IMP_DATA_ULTIMA_EXECUCAO { get; set; }
        public Nullable<bool> IMP_REEXECUTAR { get; set; }
        public Nullable<bool> IMP_EXECUTANDO { get; set; }
        public Nullable<bool> IMP_PLANILHA_INSERIDA { get; set; }
        public Nullable<bool> IMP_WEB_SERVICE { get; set; }

        public string NomeArquivo {
            get {

                if (!string.IsNullOrWhiteSpace(IMS_PATH_ARQUIVO))
                    return IMS_PATH_ARQUIVO.Split('\\').Last();
                    return null;
            }
            set { }
        }
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ImportacaoStatusDTO IMPORTACAO_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoSuspectDTO> IMPORTACAO_SUSPECT { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoHistoricoDTO> IMPORTACAO_HISTORICO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoResultadoRodizioDTO> IMPORTACAO_RESULTADO_RODIZIO { get; set; }

    }
}
