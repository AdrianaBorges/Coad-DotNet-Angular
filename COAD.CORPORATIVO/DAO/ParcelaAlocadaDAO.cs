using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Configuration;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.CORPORATIVO.DAO
{
    public class ParcelaAlocadaDAO : AbstractGenericDao<PARCELA_ALOCADA, ParcelaAlocadaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }
        public ParcelaAlocadaDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public int ObterNumeroEnvioBoleto(string titulo, List<int> ctasIds)
        {
            int numeroDoEnvio = (from a in db.PARCELA_ALOCADA
                                 where a.PAR_NUM_PARCELA == titulo && ctasIds.Contains(a.CTA_ID)
                                 select a).Count() + 1;

            return numeroDoEnvio;
        }

        public IList<int> ObterContas(string titulo)
        {
            var query = (from ct in db.PARCELA_ALOCADA
                         where ct.PAR_NUM_PARCELA==titulo
                         select ct.CTA_ID);

            return query.ToList();
        }


        // ler remessa enviada
        public IList<ParcelaAlocadaDTO> LerRemessaEnviada(string remessa = null)
        {
            if (!String.IsNullOrWhiteSpace(remessa))
            {
                var query = (from p in db.PARCELA_ALOCADA
                             where (p.PAR_REMESSA == remessa)
                             group p by new
                             {
                                 p.OCM_CODIGO,
                                 p.ALO_REM_DATA_OCORRENCIA,
                                 p.OCT_CODIGO,
                                 p.ALO_RET_DATA_OCORRENCIA,
                                 p.OCE_CODIGO,
                                 p.ALO_DATA_ALOCACAO,
                                 p.ALO_DATA_TRANSMISSAO,
                                 p.ALO_DATA_DESALOCACAO,
                                 p.CTA_ID
                             } into f
                             orderby f.Key.OCM_CODIGO
                             select new ParcelaAlocadaDTO
                             {
                                 OCM_CODIGO = f.Key.OCM_CODIGO,
                                 ALO_REM_DATA_OCORRENCIA = f.Key.ALO_REM_DATA_OCORRENCIA,
                                 OCT_CODIGO = f.Key.OCT_CODIGO,
                                 ALO_RET_DATA_OCORRENCIA = f.Key.ALO_RET_DATA_OCORRENCIA,
                                 OCE_CODIGO = f.Key.OCE_CODIGO,
                                 ALO_DATA_ALOCACAO = f.Key.ALO_DATA_ALOCACAO,
                                 ALO_DATA_TRANSMISSAO = f.Key.ALO_DATA_TRANSMISSAO,
                                 ALO_DATA_DESALOCACAO = f.Key.ALO_DATA_DESALOCACAO,
                                 CTA_ID = f.Key.CTA_ID
                             });
                return query.ToList();
            }
            else
                return null;
        }

        public IList<ParcelaAlocadaDTO> LerParcelaAlocada(int _rem_id)
        {
            IQueryable<PARCELA_ALOCADA> query = GetDbSet();
            query = query.Where(x => x.REM_ID == _rem_id);
            return ToDTO(query);

        }

        public IList<ParcelaAlocadaDTO> LerParcelaAlocada(string titulo = null, int? remessa = null, int? ctaId = null, DateTime? dtTransm = null, 
            string codRem=null, DateTime? dtCodRem = null, string codRet=null, DateTime? dtCodRet = null, string codErr=null)
        {
            IQueryable<PARCELA_ALOCADA> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(titulo))
            {
                query = query.Where(x => x.PAR_NUM_PARCELA == titulo);
            }
            if (remessa != null)
            {
                query = query.Where(x => x.REM_ID == remessa);
            }
            if (ctaId != null)
            {
                query = query.Where(x => x.CTA_ID == ctaId);
            }
            if (dtTransm != null)
            {
                query = query.Where(x => x.ALO_DATA_TRANSMISSAO == dtTransm);
            }
            if (!String.IsNullOrWhiteSpace(codRem))
            {
                query = query.Where(x => x.OCM_CODIGO == codRem);
            }
            if (dtCodRem != null)
            {
                query = query.Where(x => x.ALO_REM_DATA_OCORRENCIA == dtCodRem);
            }
            if (!String.IsNullOrWhiteSpace(codRet))
            {
                query = query.Where(x => x.OCT_CODIGO == codRet);
            }
            if (dtCodRet != null)
            {
                query = query.Where(x => x.ALO_RET_DATA_OCORRENCIA == dtCodRet);
            }
            if (!String.IsNullOrWhiteSpace(codErr))
            {
                query = query.Where(x => x.OCE_CODIGO == codErr);
            }
            if (codErr == "") // retornar apenas sem informação de erro...
            {
                query = query.Where(x => x.OCE_CODIGO == null);
            }
            
            return ToDTO(query);
        }
    }
}
