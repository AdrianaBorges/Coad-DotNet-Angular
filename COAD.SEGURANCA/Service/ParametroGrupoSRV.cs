using Coad.GenericCrud.Service.Base;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("PGR_ID")]
    public class ParametroGrupoSRV : ServiceAdapter<PARAMETRO_GRUPO, ParametroGrupoDTO, int>
    {
        private ParametroGrupoDAO _dao = new ParametroGrupoDAO();

        public ParametroGrupoSRV()
        {
            this._dao = new ParametroGrupoDAO();
            this.Dao = _dao;
        }


    }
}