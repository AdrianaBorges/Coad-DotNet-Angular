using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class DatasFaturamentoDAO : AbstractGenericDao<datas_fat, DatasFaturamentoDTO, string>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }

        public DatasFaturamentoDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }

        public DatasFaturamentoDTO GetUltimoPeriodoFaturamento()
        {
            DateTime? dataAtual = DateTime.Now;
            var dataFat = GetDbSet();

            var objDoCanco = (from objData in dataFat
                              let dia = objData.DATA_FAT.Substring(0, 2)
                              let mes = objData.DATA_FAT.Substring(3, 2)
                              let ano = objData.DATA_FAT.Substring(6, 4)
                              orderby 
                                  ano descending,
                                  mes descending
                              select new { data = objData, 
                                  ano = ano, 
                                  mes = mes, 
                                  dia = dia }).Take(5);


            var objMaterializados = 
                (from objData in objDoCanco.ToList()
                         select new DatasFaturamentoDTO()
                         {
                             DATA_FAT = objData.data.DATA_FAT,
                             PERIODO = objData.data.PERIODO,
                             SEMANA = objData.data.SEMANA,
                             ANO = objData.ano,
                             MES = objData.mes,
                             DIA = objData.dia
                         }).OrderByDescending(or => or.Data).
                         FirstOrDefault();

            return objMaterializados;
        }

        public ICollection<DatasFaturamentoDTO> ListarDataFaturamentoUltimos2Meses()
        {
            DateTime dataAtual = DateTime.Now;
            int mesAnterior = dataAtual.Month - 1;
            int anoConsulta = dataAtual.Year;
            int anoCorrente = 0;
            int mesCorrente = 0;

            if (mesAnterior <= 0)
            {
                mesCorrente = 1;
                mesAnterior = 12;
                anoCorrente = anoConsulta--;
            }
            
            var dataFat = GetDbSet();

            var objDoBanco = (from objData in dataFat
                                  let dia = objData.DATA_FAT.Substring(0, 2)
                                  let mes = objData.DATA_FAT.Substring(3, 2)
                                  let ano = objData.DATA_FAT.Substring(6, 4)
                              orderby
                                  ano descending,
                                  mes descending
                              select new
                              {
                                  data = objData,
                                  ano = ano,
                                  mes = mes,
                                  dia = dia
                              }).Take(15);

            var objMaterializados =
                (from objData in objDoBanco.ToList()
                 select new DatasFaturamentoDTO()
                 {
                     DATA_FAT = objData.data.DATA_FAT,
                     PERIODO = objData.data.PERIODO,
                     SEMANA = objData.data.SEMANA,
                     ANO = objData.ano,
                     MES = objData.mes,
                     DIA = objData.dia
                 }).OrderByDescending(or => or.Data)
                 .Where(x => 
                        (
                            x.Data.Value.Year == anoConsulta && 
                            x.Data.Value.Month >= mesAnterior
                        ) 
                            || 
                        (
                            anoCorrente > 0 && 
                            (x.Data.Value.Year == anoCorrente && x.Data.Value.Month >= mesCorrente)
                        )
                    )
                 .ToList();

            return objMaterializados;
        }

        public DatasFaturamentoDTO FindByDate(DateTime? data)
        {
            var dataFat = GetDbSet();
            if (data != null) {
                DateTime dataParametro = data.Value;

                string dataStr = dataParametro.ToString("dd/MM/yyyy");
                var objDoBanco = (from objData in dataFat
                                  let dia = objData.DATA_FAT.Substring(0, 2)
                                  let mes = objData.DATA_FAT.Substring(3, 2)
                                  let ano = objData.DATA_FAT.Substring(6, 4)
                                  where
                                    objData.DATA_FAT == dataStr
                                  select new
                                  {
                                      data = objData,
                                      ano = ano,
                                      mes = mes,
                                      dia = dia
                                  });

                var objMaterializados =
                    (from objData in objDoBanco.ToList()
                     select new DatasFaturamentoDTO()
                     {
                         DATA_FAT = objData.data.DATA_FAT,
                         PERIODO = objData.data.PERIODO,
                         SEMANA = objData.data.SEMANA,
                         ANO = objData.ano,
                         MES = objData.mes,
                         DIA = objData.dia
                     }).FirstOrDefault();

                return objMaterializados;
            }

            return null;
        }
    }
}
