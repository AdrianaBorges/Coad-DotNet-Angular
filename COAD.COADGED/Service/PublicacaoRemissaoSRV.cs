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

    public class PublicacaoRemissaoSRV : GenericService<PUBLICACAO_REMISSAO, PublicacaoRemissaoDTO, int>
    {
        private PublicacaoRemissaoDAO _dao = new PublicacaoRemissaoDAO();

        public PublicacaoRemissaoSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoRemissaoDTO> PublicacaoRemissao(int? publicacaoId, int? areaId, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.PublicacaoRemissao(publicacaoId, areaId);
            return resp;
        }

        public void SalvarPublicacaoRemissao(ICollection<PublicacaoRemissaoDTO> publicacaoRemissao)
        {
            try
            {
                if (publicacaoRemissao.Count > 0)
                {
                    // eliminando anteriores...
                    var eliminar = this.PublicacaoRemissao(publicacaoRemissao.First().PUB_ID, publicacaoRemissao.First().ARE_CONS_ID).lista;
                    DeleteAll(eliminar, "PRE_ID");

                    // salvando atuais...
                    foreach (var s in publicacaoRemissao)
                    {
                        if (s.PUB_ID != null && s.ARE_CONS_ID != null && s.PRE_REMISSAO != null && s.PRE_NUMERO != null)
                        {
                            Save(s);
                        }
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
