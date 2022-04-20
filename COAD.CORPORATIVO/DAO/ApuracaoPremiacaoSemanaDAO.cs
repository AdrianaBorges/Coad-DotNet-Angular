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
    public class ApuracaoPremiacaoSemanaCustomDTO
    {
        public int REP_ID { get; set; }
        public string REP_NOME { get; set; }
        public Nullable<decimal> APU_VLR_01META { get; set; }
        public Nullable<decimal> APU_VLR_01VLR { get; set; }
        public Nullable<decimal> APU_VLR_01PREMIO { get; set; }

        public Nullable<decimal> APU_VLR_02META { get; set; }
        public Nullable<decimal> APU_VLR_02VLR { get; set; }
        public Nullable<decimal> APU_VLR_02PREMIO { get; set; }

        public Nullable<decimal> APU_VLR_03META { get; set; }
        public Nullable<decimal> APU_VLR_03VLR { get; set; }
        public Nullable<decimal> APU_VLR_03PREMIO { get; set; }

        public Nullable<decimal> APU_VLR_04META { get; set; }
        public Nullable<decimal> APU_VLR_04VLR { get; set; }
        public Nullable<decimal> APU_VLR_04PREMIO { get; set; }

        public Nullable<decimal> APU_VLR_TOTPREMIO { get; set; }


    }

    public class ApuracaoPremiacaoSemanaDAO : DAOAdapter<APURACAO_PREMIACAO_SEMANA, ApuracaoPremiacaoSemanaDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ApuracaoPremiacaoSemanaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public List<ApuracaoPremiacaoSemanaCustomDTO> ListarApuracaoVendas(int _mes, int _ano, int _repid)
        {
            var query = (from a in db.APURACAO_PREMIACAO_SEMANA
                         join r in db.REPRESENTANTE on a.REP_ID equals r.REP_ID
                         where (a.APU_MES == _mes)
                            && (a.APU_ANO == _ano)
                            && (r.UEN_ID == 2)
                            && ((_repid == 0) || (_repid > 0 && _repid == a.REP_ID))
                         select new ApuracaoPremiacaoSemanaDTO()
                         {
                             REP_ID = a.REP_ID,
                             REP_NOME = r.REP_NOME,
                             APU_MES = a.APU_MES,
                             APU_ANO = a.APU_ANO,
                             APU_SEMANA_FAT = a.APU_SEMANA_FAT,
                             APU_VLR_PREMIO = a.APU_VLR_PREMIO,
                             APU_QTDE_CONTRATOS = a.APU_QTDE_CONTRATOS,
                             APU_VLR_CONTRATOS = a.APU_VLR_CONTRATOS,
                             APU_VLR_META = 0,
                         }).OrderByDescending(X => X.REP_ID).ThenBy(x => x.APU_SEMANA_FAT);

            var _lista = new List<ApuracaoPremiacaoSemanaCustomDTO>();
            var _ite = new ApuracaoPremiacaoSemanaCustomDTO();

            var rep_id_ant = 0;

            foreach (var _item in query)
            {
                if (rep_id_ant == 0)
                {
                    _ite = new ApuracaoPremiacaoSemanaCustomDTO();
                    _ite.REP_ID =  _item.REP_ID;
                    _ite.REP_NOME = _item.REP_NOME;
                    rep_id_ant = _item.REP_ID;

                }

                if (_item.REP_ID != rep_id_ant)
                {
                    _ite.APU_VLR_TOTPREMIO = ((_ite.APU_VLR_01PREMIO == null) ? 0 : _ite.APU_VLR_01PREMIO) +
                                             ((_ite.APU_VLR_02PREMIO == null) ? 0 : _ite.APU_VLR_02PREMIO) +
                                             ((_ite.APU_VLR_03PREMIO == null) ? 0 : _ite.APU_VLR_03PREMIO) +
                                             ((_ite.APU_VLR_04PREMIO == null) ? 0 : _ite.APU_VLR_04PREMIO);
                    _lista.Add(_ite);
                    _ite = new ApuracaoPremiacaoSemanaCustomDTO();
                    _ite.REP_ID = _item.REP_ID;
                    _ite.REP_NOME = _item.REP_NOME;
                    rep_id_ant = _item.REP_ID;
                }

                switch (_item.APU_SEMANA_FAT)
                {
                    case 1:
                        _ite.APU_VLR_01META = (_item.APU_VLR_META == null) ? 0 : _item.APU_VLR_META;
                        _ite.APU_VLR_01VLR = (_item.APU_VLR_CONTRATOS == null) ? 0 : _item.APU_VLR_CONTRATOS;
                        _ite.APU_VLR_01PREMIO = (_item.APU_VLR_PREMIO == null) ? 0 : _item.APU_VLR_PREMIO;
                        break;
                    case 2:
                        _ite.APU_VLR_02META = (_item.APU_VLR_META == null) ? 0 : _item.APU_VLR_META;
                        _ite.APU_VLR_02VLR = (_item.APU_VLR_CONTRATOS == null) ? 0 : _item.APU_VLR_CONTRATOS;
                        _ite.APU_VLR_02PREMIO = (_item.APU_VLR_PREMIO == null) ? 0 : _item.APU_VLR_PREMIO;
                        break;
                    case 3:
                        _ite.APU_VLR_03META = (_item.APU_VLR_META == null) ? 0 : _item.APU_VLR_META;
                        _ite.APU_VLR_03VLR = (_item.APU_VLR_CONTRATOS == null) ? 0 : _item.APU_VLR_CONTRATOS;
                        _ite.APU_VLR_03PREMIO = (_item.APU_VLR_PREMIO == null) ? 0 : _item.APU_VLR_PREMIO;

                        break;
                    case 4:
                        _ite.APU_VLR_04META = (_item.APU_VLR_META == null) ? 0 : _item.APU_VLR_META;
                        _ite.APU_VLR_04VLR = (_item.APU_VLR_CONTRATOS == null) ? 0 : _item.APU_VLR_CONTRATOS;
                        _ite.APU_VLR_04PREMIO = (_item.APU_VLR_PREMIO == null) ? 0 : _item.APU_VLR_PREMIO;
                        break;
                }

            }

            _ite.APU_VLR_TOTPREMIO = ((_ite.APU_VLR_01PREMIO == null) ? 0 : _ite.APU_VLR_01PREMIO) +
                                     ((_ite.APU_VLR_02PREMIO == null) ? 0 : _ite.APU_VLR_02PREMIO) +
                                     ((_ite.APU_VLR_03PREMIO == null) ? 0 : _ite.APU_VLR_03PREMIO) +
                                     ((_ite.APU_VLR_04PREMIO == null) ? 0 : _ite.APU_VLR_04PREMIO);

            _lista.Add(_ite);

            return _lista;

        }
    }
}
