using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;

namespace COAD.CORPORATIVO.Service
{
    public class EmailsDeClienteSRV : ServiceAdapter<ASSINATURA_EMAIL, AssinaturaEmailDTO>
    {
        private EmailsDeClienteDAO _dao = new EmailsDeClienteDAO();

        public EmailsDeClienteSRV()
        {
        }
        public IList<AssinaturaEmailDTO> BuscarEmails(string _assinatura, int _cli_id)
        {
            return _dao.BuscarEmails(_assinatura, _cli_id);
        }
        public IList<AssinaturaEmailDTO> BuscarEmailsNF(int _nf_numero, int _cli_id)
        {
            var _ctr = new ContratoSRV().BuscarContratoNF(_nf_numero, _cli_id);

            if (_ctr == null)
                throw new Exception("Contrato não encontrado");

            return this.BuscarEmails(_ctr.ASN_NUM_ASSINATURA, _cli_id);
        }

        public IList<AssinaturaEmailDTO> BuscarEmailsContrato(string contrato, int _cli_id)
        {
            var _ctr = new ContratoSRV().FindById(contrato);

            if (_ctr == null)
                throw new Exception("Contrato não encontrado");

            return _dao.BuscarEmails(_ctr.ASN_NUM_ASSINATURA, _cli_id);
        }

        public IList<AssinaturaEmailDTO> BuscarEmailsDeCliente(string assinatura)
        {
            return _dao.BuscarEmailsDeCliente(assinatura);
        }
        public IList<AssinaturaEmailDTO> BuscarEmailPorBoleto(string _parcela)
       {
            return _dao.BuscarEmailPorBoleto(_parcela);
        }
    }
}
