﻿using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(FILA_EMAIL))]
    public class FilaEmailDTO
    {
        public int? FLE_ID { get; set; }
        public string FLE_EMAIL { get; set; }
        public string FLE_ASSUNTO { get; set; }
        public string FLE_CORPO { get; set; }
        public Nullable<System.DateTime> FLE_DATA_CRIACAO { get; set; }
        public Nullable<System.DateTime> FLE_DATA_ENVIO { get; set; }
        public string FLE_FUNCIONALIDADE_ORIGEM { get; set; }
        public string FLM_METODO_ORIGEM { get; set; }
        public string FLM_SERVICO_ORIGEM { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public string FLE_VISUALIZACAO_ALTERNATIVA { get; set; }
        public string FLE_CONTENT_TYPE { get; set; }
        public string FLE_ACTION_NAME { get; set; }
        public string FLE_ARGUMENTO_ACAO { get; set; }
        public Nullable<int> NTS_ID { get; set; }
        public Nullable<bool> FLE_REPORTACAO_ERRO { get; set; }
        public string FLE_CC { get; set; }
        public string FLE_CCO { get; set; }
        public string FLE_CC_ERRO { get; set; }
        public string FLE_CCO_ERRO { get; set; }
        public Nullable<System.DateTime> FLE_DATA_CANCELAMENTO { get; set; }
        public Nullable<System.DateTime> FLE_ERRO_PAUSADO_ATE { get; set; }
        public Nullable<int> FLE_COD_SMTP { get; set; }
        public string FLE_PATH_ANEXO { get; set; }
        public string FLE_SUCCESS_CALLBACK { get; set; }
        public string FLE_FAIL_CALLBACK { get; set; }
        public Nullable<int> FLE_CALLBACK_CONTEXT_KEY { get; set; }
        public string FLE_CALLBACK_CONTEXT_KEY_STR { get; set; }
        public Nullable<bool> FLE_FALHA_NOTIFICADA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotificacaoSistemaDTO NOTIFICACAO_SISTEMA { get; set; }
    }
}
