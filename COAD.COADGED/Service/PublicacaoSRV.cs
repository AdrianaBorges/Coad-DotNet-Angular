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

    public class PublicacaoSRV : GenericService<PUBLICACAO, PublicacaoDTO, int>
    {
        private PublicacaoDAO _dao = new PublicacaoDAO();

        public PublicacaoSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoDTO> Publicacoes(int? tpAto = null, string nrAto = null, DateTime? dtAto = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Publicacoes(tpAto, nrAto, dtAto, pagina, itensPorPagina);
            return resp;
        }

        public PublicacaoDTO SalvarPublicacao(PublicacaoDTO publicacao)
        {
            try
            {
                if (publicacao.PUB_ID != null)
                {
                    publicacao.DATA_CADASTRO = publicacao.DATA_CADASTRO == null? DateTime.Now: publicacao.DATA_CADASTRO;
                    publicacao.DATA_ALTERA = DateTime.Now;
                    return Merge(publicacao, "PUB_ID");
                }
                else
                {
                    publicacao.DATA_CADASTRO = DateTime.Now;
                    return Save(publicacao);
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
