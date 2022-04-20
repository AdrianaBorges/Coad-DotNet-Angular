using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.PortalBusca;
using COAD.PORTAL.Model.DTO.PortalBusca;
using COAD.PORTAL.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Service.PortalBusca
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...
    public class Busca_completa_tributarioSRV : GenericService<busca_completa_tributario, Busca_completa_tributarioDTO, int>
    {
        private Busca_completa_tributarioDAO _dao = new Busca_completa_tributarioDAO();

        public Busca_completa_tributarioSRV()
        {
            Dao = _dao;
        }

        public Pagina<Busca_completa_tributarioDTO> Busca(int? idGED, int? id=null, int? id_conteudo=null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Busca(idGED, id, id_conteudo, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarBuscaCompletaTributario(Busca_completa_tributarioDTO busca)
        {
            try
            {
                if (busca.idGED != null)
                {
                    Merge(busca, "idGED");
                }
                else
                {
                    Save(busca);
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
