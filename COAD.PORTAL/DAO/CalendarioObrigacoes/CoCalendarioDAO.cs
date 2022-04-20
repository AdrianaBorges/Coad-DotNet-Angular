using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.CalendarioObrigacoes
{
    public class CoCalendarioDAO : AbstractGenericDao<CO_CALENDARIO, CoCalendarioDTO, string>
    {
        public CoCalendarioDAO()
        {
            SetProfileName("portal");
        }

        public Pagina<CoCalendarioDTO> Calendario(DateTime data, string opcao = "", string municipio = "", string estado = "", string area = "", int pagina = 1, int nLinha = 7)
        {

            //IQueryable<CO_CALENDARIO> query = GetDbSet().Take(numeroDeTuplas).OrderByDescending(x => x.AtualizadoEmDT);
            IQueryable<CO_CALENDARIO> query = null;

            long idestado = 0;
            long idmunicipio = 0;
            long idOpcao = 0;

            if (long.TryParse(opcao, out idOpcao))
            {
                if (idOpcao == 99)
                {
                    query = GetDbSet().Where(x => x.DTReferencia.Year == data.Year && x.DTReferencia.Month == data.Month && x.DTReferencia.Day == data.Day && x.CO_OBRIGACOES.TXT_ABRANGENCIA.Equals("Federal") && x.CO_OBRIGACOES.COD_AREA.Contains(area));
                }
                else if (idOpcao == 98)
                {
                    if (long.TryParse(municipio, out idmunicipio))
                        query = GetDbSet().Where(x => x.DTReferencia.Year == data.Year && x.DTReferencia.Month == data.Month && x.DTReferencia.Day == data.Day && x.CO_OBRIGACOES.NUM_UF == idmunicipio /*&& x.CO_OBRIGACOES.COD_AREA.Contains(area)*/);
                    else
                        query = GetDbSet().Where(x => x.DTReferencia.Year == data.Year && x.DTReferencia.Month == data.Month && x.DTReferencia.Day == data.Day && x.CO_OBRIGACOES.TXT_ABRANGENCIA.Equals("Municipal") /*&& x.CO_OBRIGACOES.COD_AREA.Contains(area)*/);
                }
                else if (idOpcao == 97)
                {
                    if (long.TryParse(estado, out idestado))
                        query = GetDbSet().Where(x => x.DTReferencia.Year == data.Year && x.DTReferencia.Month == data.Month && x.DTReferencia.Day == data.Day && x.CO_OBRIGACOES.TXT_ABRANGENCIA.Equals("Estadual") && x.CO_OBRIGACOES.NUM_UF == idestado && x.CO_OBRIGACOES.COD_AREA.Contains(area));
                    else
                        query = GetDbSet().Where(x => x.DTReferencia.Year == data.Year && x.DTReferencia.Month == data.Month && x.DTReferencia.Day == data.Day && x.CO_OBRIGACOES.TXT_ABRANGENCIA.Equals("Estadual") && x.CO_OBRIGACOES.COD_AREA.Contains(area));
                }
                else
                {
                    query = GetDbSet().Where(x => x.DTReferencia.Year == data.Year && x.DTReferencia.Month == data.Month && x.DTReferencia.Day == data.Day && x.CO_OBRIGACOES.COD_AREA.Contains(area));
                }
            }
            else
                query = GetDbSet().Where(x => x.DTReferencia.Year == data.Year && x.DTReferencia.Month == data.Month && x.DTReferencia.Day == data.Day && x.CO_OBRIGACOES.COD_AREA.Contains(area));


            var repPagina = ToDTOPage(query, pagina, nLinha);
            return repPagina;
        }

        public Pagina<CoCalendarioDTO> CalendarioAtual(int pagina = 1, int nLinha = 7)
        {
            IQueryable<CO_CALENDARIO> query = GetDbSet().Where(x => x.DTReferencia.Year == DateTime.Now.Year && x.DTReferencia.Month == DateTime.Now.Month && x.DTReferencia.Day == DateTime.Now.Day);
            
            var repPagina = ToDTOPage(query, pagina, nLinha);

            return repPagina;
        }

        public IList<CoCalendarioDTO> CalendarioAtual()
        {
            IList<CO_CALENDARIO> query = GetDbSet().Where(x => x.DTReferencia.Year == DateTime.Now.Year && x.DTReferencia.Month == DateTime.Now.Month && x.DTReferencia.Day == DateTime.Now.Day).ToList();

            //var repPagina = ToDTOPage(query, pagina, nLinha);

            return ToDTO(query);
        }

        public IList<CoCalendarioDTO> CaledarioTop()
        {
            IList<CO_CALENDARIO> query = GetDbSet().Where(x => x.DTReferencia.Year == DateTime.Now.Year && x.DTReferencia.Month == DateTime.Now.Month && x.DTReferencia.Day == DateTime.Now.Day).Take(10).OrderBy(x => x.CO_OBRIGACOES.TXT_TITULO).ToList();

            return ToDTO(query);
        }

        public CoCalendarioDTO CalendarioPorIDObrigacao(int id)
        {
            CO_CALENDARIO query = GetDbSet().Where(x => x.NUM_OBRIGACAO == id).FirstOrDefault();

            return ToDTO(query);
        }

        public IList<CoCalendarioDTO> CalendarioPorData(DateTime data)
        {
            IQueryable<CO_CALENDARIO> query = GetDbSet().Where(x => x.DTReferencia.Year == data.Year && x.DTReferencia.Month == data.Month && x.DTReferencia.Day == data.Day);

            return ToDTO(query);
        }
    }
}
