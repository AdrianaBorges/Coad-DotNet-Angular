using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("AUTOID")]
    public class BloqueiaConsultaIndividualSRV : GenericService<bloqueia_consulta_individual, BloqueiaConsultaIndividualDTO, int>
    {
        private BloqueiaConsultaIndividualDAO _dao = new BloqueiaConsultaIndividualDAO();

        public BloqueiaConsultaIndividualSRV()
        {
            Dao = _dao;
        }

        public void AdicionarConsultas(string codAssinatura, int? qtdConsultas, string usuLogin)
        {
            BloqueiaConsultasDaAssinatura(codAssinatura, qtdConsultas);

            var data = DateTime.Now;
            var bloqueio = new BloqueiaConsultaIndividualDTO()
            {
                assinatura = codAssinatura,
                data = data.ToString("dd/MM/yyyy"),
                hora = data.ToString("hh:mm:ss"),
                usuario = usuLogin,
                qtd_consulta_sem = qtdConsultas,
                ativo_sn = "S",
                qtd_consulta_total = (qtdConsultas * 12),
                qtd_consulta_usou = 0,
                qtd_disponibilizar = qtdConsultas,
                qtd_consulta_acum = 0,
                per_disponibilizar = "M",
                atualizou = "",
                DATA_INSERT = data,
                USU_LOGIN = usuLogin
            };

            Save(bloqueio);
        }

        public void BloqueiaConsultasDaAssinatura(string codAssinatura, int? qtdConsultas)
        {
            var lstBlqCon = ConsultaPorAssinatura(codAssinatura);

            if (lstBlqCon != null)
            {
                foreach (var blq in lstBlqCon)
                {
                    blq.ativo_sn = "N";
                }

                SaveOrUpdateAll(lstBlqCon);
            }
        }

        public IList<BloqueiaConsultaIndividualDTO> ConsultaPorAssinatura(string codAssinatura)
        {
            return _dao.ConsultaPorAssinatura(codAssinatura);
        }

        public BloqueiaConsultaIndividualDTO ConsultarPrimeiroPorAssinatura(string codAssinatura)
        {
            return _dao.ConsultarPrimeiroPorAssinatura(codAssinatura);
        }
    }
}
