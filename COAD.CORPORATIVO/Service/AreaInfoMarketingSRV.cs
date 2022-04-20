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
using COAD.CORPORATIVO.Model.Comparators;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("MKT_CLI_ID", "AREA_ID")]
    public class AreaInfoMarketingSRV : GenericService<AREA_INFO_MARKETING, AreaInfoMarketingDTO, int>
    {
        public AreaInfoMarketingDAO _dao = new AreaInfoMarketingDAO();

        public AreaInfoMarketingSRV()
        {
            this.Dao = _dao;
        }

        public AreaInfoMarketingDTO FindAreaInfoMarketing(int MKT_CLI_ID, int AREA_ID)
        {
           return _dao.FindAreaInfoMarketing(MKT_CLI_ID, AREA_ID);
        }

        public bool HasAreaInfoMarketing(int MKT_CLI_ID, int AREA_ID)
        {
            return _dao.HasAreaInfoMarketing(MKT_CLI_ID, AREA_ID);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="regiaoTabPreco"></param>
        /// <param name="excecoes"></param>
        public void ExcluirAreaInfoMarketing(InfoMarketingDTO infoMarketing)
        {

            var MKT_CLI_ID = (int)infoMarketing.MKT_CLI_ID;

            if (infoMarketing.AREA_INFO_MARKETING != null)
            {

                var excecoes = infoMarketing.AREA_INFO_MARKETING;
                var infoMarketingOriginal = new InfoMarketingSRV().FindByCliIdFullLoaded(MKT_CLI_ID);

                if (infoMarketingOriginal != null)
                {                
                    var areaInfoMarketing = infoMarketingOriginal.AREA_INFO_MARKETING;
                    _processarChaves(MKT_CLI_ID, areaInfoMarketing);

                    if (areaInfoMarketing != null && excecoes != null)
                    {
                        var regiaoTabelaPrecoPraExcluir = areaInfoMarketing.Except(excecoes, new AreaInfoMarketingDTOComparator());

                        if (regiaoTabelaPrecoPraExcluir != null && regiaoTabelaPrecoPraExcluir.Count() > 0)
                        {
                            DeleteAll(regiaoTabelaPrecoPraExcluir, "MKT_CLI_ID", "AREA_ID");
                        }

                    }

                }
            }
        }

        public void SalvarAreaInfoMarketing(IEnumerable<AreaInfoMarketingDTO> lstAreaInfoMarketing, int MKT_CLI_ID) 
        {
            if (lstAreaInfoMarketing != null)
            {
                IList<AreaInfoMarketingDTO> lstAtualizar = new List<AreaInfoMarketingDTO>();
                IList<AreaInfoMarketingDTO> lstSalvar = new List<AreaInfoMarketingDTO>();

                foreach (var infoAreaMarketing in lstAreaInfoMarketing)
                {
                    // var MKT_CLI_ID = infoAreaMarketing.MKT_CLI_ID;
                    infoAreaMarketing.MKT_CLI_ID = MKT_CLI_ID;

                    var AREA_ID = infoAreaMarketing.AREA_ID;

                    if (HasAreaInfoMarketing((int) MKT_CLI_ID, (int) AREA_ID))
                    {
                        lstAtualizar.Add(infoAreaMarketing);
                    }
                    else
                    {
                        lstSalvar.Add(infoAreaMarketing);
                    }
                }

                MergeAll(lstAtualizar, true, "MKT_CLI_ID", "AREA_ID");
                SaveAll(lstSalvar);
            }
        }

        private void _processarChaves(int? MTK_CLI_ID, IEnumerable<AreaInfoMarketingDTO> lstAreaInfoMarketing)
        {
            if (lstAreaInfoMarketing != null)
            {   
                foreach (var areaInfoMarketing in lstAreaInfoMarketing)
                {
                    if (areaInfoMarketing.MKT_CLI_ID == null)
                    {
                        areaInfoMarketing.MKT_CLI_ID = MTK_CLI_ID;
                    }

                    if (areaInfoMarketing.AREA_ID == null)
                    {
                        areaInfoMarketing.AREA_ID = areaInfoMarketing.AREAS.AREA_ID;
                    }
                }
            }
        }

        public IList<AreaInfoMarketingDTO> ProcessarEConcatenarAreaInfoMarketing(InfoMarketingDTO marketingDTO, IList<AreaInfoMarketingDTO> lstAcumulada)
        {
            if(marketingDTO != null && marketingDTO.AREA_INFO_MARKETING != null && lstAcumulada != null)
            {
                var lstAreaInfoMkt = marketingDTO.AREA_INFO_MARKETING.ToList();
                _processarChaves(marketingDTO.MKT_CLI_ID, lstAreaInfoMkt);

                lstAcumulada = lstAcumulada.Concat(lstAreaInfoMkt).ToList();

            }

            return lstAcumulada;

        }

        public void ProcessarExclusaoEAtualizacaoAreaInfoMarketing(InfoMarketingDTO marketingDTO)
        {
            if (marketingDTO.MKT_CLI_ID != null)
            {
                _processarChaves(marketingDTO.MKT_CLI_ID, marketingDTO.AREA_INFO_MARKETING);
                ExcluirAreaInfoMarketing(marketingDTO);
                SalvarAreaInfoMarketing(marketingDTO.AREA_INFO_MARKETING, (int) marketingDTO.MKT_CLI_ID);
            }
        }

    }
}
