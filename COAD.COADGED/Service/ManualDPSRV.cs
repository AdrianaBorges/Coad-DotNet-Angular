using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.COADGED.Service
{
    [ServiceConfig("MAN_ID")]
    public class ManualDPSRV : GenericService<MANUAL_DP, ManualDPDTO, int>
    {
        private ManualDPDAO _dao = new ManualDPDAO();

        public ManualDPSRV()
        {
            Dao = _dao;
        }

             
        public IList<ManualDPDTO> Listar(string _assunto, int _mod_id)
        {
            return _dao.Listar(_assunto, _mod_id);

        }
        public IList<ManualDPDTO> Listar(string _assunto = null)
        {
            return _dao.Listar(_assunto);

        }
        public void PublicarAssuntoGeral(ManualDPDTO _manual, int _tipo)
        {
            try
            {
                var _lista = new ManualDPItemSRV().ListarPorAssunto((int)_manual.MAN_ID);

                TransactionOptions txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {

                    if (_tipo == 0)
                        _manual.MAN_DATA_PUBLICACAO = null;
                    else
                        _manual.MAN_DATA_PUBLICACAO = DateTime.Now;

                    this.Merge(_manual);

                    foreach (var _item in _lista)
                    {
                        _item.DATA_ALTERA = DateTime.Now;
                        _item.USU_LOGIN_ALT = SessionContext.autenticado.USU_LOGIN;

                        if (_tipo == 0)
                            _item.MAI_DATA_PUBLICACAO = null;
                        else
                            _item.MAI_DATA_PUBLICACAO = DateTime.Now;

                        new ManualDPItemSRV().Merge(_item);

                    }

                    scope.Complete();

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public void RestaurarIndice(ManualDPDTO _manual)
        {
            try
            {
                var _lista = new ManualDPItemSRV().ListarPorAssunto((int)_manual.MAN_ID);

                TransactionOptions txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    var index = 0;

                    foreach (var _item in _lista)
                    {
                        _item.DATA_ALTERA = DateTime.Now;
                        _item.USU_LOGIN_ALT = SessionContext.autenticado.USU_LOGIN;
                        _item.MAI_INDEX = index;
                        _item.MAI_NIVEL = 0;

                        new ManualDPItemSRV().Merge(_item);

                        index += 1;
                    }

                    scope.Complete();

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
