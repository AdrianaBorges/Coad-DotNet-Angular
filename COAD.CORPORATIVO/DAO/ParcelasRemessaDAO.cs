using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ParcelasRemessaDAO : DAOAdapter<PARCELAS_REMESSA, ParcelasRemessaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ParcelasRemessaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<ParcelasRemessaDTO> Listar(string status)
        {
            try
            {
                var _query = (from p in db.PARCELAS_REMESSA
                              where ((status == null) || (status != null && p.REM_TRANSMITIDO == status)) && (p.REM_DATA_DESALOCACAO == null)
                              select p).OrderByDescending(x => x.REM_DATA);

                return ToDTO(_query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IList<ParcelasRemessaDTO> ListarRemessa(DateTime _dtini, DateTime _dtfinal, int _emp_id, string _ban_id = null)
        {
            try
            {
                if (_ban_id == "")
                    _ban_id = null;

                _dtfinal = _dtfinal.AddDays(1);

                var _query = (from p in db.PARCELAS_REMESSA
                                    //join parc in db.PARCELAS on p.REM_ID equals parc.REM_ID
                                where (p.REM_DATA >= _dtini && p.REM_DATA < _dtfinal) && (p.EMP_ID == _emp_id) && (_ban_id == null || (_ban_id != null && p.BAN_ID == _ban_id))
                                select p).OrderByDescending(x => x.REM_ID);

                return ToDTO(_query);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Pagina<ParcelasRemessaDTO> ListarRemessa(DateTime _dtini, DateTime _dtfinal, int _emp_id, string _ban_id = null, int pagina = 1, int itensPorPagina = 10)
        {
            try
            {
                if (_ban_id == "")
                    _ban_id = null;

                _dtfinal = _dtfinal.AddDays(1);

                if (_emp_id > -1)
                {

                    var _query = (from p in db.PARCELAS_REMESSA
                                  where (p.REM_DATA >= _dtini && p.REM_DATA < _dtfinal) && (p.EMP_ID == _emp_id) && (_ban_id == null || (_ban_id != null && p.BAN_ID == _ban_id))
                                  select p).OrderByDescending(x => x.REM_ID);


                    return ToDTOPage(_query, pagina, itensPorPagina);

                }
                else
                {

                    var _query = (from p in db.PARCELAS_REMESSA
                                  where (p.REM_DATA >= _dtini && p.REM_DATA < _dtfinal) && (_ban_id == null || (_ban_id != null && p.BAN_ID == _ban_id))
                                  select p).OrderByDescending(x => x.REM_ID);


                    return ToDTOPage(_query, pagina, itensPorPagina);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Pagina<ParcelasRemessaDTO> ResumoAlocacao(DateTime _dtini, DateTime _dtfinal, int _emp_id, string _ban_id = null, int pagina = 1, int itensPorPagina = 10)
        {
            try
            {
                if (_ban_id == "")
                    _ban_id = null;

                _dtfinal = _dtfinal.AddDays(1);

                var _query = (from r in db.PARCELAS_REMESSA
                              join p in db.PARCELAS on r.REM_ID equals p.REM_ID
                              select r);
                              //where (r.REM_DATA >= _dtini && r.REM_DATA < _dtfinal)
                              //   && (p.EMP_ID == _emp_id)
                              //   && (_ban_id == null || (_ban_id != null && p.BAN_ID == _ban_id))
                              //group p by new
                              //{
                              //    p.BAN_ID,
                              //    p.BANCOS.BAN_NOME,
                              //    r.TIPO_REMESSA

                              //} into f
                              //orderby f.Key.BAN_ID
                              //select f).ToList();


                return ToDTOPage(_query, pagina, itensPorPagina);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}
