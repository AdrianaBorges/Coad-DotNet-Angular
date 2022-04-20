using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.UTIL.Helpers;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("ID")]
    public class ClientePassivelCobrancaSRV : ServiceAdapter<CLIENTE_PASSIVEL_COBRANCA, ClientePassivelCobrancaDTO, int>
    {

        public ClientePassivelCobrancaDAO _dao = new ClientePassivelCobrancaDAO();

        public ClientePassivelCobrancaSRV()
        {
            SetDao(_dao);
        }

        public void ExcluirPorParcela(string _parcela)
        {
            new ClientePassivelCobrancaDAO().ExcluirPorParcela(_parcela);
        }


        public IList<ClientePassivelCobrancaDTO> ListarClientePorParcela(string parcela)
        {
            return _dao.FindByNumParcela(parcela);

        }

        //public void ExcluirRegistroPassivelDeCobranca(IList<ClientePassivelCobrancaDTO> parcelas)
        //{
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            new ClientePassivelCobrancaSRV().Excluir(parcelas);

        //            scope.Complete();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(SysException.Show(ex));
        //    }

        //}

        public IList<ClientePassivelCobrancaDTO> AlterarSituacaoDeParcelaAgendada(IList<ClientePassivelCobrancaDTO> parcelas)
        {
            if (parcelas != null)
            {
                //foreach (var par in parcelas)
                //{
                //    if (par.PAR_NUM_PARCELA != null)
                //    {
                //        par.PAR_DATA_VENCTO = DateTime.Now;
                //        par.PAR_SITUACAO = "PRO";
                //    }
                //}

                this.DeleteAll(parcelas);
                //this.MergeAll(parcelas);

            }

            return parcelas;

        }

    }
}
