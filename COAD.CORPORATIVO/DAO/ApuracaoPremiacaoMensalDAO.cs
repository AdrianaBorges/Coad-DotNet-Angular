using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ApuracaoPremiacaoMensalDAO : DAOAdapter<APURACAO_PREMIACAO_MENSAL, ApuracaoPremiacaoMensalDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ApuracaoPremiacaoMensalDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public List<ApuracaoPremiacaoMensalDTO> ListarApuracaoVendas(int _mes, int _ano, int _repid)
        {
            int _tcoid = 1;

            var query = (from a in db.APURACAO_PREMIACAO_MENSAL
                         join r in db.REPRESENTANTE on a.REP_ID equals r.REP_ID
                         where (a.APU_MES == _mes)
                            && (a.APU_ANO == _ano)
                            && (r.UEN_ID == 2)
                            && ((_repid == 0) || (_repid > 0 && _repid == a.REP_ID))
                         select new ApuracaoPremiacaoMensalDTO()
                         {
                             REP_ID = a.REP_ID,
                             REP_NOME = r.REP_NOME,
                             APU_MES = a.APU_MES,
                             APU_ANO = a.APU_ANO,
                             APU_VLR_RENOVACAO = a.APU_VLR_RENOVACAO,
                             APU_VLR_VENDA_NOVA = a.APU_VLR_VENDA_NOVA,
                             APU_VLR_PRODUTOS = a.APU_VLR_PRODUTOS,
                             APU_VLR_AVISTA = a.APU_VLR_AVISTA,
                             APU_VLR_4PARCELAS = a.APU_VLR_4PARCELAS,
                             APU_PERC_4PARCELAS = a.APU_PERC_4PARCELAS,
                             APU_VLR_TOTAL = a.APU_VLR_TOTAL,
                             RME_VLR_META = db.REPRESENTANTE_META.Where(x => x.REP_ID == a.REP_ID && x.TCO_ID == _tcoid && x.RME_MES == a.APU_MES && x.RME_ANO == a.APU_ANO).FirstOrDefault().RME_VLR_META,
                             SER_VLR_PREMIO = db.REPRESENTANTE_META.Where(x => x.REP_ID == a.REP_ID && 
                                                                               x.TCO_ID == _tcoid && 
                                                                               x.RME_MES == a.APU_MES && 
                                                                               x.RME_ANO == a.APU_ANO &&
                                                                               x.RME_VLR_META <= a.APU_VLR_TOTAL).FirstOrDefault().SER_PREMIO_MAX 

                         }).OrderByDescending(X => X.APU_VLR_TOTAL).ToList();

            return query;

        }
    }

}
