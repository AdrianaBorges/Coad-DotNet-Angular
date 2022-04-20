using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.COADGED.Service
{
    public class OrigemFuncionalidadeSRV : GenericService<ORIGEM_FUNCIONALIDADE, OrigemFuncionalidadeDTO, object>
    {
        private OrigemFuncionalidadeDAO _dao = new OrigemFuncionalidadeDAO();

        public OrigemFuncionalidadeSRV()
        {
            Dao = _dao;
        }
        public Pagina<OrigemFuncionalidadeDTO> ListarOrigem(string _descricao = null, int _pagina = 1, int _itensPorPagina = 10)
        {
            return _dao.ListarOrigem(_descricao, _pagina, _itensPorPagina);
        }

        public void SalvarOrigemAcesso(OrigemAcessoDTO _origem, List<OrigemFuncionalidadeDTO> _origemfunc)
        {
            try
            {
                OrigemAcessoSRV _origsrv = new OrigemAcessoSRV();

                using (TransactionScope scope = new TransactionScope())
                {

                    OrigemAcessoDTO _ori = _origsrv.FindById(_origem.OAC_ID);

                     _origem.USU_LOGIN = SessionContext.autenticado.USU_LOGIN; 

                    if (_ori == null)
                    {
                        _origem.DATA_ALTERA = DateTime.Now;
                        _origem = _origsrv.Save(_origem);
                        
                        OrigemAcessoRefDTO _source = new OrigemAcessoRefDTO();
                        _source.OAC_ID = _origem.OAC_ID;
                        new OrigemAcessoRefSRV().Save(_source);
                    }
                    else
                        _ori = _origsrv.Merge(_origem, "OAC_ID");

                    //-----------
                    //--- Excluir os dados ja existentes (OrigemFuncionalidadeDTO)
                    //-----------

                    IList<OrigemFuncionalidadeDTO> _lista = this.ListarFuncionalidades(_origem.OAC_ID);

                    foreach (OrigemFuncionalidadeDTO _item in _lista)
                    {
                        this.Delete(_item, "OAC_ID", "FCI_ID");
                    }

                    //-----------
                    //--- Realiza a inclusão ou alteração dados (OrigemFuncionalidadeDTO)
                    //-----------

                    foreach (OrigemFuncionalidadeDTO _item in _origemfunc)
                    {
                        _item.OAC_ID = _origem.OAC_ID;

                        OrigemFuncionalidadeDTO _orif = this.FindById(_item.OAC_ID, _item.FCI_ID);

                        _item.USU_LOGIN = SessionContext.autenticado.USU_LOGIN; 
                        
                        if (_orif == null)
                        {
                            _item.OFU_DATA_ALTERA = DateTime.Now;
                            _orif = this.Save (_item);
                        }
                        else
                            _orif = this.Merge(_item, "OAC_ID", "FCI_ID");

                    }
         
                    //------------

                    scope.Complete();

                }
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                throw new Exception(SysException.Show(ex));
            }
        }
        public IList<OrigemFuncionalidadeDTO> ListarFuncionalidades(int _oac_id)
        {
            return _dao.ListarFuncionalidades(_oac_id);
        }
        public IList<OrigemFuncionalidadeDTO> ListarFuncionalidades(int _oac_id, string _posicao)
        {
            return _dao.ListarFuncionalidades(_oac_id, _posicao);
        }

    }
}
