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
    public class AssinaturaTelDAO : DAOAdapter<ASSINATURA_TELEFONE, AssinaturaTelefoneDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AssinaturaTelDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public IList<AssinaturaTelefoneDTO> BuscarTelefonesRemovidos(string _assinatura, List<int> telok)
        {
            var query = db.ASSINATURA_TELEFONE.Where(x => x.ASN_NUM_ASSINATURA == _assinatura && !telok.Contains(x.ATE_ID)).ToList();

            return ToDTO(query);
        }

        public ICollection<AssinaturaTelefoneDTO> FindAllByCliId(int CLI_ID)
        {
            var query = db.ASSINATURA.Where(x => x.CLI_ID == CLI_ID && x.UEN_ID == 1).FirstOrDefault();
            ICollection<AssinaturaTelefoneDTO> lista = new List<AssinaturaTelefoneDTO>();

            if (query != null)
            {
                lista = ToDTO(query.ASSINATURA_TELEFONE);
            }
            return lista;
        }

        public IList<AssinaturaTelefoneDTO> FindByNumAssinatura(string codigoAssinatura)
        {
            if (!string.IsNullOrWhiteSpace(codigoAssinatura))
            {
                var query = GetDbSet().Where(x => x.ASN_NUM_ASSINATURA == codigoAssinatura);
                return ToDTO(query);
            }

            return new List<AssinaturaTelefoneDTO>();
        }

        public IList<AssinaturaTelefoneDTO> FindByCliente(int? CLI_ID)
        {
            if (CLI_ID != null)
            {
                var query = GetDbSet().Where(x => x.CLI_ID == CLI_ID);
                return ToDTO(query);
            }

            return new List<AssinaturaTelefoneDTO>();
        }
        public IList<AssinaturaTelefoneDTO> BuscarTelefone(string _telefone)
        {
            var query = db.ASSINATURA_TELEFONE.Where(x => x.ATE_TELEFONE.Contains(_telefone)).ToList();

            return ToDTO(query);
        }


        public AssinaturaTelefoneDTO BuscarPorAssinatura(string _assinatura)
        {
            var query = db.ASSINATURA_TELEFONE.Where(x => x.ASSINATURA.Equals(_assinatura)).FirstOrDefault();

            return ToDTO(query);
        }

        public IList<AssinaturaTelefoneDTO> BuscarTelefonesURA(string _assinatura)
        {
            var query =  (from a in db.ASSINATURA_TELEFONE
                          join o in db.OPCAO_ATENDIMENTO on a.OPC_ID equals o.OPC_ID
                         where a.ASN_NUM_ASSINATURA  == _assinatura && o.OPC_URA == true
                        select a);
                
            return ToDTO(query);
        }


        public AssinaturaTelefoneDTO FindPrimeiroTelefoneDoClienteOuAssinatura(int? CLI_ID)
        {
            if (CLI_ID != null)
            {
                var query = GetDbSet().Where(x =>
                    x.CLI_ID == CLI_ID ||
                    x.ASSINATURA.CLI_ID == CLI_ID);
                return ToDTO(query).FirstOrDefault();
            }

            return null;
        }

        public IList<AssinaturaTelefoneDTO> ListarDeTodasAsAssinaturas(int? cliId)
        {
            var query = (from assTel in 
                             db.ASSINATURA_TELEFONE join
                            ass in db.ASSINATURA on assTel.ASN_NUM_ASSINATURA equals ass.ASN_NUM_ASSINATURA
                         where ass.CLI_ID == cliId
                         select assTel);
            return ToDTO(query);
        }

    }
}
