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
    public class EmailAtendAnexoSRV : ServiceAdapter<EMAIL_ATEND_ANEXO, EmailAtendAnexoDTO, int>
    {
        private EmailAtendAnexoDAO _dao = new EmailAtendAnexoDAO();

        public EmailAtendAnexoSRV()
        {
            SetDao(_dao);
            SetKeys("ANX_ID","EAT_ID");
        }

        public List<EmailAtendAnexoDTO> BuscarPorEmail(int _eat_id)
        {
            return _dao.BuscarPorEmail(_eat_id).ToList();
        }
    
    }
}
