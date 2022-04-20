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

    //public class GrupoConsultoriaSRV : GenericService<GRUPO_CONSULTORIA, GrupoConsultoriaDTO, int>
    //{
    //    private GrupoConsultoriaDAO _dao = new GrupoConsultoriaDAO();

    //    public GrupoConsultoriaSRV()
    //    {
    //        Dao = _dao;
    //    }

    //    public Pagina<GrupoConsultoriaDTO> GruposConsultoria(int? grupoConsultoriaId, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
    //    {
    //        var resp = _dao.GruposConsultoria(grupoConsultoriaId, descricao, ativoId, pagina, itensPorPagina);
    //        return resp;
    //    }

    //    public void SalvarGrupoConsultoria(GrupoConsultoriaDTO grupoConsultoria)
    //    {
    //        try
    //        {
    //            if (grupoConsultoria.GRD_CONS_ID != null)
    //            {
    //                grupoConsultoria.DATA_ALTERA = DateTime.Now;
    //                Merge(grupoConsultoria, "GRD_CONS_ID");
    //            }
    //            else
    //            {
    //                grupoConsultoria.DATA_CADASTRO = DateTime.Now;
    //                Save(grupoConsultoria);
    //            }
    //        }
    //        catch (DbEntityValidationException dbEx)
    //        {
    //            var _erro = new FormattedDbEntityValidationException(dbEx);

    //            SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

    //            throw _erro;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(SysException.Show(ex));
    //        }
    //    }

    //    public void DeletarGrupoConsultoria(int grupoConsultoriaId)
    //    {
    //        var grupoConsultoria = this.FindById(grupoConsultoriaId);
    //        //grupoConsultoria.DATA_EXCLUSAO = DateTime.Now;
    //        //Merge(grupoConsultoria, "GRD_CONS_ID");
    //    }

    //}
}
