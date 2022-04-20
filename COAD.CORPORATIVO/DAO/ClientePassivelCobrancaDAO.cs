using System;
using System.Collections.Generic;
using System.Linq;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class ClientePassivelCobrancaDAO : DAOAdapter<CLIENTE_PASSIVEL_COBRANCA, ClientePassivelCobrancaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ClientePassivelCobrancaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<ClientePassivelCobrancaDTO> FindByNumParcela(string codigoParcela)
        {
            if (!string.IsNullOrWhiteSpace(codigoParcela))
            {
                var query = GetDbSet().Where(x => x.PAR_NUM_PARCELA == codigoParcela);

                return ToDTO(query);
            }

            return new List<ClientePassivelCobrancaDTO>();
        }

        public void ExcluirPorParcela(string _parcela)
        {
            try
            {
                //PARCELA_PENDENTE
                List<CLIENTE_PASSIVEL_COBRANCA> lista = (from p in db.CLIENTE_PASSIVEL_COBRANCA
                                                         where (p.PAR_NUM_PARCELA == _parcela)
                                                         select p).ToList();

                this.DeleteAll(lista, "ID");

                foreach (var parcelas in lista)
                {
                    //this.Excluir(parcelas);


                }
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(ex.InnerException.InnerException.Message, ex.InnerException.InnerException.HResult.ToString(), SessionContext.autenticado);
                throw;
            }

        }

    }
}
