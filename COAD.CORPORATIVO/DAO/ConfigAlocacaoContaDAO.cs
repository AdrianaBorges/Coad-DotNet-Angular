using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ConfigAlocacaoContaDAO : DAOAdapter<CONFIG_ALOCACAO_CONTA, ConfigAlocacaoContaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ConfigAlocacaoContaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public int? RetornarCtaIdDeAcordoComConfiguracao(int? RG_ID, int? EMP_ID, int? TCC_ID)
        {
            var query = (from confConta in db.CONFIG_ALOCACAO_CONTA 
                         where 
                             confConta.TCC_ID == TCC_ID &&
                             confConta.CONFIG_ALOCACAO.EMP_ID == EMP_ID &&
                             confConta.CONFIG_ALOCACAO.RG_ID == RG_ID
                         select confConta.CTA_ID);
            return query.FirstOrDefault();
        }

    }
}
