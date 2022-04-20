
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("UF_SIGLA")]
    public class UFSRV : ServiceAdapter<UF, UFDTO>
    {
        private UFDAO _dao = new UFDAO();

        [ServiceProperty("UF", Name = "municipio", PropertyName = "MUNICIPIO")]
        private MunicipioSRV _municipioSRV = new MunicipioSRV();

        public UFSRV()
        {
            this.SetDao(_dao);
        }

        public IList<UFDTO> BuscarSomenteUF()
        {
            return _dao.BuscarSomenteUF();
        }
  
        public IList<UFDTO> BuscarNaoCadastrada(int _prod_id, string _ura_id)
        {
            return _dao.BuscarNaoCadastrada(_prod_id, _ura_id);
        }
        /// <summary>
        /// Lista todas as ufs pertencentes ao carteiramento
        /// </summary>
        /// <returns></returns>
        public IList<UFDTO> GetUfsNoCarteiramento()
        {
            return _dao.GetUfsNoCarteiramento();
        }

        /// <summary>
        /// Lista todas as ufs pertencentes ao representante
        /// </summary>
        /// <returns></returns>
        public IList<UFDTO> GetUfsNoRepresentante()
        {
            return _dao.GetUfsNoRepresentante();
        }

        public IList<UFDTO> ListUfsPorRegiao(int? rgId)
        {
            var lstUfs = _dao.ListUfsPorRegiao(rgId);

            GetAssociations(lstUfs, "municipio");
            return lstUfs;
        }

        public void DesanexarUfs(RegiaoDTO regiao)
        {
            var rgId = regiao.RG_ID;
            var regiaoDoBanco = new RegiaoSRV().FindByIdFullLoaded(rgId, false, true);
            var lstUfParaDesanexar = GetMissinList(regiao, regiaoDoBanco, "UF");

            foreach (var uf in lstUfParaDesanexar)
            {
                uf.RG_ID = null;
            }

            SaveOrUpdateAll(lstUfParaDesanexar);
        }

 
        public void SalvarOuDessanexarUF(RegiaoDTO regiao)
        {
            if (regiao != null)
            {
                DesanexarUfs(regiao);

                var lstUfs = regiao.UF;
                CheckAndAssignKeyFromParentToChildsList(regiao, lstUfs, "RG_ID");
                SaveOrUpdateAll(lstUfs);
            }
        }

        public string BuscarCodUFPorDescricao(string descricaoEstado)
        {
            return _dao.BuscarCodUFPorDescricao(descricaoEstado);
        }       
    }
}
