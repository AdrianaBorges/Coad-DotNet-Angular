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
    public class AcaoAtendimentoDAO : DAOAdapter<ACAO_ATENDIMENTO, AcaoAtendimentoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AcaoAtendimentoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public bool ImpEtiqueta(int _acao_id)
        {       
            return (bool)db.ACAO_ATENDIMENTO.Where(x => x.ACA_ID == _acao_id).FirstOrDefault().ACA_IMP_ETIQUETA;
        }
		public void OutroTeste(){
			//testeeee
		}
    }
}
