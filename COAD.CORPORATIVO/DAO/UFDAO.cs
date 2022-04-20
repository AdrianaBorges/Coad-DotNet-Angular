 using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class UFDAO : DAOAdapter<UF, UFDTO>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public UFDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public IList<UFDTO> BuscarSomenteUF()
        {
            List<UF> query = this.db.UF.Where(x => x.UF_VALIDA == true).ToList();

            return ToDTO(query);
        }
  
        public IList<UFDTO> BuscarNaoCadastrada(int _prod_id, string _ura_id)
        {
            List<string> estados = new List<string>(); 

            List<URA_CONFIG> _uraconfig = this.db.URA_CONFIG.Where(x => x.PRO_ID == _prod_id && x.URA_ID == _ura_id).ToList();

            foreach(var _item in _uraconfig)
            {
                estados.Add(_item.UF_SIGLA_ACESSO);
            }

            var query = this.db.UF.Where(x => !estados.Contains(x.UF_SIGLA) && x.UF_VALIDA == true);

            return ToDTO(query);
        }

        /// <summary>
        /// Lista todas as ufs pertencentes ao carteiramento
        /// </summary>
        /// <returns></returns>
        public IList<UFDTO> GetUfsNoCarteiramento()
        {
            var queryCarteiramento = GetDb<COADCORPEntities>(false).CARTEIRA;
            IQueryable<UF> query =  queryCarteiramento.Where(obj => obj.AREA_ID == 1).Select(obj => obj.UF).Distinct();

            return ToDTO(query);
        }

        /// <summary>
        /// Lista todas as ufs pertencentes ao representante
        /// </summary>
        /// <returns></returns>
        public IList<UFDTO> GetUfsNoRepresentante()
        {
        
            IQueryable<UF> query = (from rep in db.REPRESENTANTE
                                    join cr in db.CARTEIRA_REPRESENTANTE on rep.REP_ID equals cr.REP_ID
                                    join ca in db.CARTEIRA on cr.CAR_ID equals ca.CAR_ID
                                    join uf in db.UF on ca.REGIAO_UF equals uf.UF_SIGLA
                                    where ca.AREA_ID == 1
                                    select uf).Distinct();


            return ToDTO(query);
        }

        public IList<UFDTO> ListUfsPorRegiao(int? rgId)
        {
            var query = GetDbSet().Where(x => x.RG_ID == rgId);
            return ToDTO(query);
        }

        public string BuscarCodUFPorDescricao(string descricaoEstado)
        {
            var query = (from uf in db.UF
                        where
                            uf.UF_DESCRICAO.ToLower() == descricaoEstado.ToLower() &&
                            uf.UF_VALIDA == true
                        select uf.UF_SIGLA)
                        .FirstOrDefault();
            return query;
        }

    }
}
