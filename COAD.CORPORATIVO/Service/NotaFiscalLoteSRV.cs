using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("NFL_ID")]
    public class NotaFiscalLoteSRV : GenericService<NOTA_FISCAL_LOTE, NotaFiscalLoteDTO, Int32>
    {

        public NotaFiscalLoteDAO _dao { get; set; }

        public NotaFiscalLoteSRV(NotaFiscalLoteDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public void CadastrarLoteNotaFiscal(ICollection<int> lstCodPedido)
        {
            if (lstCodPedido != null)
            {
                var lote = new NotaFiscalLoteDTO()
                {
                    NFL_DATA = DateTime.Now,
                    NLS_ID = 1
                };

            }
        }

        public NotaFiscalLoteDTO RetornarLotePorRecibo(string codRecibo)
        {
            return _dao.RetornarLotePorRecibo(codRecibo);
        }

        public NotaFiscalLoteDTO RetornarLoteVigente(int? empID)
        {
            return _dao.RetornarLoteVigente(empID);
        }

        public NotaFiscalLoteDTO RetornarLoteParaEnviar()
        {
            return _dao.RetornarLoteParaEnviar();
        }

        public NotaFiscalLoteDTO RetornarLoteParaProcessar()
        {
            return _dao.RetornarLoteParaProcessar();
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLotesParaEnviar()
        {
            return _dao.RetornarLotesParaEnviar();
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLotesParaProcessar()
        {
            return _dao.RetornarLotesParaProcessar();
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLoteNFseParaEnviar()
        {
            return _dao.RetornarLoteNFseParaEnviar();
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLotesNFseParaProcessar()
        {
            return _dao.RetornarLotesNFseParaProcessar();
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLoteAutorizadoItemNaoEnviado()
        {
            return _dao.RetornarLoteAutorizadoItemNaoEnviado();
        }

        public NotaFiscalLoteDTO FindByIdFullLoaded(int? nflId, bool trazItens = false)
        {
            var notaFiscalLote = FindById(nflId);

            if(trazItens && notaFiscalLote != null)
            {
                ServiceFactory.RetornarServico<NotaFiscalLoteItemSRV>().PreencherItensNoLote(notaFiscalLote);
            }

            return notaFiscalLote;
        }

        public void CancelarProcessamentoLote(ICollection<int> lstNflId)
        {

            if(lstNflId != null)
            {
                var lstLote = new List<NotaFiscalLoteDTO>();
                using (var scope = new TransactionScope())
                {

                    foreach (var nflId in lstNflId)
                    {
                        var lote = FindById(nflId);

                        if (lote != null)
                        {
                            lote.NLS_ID = 12;
                            lstLote.Add(lote);
                        }
                    }
                    MergeAll(lstLote);
                    scope.Complete();
                }
            }
        }

        public void SalvarLoteDosItens(ICollection<NotaFiscalLoteItemDTO> lstLotesItens)
        {
            if(lstLotesItens != null)
            {
                var lstLotes = lstLotesItens.Select(x => x.NOTA_FISCAL_LOTE);

                MergeAll(lstLotes);
            }
        }
    }
}
