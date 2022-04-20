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

    public class PublicacaoBuscaSRV : GenericService<PUBLICACAO_BUSCA, PublicacaoBuscaDTO, int>
    {
        private PublicacaoBuscaDAO _dao = new PublicacaoBuscaDAO();

        public PublicacaoBuscaSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoBuscaDTO> PublicacaoBusca(int? pubId, int? colecionadorId, string palavra = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.PublicacaoBusca(pubId, colecionadorId, palavra, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarPublicacaoBusca(ICollection<PublicacaoBuscaDTO> publicacaoBusca)
        {
            try
            {
                if (publicacaoBusca.Count > 0)
                {
                    // eliminando anteriores...
                    var eliminar = this.PublicacaoBusca(publicacaoBusca.First().PUB_ID, publicacaoBusca.First().ARE_CONS_ID).lista;
                    DeleteAll(eliminar, "PBU_ID");

                    // salvando atuais...
                    foreach (var s in publicacaoBusca)
                    {
                        if (s.PUB_ID != null && s.ARE_CONS_ID != null && s.PBU_PALAVRA != null)
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
