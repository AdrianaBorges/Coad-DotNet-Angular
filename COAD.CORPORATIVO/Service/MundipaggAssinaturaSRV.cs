using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using MundiAPI.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("MPG_ASN_ID")]
    public class MundipaggAssinaturaSRV : GenericService<MUNDIPAGG_ASSINATURA, MundipaggAssinaturaDTO, int>
    {
        public MundipaggAssinaturaDAO _dao { get; set; }
        public MundipaggAssinaturaSRV(MundipaggAssinaturaDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }
    }
}
