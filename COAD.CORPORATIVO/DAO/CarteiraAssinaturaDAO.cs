using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;


namespace COAD.CORPORATIVO.DAO
{
    public class CarteiraAssinaturaDAO : AbstractGenericDao<CARTEIRA_ASSINATURA, CarteiraAssinaturaDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CarteiraAssinaturaDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        private IQueryable<CARTEIRA_ASSINATURA> TemplateCarteiraAssinatura(string CAR_ID, string ASS_NUM_ASSINATURA)
        {
            IQueryable<CARTEIRA_ASSINATURA> query = db.CARTEIRA_ASSINATURA.Where(x => x.CAR_ID == CAR_ID && x.ASN_NUM_ASSINATURA == ASS_NUM_ASSINATURA);
            return query;
        }

        public bool HasCarteiraAssinatura(string CAR_ID, string ASS_NUM_ASSINATURA)
        {
            var query = TemplateCarteiraAssinatura(CAR_ID, ASS_NUM_ASSINATURA);
            var resp = (query.Count() > 0);
            return resp;            
        }

        public IList<CarteiraAssinaturaDTO> FindByCarId(string CAR_ID)
        {
            var query = db.CARTEIRA_ASSINATURA.Where(x => x.CAR_ID == CAR_ID);
            return ToDTO(query);
        }

        public IList<CarteiraAssinaturaDTO> FindByAssinaturaERegiao(string ASN_NUM_ASSINATURA, int? RG_ID)
        {
            var query = db.CARTEIRA_ASSINATURA.Where(x => x.ASSINATURA.ASN_NUM_ASSINATURA == ASN_NUM_ASSINATURA && x.CARTEIRA.RG_ID == RG_ID);
            return ToDTO(query);
        }

        public IList<CarteiraAssinaturaDTO> BuscarClientes(string _car_id, string _asn_num_assinatura)
        {
            var query = (from a in db.ASSINATURA 
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         join c in db.CARTEIRA_ASSINATURA on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA into gj from subpet in gj.DefaultIfEmpty()
                         where ((_car_id == null || _car_id == "" ) || (subpet.CAR_ID == _car_id))
                            && ((_asn_num_assinatura == null || _asn_num_assinatura == "") || (a.ASN_NUM_ASSINATURA == _asn_num_assinatura))
                         select new CarteiraAssinaturaDTO()
                         {
                            ASN_NUM_ASSINATURA = a.ASN_NUM_ASSINATURA
                            ,CAR_ID = subpet.CAR_ID
                            ,CLI_ID = l.CLI_ID
                            ,CLI_NOME = l.CLI_NOME

                         }).ToList();

            return query;

        }

    }
}
