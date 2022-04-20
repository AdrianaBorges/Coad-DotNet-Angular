﻿using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(HISTORICO_EXECUCAO))]
    public class HistoricoExecucaoDTO
    {       /// <summary>
            /// 
            /// </summary>
        public int? HIE_ID { get; set; }
        public string HIE_NOME { get; set; }
        public string HIE_DESCRICAO { get; set; }
        public Nullable<System.DateTime> HIE_DATA_EXECUCAO { get; set; }
        public string HIE_ERRO_NOME { get; set; }
        public string HIE_ERRO_DESCRICAO { get; set; }
        public string HIE_NOME_SERVICO { get; set; }
        public string HIE_PROJETO { get; set; }
        public string HIE_STACK_TRACE { get; set; }
        public Nullable<bool> HIE_CORREU_ERRO { get; set; }
        public Nullable<int> TPJ_ID { get; set; }
        public Nullable<int> HIE_ID_REF { get; set; }
        public string HIE_ID_DESC { get; set; }
        public string HIE_ID_STR_REF { get; set; }

        public Nullable<int> NTS_ID { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoJobDTO TIPO_JOB { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotificacaoSistemaDTO NOTIFICACAO_SISTEMA { get; set; }
    }

}
