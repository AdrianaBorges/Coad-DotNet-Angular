

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.RM.Repositorios.Base;
using COAD.RM.Model.Dto;
using COAD.RM.Repositorios.Contexto;

namespace COAD.RM.DAO
{
    public class PfperffDAO : AbstractGenericDao<PFPERFF, PfperffDTO, object>
    {
        public CorporeRMEntities db { get { return GetDb<CorporeRMEntities>(); } set { } }
        public PfperffDAO()
        {
            this.SetProfileName("rm");
            db = GetDb<CorporeRMEntities>(false);
        }
        public IList<PfperffDTO> ListarContraCheque(string _cpf)
        {
            var _query = (from p in db.PFPERFF
                          join f in db.PFUNC on new { p.CODCOLIGADA, p.CHAPA } equals new { f.CODCOLIGADA, f.CHAPA }
                          join e in db.PPESSOA on f.CODPESSOA equals e.CODIGO
                          where (e.CPF == _cpf)
                          select new PfperffDTO
                          {
                              CODCOLIGADA = p.CODCOLIGADA,
                              CHAPA = p.CHAPA,
                              MESCOMP = p.MESCOMP,
                              ANOCOMP = p.ANOCOMP,
                              NROPERIODO = p.NROPERIODO,
                              DESCRICAO = p.DESCRICAO,
                              NOME = f.NOME, 
                              CCH_NUM_CPF = e.CPF

                          }).OrderByDescending(x => x.ANOCOMP).ThenByDescending(x => x.MESCOMP).ToList();

            return _query;
        }
        public ContraChequeCustomDTO BuscarContraCheque(string cpf, string empresa, string ano, string mes, string periodo)
        {

            var _contracheque = new ContraChequeCustomDTO();
            var _proventos = db.COAD_EMISSAO_CONTRACHEQUES(cpf, empresa, ano, mes, periodo).ToList();
            _contracheque.CCH_TOT_VLR_PROVENTO = _proventos.Sum(x => x.CCH_VLR_PROVENTO);
            _contracheque.CCH_TOT_VLR_DESCONTOS = _proventos.Sum(x => x.CCH_VLR_DESCONTOS);
            _contracheque.Lista = _proventos;

            return _contracheque;
        }

    }
}
