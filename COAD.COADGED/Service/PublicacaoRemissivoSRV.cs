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

    public class PublicacaoRemissivoSRV : GenericService<PUBLICACAO_REMISSIVO, PublicacaoRemissivoDTO, int>
    {
        private PublicacaoRemissivoDAO _dao = new PublicacaoRemissivoDAO();

        public PublicacaoRemissivoSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoRemissivoDTO> PublicacaoRemissivo(int? publicacaoId, int? areaId, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.PublicacaoRemissivo(publicacaoId, areaId);
            return resp;
        }

        public void SalvarPublicacaoRemissivo(ICollection<PublicacaoRemissivoDTO> publicacaoRemissivo)
        {
            try
            {
                if (publicacaoRemissivo.Count > 0)
                {
                    // eliminando anteriores...
                    var eliminar = this.PublicacaoRemissivo(publicacaoRemissivo.First().PUB_ID, publicacaoRemissivo.First().ARE_CONS_ID).lista;
                    DeleteAll(eliminar, "PRE_ID");

                    // salvando atuais...
                    foreach (var s in publicacaoRemissivo)
                    {
                        if (s.PUB_ID != null && s.ARE_CONS_ID != null && s.PRE_REMISSIVO != null)
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
