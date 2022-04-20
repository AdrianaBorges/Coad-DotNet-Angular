using System;
using System.Collections.Generic;
using System.Linq;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class ParcelaPendenteDAO : DAOAdapter<PARCELA_PENDENTE, ParcelaPendenteDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ParcelaPendenteDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<ParcelaPendenteDTO> FindByNumParcela(string codigoParcela)
        {
            if (!string.IsNullOrWhiteSpace(codigoParcela))
            {
                var query = GetDbSet().Where(x => x.PAR_NUM_PARCELA == codigoParcela);

                return ToDTO(query);
            }

            return new List<ParcelaPendenteDTO>();
        }

        public void ExcluiRegistroPorParcela(string _parcela)
        {
            try
            {
                List<PARCELA_PENDENTE> lista = (from p in db.PARCELA_PENDENTE
                                                         where (p.PAR_NUM_PARCELA == _parcela)
                                                         select p).ToList();

                this.DeleteAll(lista, "ID_PARCELA");

                foreach (var parcelas in lista)
                {
                    this.Excluir(parcelas);

                }
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(ex.InnerException.InnerException.Message, 
                    
                    ex.InnerException.InnerException.HResult.ToString(), SessionContext.autenticado);
                throw;
            }

        }
    }
}
