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

    public class SecoesSRV : GenericService<SECOES, SecoesDTO, int>
    {
        private SecoesDAO _dao = new SecoesDAO();

        public SecoesSRV()
        {
            Dao = _dao;
        }

        public Pagina<SecoesDTO> Secoes(int? secaoId, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Secoes(secaoId, descricao, ativoId, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarSecao(SecoesDTO secao)
        {
            try
            {
                if (secao.SEC_ID == null && _dao.Secoes(null, secao.SEC_DESCRICAO).lista.Count() > 0)
                {
                    throw new Exception("Já existe uma seção cadastrada com este nome!");
                }
                else
                {
                    secao.SEC_ATIVO = secao.SEC_ATIVO == null ? 1 : secao.SEC_ATIVO;

                    if (secao.SEC_ID != null)
                    {
                        secao.DATA_ALTERA = DateTime.Now;
                        Merge(secao, "SEC_ID");
                    }
                    else
                    {
                        secao.DATA_CADASTRO = DateTime.Now;
                        Save(secao);
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

        public void DeletarSecao(int secaoId)
        {
            var secao = this.FindById(secaoId);
            secao.DATA_EXCLUSAO = DateTime.Now;
            Merge(secao, "SEC_ID");
        }

    }
}
