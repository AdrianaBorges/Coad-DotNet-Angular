using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class UraLogSRV : ServiceAdapter<URA_LOG, UraLogDTO>
    {
        public UraLogDAO _dao = new UraLogDAO();

        public UraLogSRV()
        {
           SetDao(_dao);
        }

        public void Registrar(string _ura, string _assinatura, int _tipo, string _obs, string _login)
        {
            UraLogDTO log = new UraLogDTO();
            log.URA_ID = _ura;
            log.ASN_NUM_ASSINATURA = _assinatura;
            log.URA_TP_ATU_ID = _tipo; 
            log.ULG_OBS = _obs;
            log.USU_LOGIN = _login;
            log.ULG_DATA = DateTime.Now;

            this.Save(log);
        }

    }

}
