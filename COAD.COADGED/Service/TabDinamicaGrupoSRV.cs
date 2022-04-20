using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    public class TabDinamicaGrupoSRV : GenericService<TAB_DINAMICA_GRUPO, TabDinamicaGrupoDTO, int>
    {
        private TabDinamicaGrupoDAO _dao = new TabDinamicaGrupoDAO();

        public TabDinamicaGrupoSRV()
        {
            Dao = _dao;
        }
        public IList<TabDinamicaGrupoDTO> BuscarGrupoTabela(int _tipo, int? _tgr_id, int? _tgr_tipo)
        {
            return _dao.BuscarGrupoTabela(_tipo, _tgr_id, _tgr_tipo);
        }
    }

}
