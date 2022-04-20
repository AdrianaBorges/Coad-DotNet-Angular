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
    [ServiceConfig("ID_PARCELA")]
    public class ParcelaPendenteSRV : ServiceAdapter<PARCELA_PENDENTE, ParcelaPendenteSRV, int>
    {
        public ParcelaPendenteDAO _dao = new ParcelaPendenteDAO();

        public ParcelaPendenteSRV()
        {
            SetDao(_dao);
        }

        public void ExcluirPorParcela(string _parcela)
        {
            new ParcelaPendenteDAO().ExcluirPorParcela(_parcela);
        }

        public IList<ParcelaPendenteDTO> ListarClientePorParcela(string parcela)
        {
            return _dao.FindByNumParcela(parcela);

        }

        public IList<ParcelaPendenteDTO> AlterarSituacaoDeParcelaAgendada(IList<ParcelaPendenteDTO> parcelas)
        {
            if (parcelas != null)
            {
                this.DeleteAll(parcelas);
            }

            return parcelas;

        }

    }
}
