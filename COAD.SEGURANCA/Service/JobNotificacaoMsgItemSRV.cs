

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Models;

namespace COAD.SEGURANCA.Service
{ 
	[ServiceConfig("NTM_ID")]
	public class JobNotificacaoMsgItemSRV : GenericService<JOB_NOTIFICACAO_MSG_ITEM, JobNotificacaoMsgItemDTO, Int32>
	{

        public JobNotificacaoMsgItemDAO _dao { get; set; }

        public JobNotificacaoMsgItemSRV(JobNotificacaoMsgItemDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

        public IList<JobNotificacaoMsgItemDTO> ListarJobNotificacaoMsgItem(int? jnf)
        {
            return _dao.ListarJobNotificacaoMsgItem(jnf);
        }


        public ICollection<JobNotificacaoMsgItemDTO> CriarNotificacaoMsgItem(ICollection<NotifyHandlerResultItem> lstMsg)
        {
            ICollection<JobNotificacaoMsgItemDTO> lstMsgResult = new HashSet<JobNotificacaoMsgItemDTO>();
            if(lstMsg != null)
            {
                lstMsgResult = lstMsg.Select(x => new JobNotificacaoMsgItemDTO() {

                    NTM_COD_REF = x.CodRef,
                    NTM_COD_REF_STR = x.CodRefStr,
                    NTM_DATA = x.Data,
                    NTM_MENSAGEM = x.Mensagem
                })
                .ToList();
            }

            return lstMsgResult;
        }

        public void SalvarNotMsgItem(JobNotificacaoDTO jobNotificacao)
        {
            if(jobNotificacao != null && jobNotificacao.JOB_NOTIFICACAO_MSG_ITEM != null)
            {
                var lstNotMsgItem = jobNotificacao.JOB_NOTIFICACAO_MSG_ITEM;
                CheckAndAssignKeyFromParentToChildsList(jobNotificacao, lstNotMsgItem, "JNF_ID");

                SaveOrUpdateAll(lstNotMsgItem);
            }
        }
    }
}
