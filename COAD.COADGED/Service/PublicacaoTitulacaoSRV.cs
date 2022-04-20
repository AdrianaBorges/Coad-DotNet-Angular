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

    public class PublicacaoTitulacaoSRV : GenericService<PUBLICACAO_TITULACAO, PublicacaoTitulacaoDTO, int>
    {
        private PublicacaoTitulacaoDAO _dao = new PublicacaoTitulacaoDAO();

        public PublicacaoTitulacaoSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoTitulacaoDTO> PublicacaoTitulacao(int? publicacaoId, int? areaId, int? principal = null, int pagina = 1, int itensPorPagina = 10) 
        {
            var resp = _dao.PublicacaoTitulacao(publicacaoId, areaId, principal, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarPublicacaoTitulacao(ICollection<PublicacaoTitulacaoDTO> publicacaoTitulacao)
        {
            try
            {
                if (publicacaoTitulacao.Count > 0)
                {
                    // eliminando anteriores...
                    var eliminar = this.PublicacaoTitulacao(publicacaoTitulacao.First().PUB_ID, publicacaoTitulacao.First().ARE_CONS_ID).lista;
                    DeleteAll(eliminar, "PTI_ID");

                    // salvando atuais...
                    foreach (var s in publicacaoTitulacao)
                    {
                        if (s.PUB_ID != null && s.ARE_CONS_ID != null && s.TIT_ID != null && s.TIT_ID_VERBETE != null && s.TIT_ID_SUBVERBETE != null)
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
