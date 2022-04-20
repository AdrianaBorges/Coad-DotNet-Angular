using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class OrgaoSRV : GenericService<ORGAO, OrgaoDTO, int>
    {
        private OrgaoDAO _dao = new OrgaoDAO();

        public OrgaoSRV()
        {
            Dao = _dao;
        }
        public IList<OrgaoDTO> Listar(int? _situacao)
        {
            return _dao.Listar(_situacao);
        }

        public Pagina<OrgaoDTO> Orgaos(int? orgaoId, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Orgaos(orgaoId, descricao, ativoId, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarOrgao(OrgaoDTO orgao)
        {
            try
            {
                if (orgao.ORG_ID == null && _dao.Orgaos(null, orgao.ORG_DESCRICAO).lista.Count() > 0)
                {
                    throw new Exception("Já existe um órgão cadastrado com este nome!");
                }
                else
                {
                    orgao.ORG_ATIVO = orgao.ORG_ATIVO == null ? 1 : orgao.ORG_ATIVO;

                    if (orgao.ORG_ID != null)
                    {
                        orgao.DATA_ALTERA = DateTime.Now;
                        Merge(orgao, "ORG_ID");
                    }
                    else
                    {
                        orgao.DATA_CADASTRO = DateTime.Now;
                        Save(orgao);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var _erro = new FormattedDbEntityValidationException(dbEx);

                SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                throw _erro;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
    }
}
