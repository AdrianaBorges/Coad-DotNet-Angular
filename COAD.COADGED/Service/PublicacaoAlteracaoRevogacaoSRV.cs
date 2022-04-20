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

    public class PublicacaoAlteracaoRevogacaoSRV : GenericService<PUBLICACAO_ALTERACAO_REVOGACAO, PublicacaoAlteracaoRevogacaoDTO, int>
    {
        private PublicacaoAlteracaoRevogacaoDAO _dao = new PublicacaoAlteracaoRevogacaoDAO();

        public PublicacaoAlteracaoRevogacaoSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoAlteracaoRevogacaoDTO> PublicacaoAlteracaoRevogacao(int? id, int? publicacaoId, string tipo = null, int? tipoAtoId=null, string nrAto = null, DateTime? dtAto = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.PublicacaoAlteracaoRevogacao(id, publicacaoId, tipo, tipoAtoId, nrAto, dtAto, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarPublicacaoAlteracaoRevogacao(ICollection<PublicacaoAlteracaoRevogacaoDTO> publicacaoRevogacao)
        {
            try
            {
                if (publicacaoRevogacao.Count > 0)
                {
                    // eliminando anteriores...
                    var eliminar = this.PublicacaoAlteracaoRevogacao(null, publicacaoRevogacao.First().PUB_ID).lista;
                    DeleteAll(eliminar, "PAR_ID");

                    // salvando atuais...
                    foreach (var s in publicacaoRevogacao)
                    {
                        if (s.PUB_ID != null && s.PAR_TIPO != null && s.TIP_ATO_ID != null && s.PUB_NUMERO_ATO != null)
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
