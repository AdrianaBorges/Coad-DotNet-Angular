using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("CODIGO_UNIX")]
    public class AssinaturaLegadoSRV : GenericService<ASSINATURA, AssinaturaLegadoDTO, object>
    {
        private AssinaturaLegadoDAO _dao = new AssinaturaLegadoDAO();
        private Telefones2SRV _telefoneSRV = new Telefones2SRV();
        private EmailsSRV _emailsSRV = new EmailsSRV();

        public AssinaturaLegadoSRV()
        {
            Dao = _dao;
        }

        public AssinaturaLegadoDTO SalvarAssinaturaLegado(AssinaturaLegadoDTO assin)
        {
            if (assin != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(assin);
                var codAssi = assin.CODIGO_UNIX;

                var emails = assin.EMAILS;
                var telefones = assin.TELEFONES2;

                var emailsSalvos = _emailsSRV.SalvarEmails(emails, codAssi);
                var telefonesSalvos = _telefoneSRV.SalvarTelefones(telefones, codAssi);


                if (emails != null && emailsSalvos != null)
                {
                    var lstEmails = emails.ToList();
                    var lstTelefones = telefones.ToList();
                    var lstEmailsSalvos = emailsSalvos.ToList();
                    var lstTelefonesSalvos = telefonesSalvos.ToList();

                    _processarReferenciaEmail(lstEmails, lstEmailsSalvos);
                    _processarReferenciaTelefone(lstTelefones, lstTelefonesSalvos);
                        


                    assin.EMAILS = lstEmailsSalvos;
                    assin.TELEFONES2 = lstTelefonesSalvos;
                }
                return assin;
            }
            return null;
        }

        private void _processarReferenciaEmail(IList<EmailsDTO> emails, IList<EmailsDTO> emailsSalvos)
        {
            if (emails != null && emailsSalvos != null)
            {
                var lstEmails = emails.ToList();
                var lstEmailsSalvos = emailsSalvos.ToList();

                var length = lstEmails.Count();
                var list = lstEmails.ToList();

                for (var index = 0; index < length; index++)
                {
                    var email = lstEmails[index];
                    var emailSalvo = lstEmailsSalvos[index];

                    emailSalvo.IdEmailCoadCorp = email.IdEmailCoadCorp;
                }

            }
        }

        private void _processarReferenciaTelefone(IList<Telefones2DTO> telefones, IList<Telefones2DTO> telefonesSalvos)
        {

            if (telefones != null && telefonesSalvos != null)
            {
                var lstTelefones = telefones.ToList();
                var lstTelefonesSalvos = telefonesSalvos.ToList();

                var length = lstTelefones.Count();

                for (var index = 0; index < length; index++)
                {
                    var telefone = lstTelefones[index];
                    var telefoneSalvo = lstTelefonesSalvos[index];
                    
                    telefoneSalvo.IdTelCoadCorp = telefone.IdTelCoadCorp;
                }

            }
        }

        public void TransferirVigencia(string vASSIN_ANT, string vASSIN_ATU, string vSOLIC, string vDATA_TRANSF, string vVIGENCIA, string vCONTRATO, Nullable<int> vMES_REFERENCIA, Nullable<System.DateTime> vDATA_INI_VIGENCIA, Nullable<System.DateTime> vDATA_FIM_VIGENCIA, string uSU_LOGIN, string aSN_TRANSF_MOTIVO)
        {
            _dao.TransferirVigencia(vASSIN_ANT, 
                                    vASSIN_ATU, 
                                    vSOLIC, 
                                    vDATA_TRANSF, 
                                    vVIGENCIA, 
                                    vCONTRATO, 
                                    vMES_REFERENCIA, 
                                    vDATA_INI_VIGENCIA, 
                                    vDATA_FIM_VIGENCIA, 
                                    uSU_LOGIN, 
                                    aSN_TRANSF_MOTIVO);
        }


        public void AdicionarConsultaNaAssinatura(string codAssinatura, int? qtdConsultas, string usuLogin)
        {
           // var bloqueiaConsulta = new BloqueiaConsultaIndividualSRV();
           // bloqueiaConsulta.AdicionarConsultas(codAssinatura, qtdConsultas, usuLogin);
        }
    }
}
