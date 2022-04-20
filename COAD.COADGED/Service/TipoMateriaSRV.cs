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

    public class TipoMateriaSRV : GenericService<TIPO_MATERIA, TipoMateriaDTO, int>
    {
        private TipoMateriaDAO _dao = new TipoMateriaDAO();

        public TipoMateriaSRV()
        {
            Dao = _dao;
        }

        public Pagina<TipoMateriaDTO> TiposMaterias(int? tipoMateriaId, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.TiposMaterias(tipoMateriaId, descricao, ativoId, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarTipoMateria(TipoMateriaDTO tipoMateria)
        {
            try
            {
                if (tipoMateria.TIP_MAT_ID == null && _dao.TiposMaterias(null, tipoMateria.TIP_MAT_DESCRICAO).lista.Count() > 0)
                {
                    throw new Exception("Já existe um tipo de matéria cadastrada com este nome!");
                }
                else
                {
                    tipoMateria.TIP_MAT_ATIVO = tipoMateria.TIP_MAT_ATIVO == null ? 1 : tipoMateria.TIP_MAT_ATIVO;

                    if (tipoMateria.TIP_MAT_ID != null)
                    {
                        Merge(tipoMateria, "TIP_MAT_ID");
                    }
                    else
                    {
                        Save(tipoMateria);
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
