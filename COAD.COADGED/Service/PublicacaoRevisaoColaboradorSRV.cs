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
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PublicacaoRevisaoColaboradorSRV : GenericService<PUBLICACAO_REVISAO_COLABORADOR, PublicacaoRevisaoColaboradorDTO, int>
    {
        private PublicacaoRevisaoColaboradorDAO _dao = new PublicacaoRevisaoColaboradorDAO();

        public PublicacaoRevisaoColaboradorSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoRevisaoColaboradorDTO> PublicacaoRevisaoColaborador(int? pubId = null, int? colecionadorId = null, int? colId = null, DateTime? data = null, string revisao = null, string editada = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.PublicacaoRevisaoColaborador(pubId, colecionadorId, colId, data, revisao, editada, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarPublicacaoRevisaoColaborador(PublicacaoRevisaoColaboradorDTO publicacaoRevisaoColaborador)
        {
            try
            {
                if (publicacaoRevisaoColaborador.REV_ID != null)
                {
                    Merge(publicacaoRevisaoColaborador, "REV_ID");
                }
                else
                {
                    Save(publicacaoRevisaoColaborador);
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
