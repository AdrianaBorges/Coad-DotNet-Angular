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

    public class TipoAtoSRV : GenericService<TIPO_ATO, TipoAtoDTO, int>
    {
        private TipoAtoDAO _dao = new TipoAtoDAO();

        public TipoAtoSRV()
        {
            Dao = _dao;
        }
        public IList<TipoAtoDTO> Listar(int? _situacao)
        {
            return _dao.Listar(_situacao);
        }

        public Pagina<TipoAtoDTO> TiposAtos(int? tipoAtoId, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.TiposAtos(tipoAtoId, descricao, ativoId, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarTipoAto(TipoAtoDTO tipoAto)
        {
            try
            {
                if (tipoAto.TIP_ATO_ID == null && _dao.TiposAtos(null, tipoAto.TIP_ATO_DESCRICAO).lista.Count() > 0)
                {
                    throw new Exception("Já existe um tipo de ato cadastrado com este nome!");
                }
                else
                {
                    tipoAto.TIP_ATIVO = tipoAto.TIP_ATIVO == null ? 1 : tipoAto.TIP_ATIVO;

                    if (tipoAto.TIP_ATO_ID != null)
                    {
                        tipoAto.DATA_ALTERA = DateTime.Now;
                        Merge(tipoAto, "TIP_ATO_ID");
                    }
                    else
                    {
                        tipoAto.DATA_CADASTRO = DateTime.Now;
                        Save(tipoAto);
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
