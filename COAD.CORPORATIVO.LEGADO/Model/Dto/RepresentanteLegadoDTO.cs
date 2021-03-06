using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{   [Mapping(typeof(representante))]
    public class RepresentanteLegadoDTO
    {
        public string COD_REPR { get; set; }
        public string OPER { get; set; }
        public string NOME { get; set; }
        public string REGIAO { get; set; }
        public string PROX_ENCART { get; set; }
        public string AREA { get; set; }
        public string PERIODO { get; set; }
        public string ANO { get; set; }
        public Nullable<double> PERC_OBJ { get; set; }
        public Nullable<double> PERC_PERF { get; set; }
        public Nullable<double> LIQUIDEZ { get; set; }
        public Nullable<double> VLR_OBJETIVO { get; set; }
        public Nullable<double> VLR_COTA { get; set; }
        public string FUNCAO { get; set; }
        public string MATRICULA { get; set; }
        public string MATGERSUP { get; set; }
        public string MATGERNAC { get; set; }
        public string MATEX { get; set; }
        public string MATASSVDA { get; set; }
        public Nullable<double> CRED_CAMPANHA { get; set; }
        public Nullable<double> DEB_CAMPANHA { get; set; }
        public Nullable<double> OUTROS_CREDITOS { get; set; }
        public Nullable<double> OUTROS_DEBITOS { get; set; }
        public Nullable<double> PERC_REA { get; set; }
        public Nullable<double> INCOB { get; set; }
        public Nullable<double> INCOB_FAT { get; set; }
        public Nullable<double> INCOB_DEB { get; set; }
        public Nullable<int> COTA_RECIBO { get; set; }
        public Nullable<double> VLR_REALIZADO { get; set; }
        public Nullable<double> VLR_PERF_ANT { get; set; }
        public Nullable<double> VLR_PERF_ATU { get; set; }
        public string AREAN { get; set; }
        public Nullable<int> SETOR { get; set; }
        public Nullable<int> SORTEIO { get; set; }
        public int AUTOID { get; set; }
        public string TEL_1 { get; set; }
        public string TEL_2 { get; set; }
        public string TEL_3 { get; set; }
    }
}
