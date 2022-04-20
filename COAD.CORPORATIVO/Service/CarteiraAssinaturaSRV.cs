using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;
using System.Transactions;
using COAD.SEGURANCA.Repositorios.Base;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CAR_ID", "ASN_NUM_ASSINATURA")]
    public class CarteiraAssinaturaSRV : GenericService<CARTEIRA_ASSINATURA, CarteiraAssinaturaDTO, string>
    {
        public CarteiraAssinaturaDAO _dao = new CarteiraAssinaturaDAO();

        public CarteiraAssinaturaSRV()
        {
            this.Dao = _dao;
        }

        public bool HasCarteiraAssinatura(string CAR_ID, string ASS_NUM_ASSINATURA)
        {
           return _dao.HasCarteiraAssinatura(CAR_ID, ASS_NUM_ASSINATURA);          
        }

        public void SalvarAssinaturaSRV(IList<CarteiraAssinaturaDTO> lstCarteira)
        {
            if (lstCarteira != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(lstCarteira, "HasCarteiraAssinatura");
            }
        }

        public void SalvarCarteiraAssinatura(CarteiraAssinaturaDTO carteria)
        {
            SalvarAssinaturaSRV(new List<CarteiraAssinaturaDTO>() { carteria });
        }

        public CarteiraAssinaturaDTO CriarESalvarCarteiraAssinatura(CarteiraDTO carteira, AssinaturaDTO assi)
        {
            if (carteira != null && assi != null)
            {
                var carteiraAssinatura = new CarteiraAssinaturaDTO()
                {
                    CAR_ID = carteira.CAR_ID,
                    ASN_NUM_ASSINATURA = assi.ASN_NUM_ASSINATURA,
                    UEN_ID = 2
                };

                SalvarCarteiraAssinatura(carteiraAssinatura);
                return carteiraAssinatura;
            }

            return null;
        }

        public IList<CarteiraAssinaturaDTO> FindByCarId(string CAR_ID)
        {
            return _dao.FindByCarId(CAR_ID);
        }

        public void PreencherCarteiraAssinatura(CarteiraDTO carteira)
        {
            if (carteira != null && !string.IsNullOrWhiteSpace(carteira.CAR_ID))
            {
                carteira.CARTEIRA_ASSINATURA = FindByCarId(carteira.CAR_ID);
            }
        }

        public void RemoverCarteiraAssinaturaFromAssinatura(AssinaturaDTO assinatura, string CAR_ID)
        {
            if (assinatura != null && !string.IsNullOrWhiteSpace(CAR_ID))
            {
                var carteiraAssinatura = FindById(CAR_ID, assinatura.ASN_NUM_ASSINATURA);

                if (carteiraAssinatura != null)
                {
                    Delete(carteiraAssinatura);
                }
            }
        }
        
        public IList<CarteiraAssinaturaDTO> FindByAssinaturaERegiao(string ASN_NUM_ASSINATURA, int? RG_ID)
        {
            return _dao.FindByAssinaturaERegiao(ASN_NUM_ASSINATURA, RG_ID);
        }

        public IList<CarteiraAssinaturaDTO> BuscarClientes(string _car_id, string _asn_num_assinatura)
        {
            return _dao.BuscarClientes(_car_id, _asn_num_assinatura);
        }
        public void TransfAssinatura(string _car_id, CarteiraAssinaturaDTO _carteira)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (!String.IsNullOrWhiteSpace(_carteira.CAR_ID))
                        this.Delete(_carteira);

                    _carteira.CAR_ID = _car_id;
                    this.Save(_carteira);

                    new ClienteSRV().GravarHistorico(3, _carteira.CLI_ID, _carteira.ASN_NUM_ASSINATURA, "Transferência de carteira", 19);

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
    }
}
