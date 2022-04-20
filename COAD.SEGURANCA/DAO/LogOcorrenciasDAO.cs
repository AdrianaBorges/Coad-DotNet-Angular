using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;


namespace COAD.SEGURANCA.DAO
{
    public class LogOcorrenciasDAO : RepositorioPadrao<LOG_OCORRENCIA>
    {
        public LOG_OCORRENCIA Buscar(int id)
        {
            var _log = (from l in db.LOG_OCORRENCIA
                            where l.LOG_SEQ == id
                            select l).First();

            return _log;
        }
        public List<LOG_OCORRENCIA> Listar(DateTime? dtini,DateTime? dtfim,string _login)
        {
            List<LOG_OCORRENCIA> _log = (from l in db.LOG_OCORRENCIA
                                        where ((dtini == null) || ( l.LOG_DATA >= dtini && l.LOG_DATA <= dtfim)) &&
                                              ((_login == "") || ((_login != "") && (_login == l.USU_LOGIN)))     
                                       orderby l.LOG_DATA descending
                                       select l).ToList();

            return _log;
        }

    }
}
