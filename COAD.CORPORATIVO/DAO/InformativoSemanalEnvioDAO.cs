using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class InformativoSemanalEnvioDAO : DAOAdapter<INFORMATIVO_SEMANAL_ENVIO, InformativoSemanalEnvioDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public InformativoSemanalEnvioDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public List<InformativoSemanalEnvioDTO> Buscar(string _ano, string _remessa, int _envio, int? _produto = null)
        {
            IQueryable<INFORMATIVO_SEMANAL_ENVIO> q = GetDbSet();

            q = q.Where(x => x.INF_ANO == _ano && x.INF_REMESSA == _remessa && x.INF_ENVIO == _envio);

            if (_produto != null)
                q = q.Where(x => x.INF_PRO_ID == _produto);

            return ToDTO(q).ToList();
        }

        public void GerarRemessa(string _ano, string _remessa, string _produto, int? _envio, string _usuario, DateTime? _entrega = null, bool _temMDP = false)
        {
            if (_envio == 1) // cartas
            {
                db.Database.ExecuteSqlCommand(
                                                "EXEC COADCORP.DBO.INFORMATIVO_SEMANAL_CARTAS @ano, @remessa, @produto, @usuLogin, @dtEntrega, @temMDP",
                                                new SqlParameter("@ano", _ano),
                                                new SqlParameter("@remessa", _remessa),
                                                new SqlParameter("@produto", _produto),
                                                new SqlParameter("@usuLogin", _usuario),
                                                new SqlParameter("@dtEntrega", (object)_entrega ?? DBNull.Value),
                                                new SqlParameter("@temMDP", _temMDP)
                                            );
            }
            else if (_envio == 2) // Entrega Direta (ED)
            {
                db.Database.ExecuteSqlCommand(
                                                "EXEC COADCORP.DBO.INFORMATIVO_SEMANAL_ED @ano, @remessa, @produto, @usuLogin, @dtEntrega, @temMDP",
                                                new SqlParameter("@ano", _ano),
                                                new SqlParameter("@remessa", _remessa),
                                                new SqlParameter("@produto", _produto),
                                                new SqlParameter("@usuLogin", _usuario),
                                                new SqlParameter("@dtEntrega", (object)_entrega ?? DBNull.Value),
                                                new SqlParameter("@temMDP", _temMDP)
                                            );
            }
            else
            {
                throw new Exception("Nº do Envio não informado ou incorreto! Use apenas 1 ou 2.");
            }
        }

        public IList<InformativoSemanalEnvioDTO> RemessaAenviar(string ano, string remessa, int envio)
        {       
            return ToDTO(db.INFORMATIVO_SEMANAL_ENVIO.
                            Where(x => x.INF_ANO == ano && x.INF_REMESSA == remessa && x.INF_ENVIO == envio).
                            OrderBy(x => x.INF_ANO).
                            ThenBy(x => x.INF_REMESSA).
                            ThenBy(x => x.INF_ENVIO).
                            ThenBy(x => x.INF_PRO_ID).
                            ThenBy(x => x.INF_TIPO)
                        );
        }

        public IList<InformativoSemanalEnvioDTO> GerarArquivo(string ano, string remessa, int envio, int produto)
        {
            return ToDTO(db.INFORMATIVO_SEMANAL_ENVIO.
                            Where(x => x.INF_ANO == ano && x.INF_REMESSA == remessa && x.INF_ENVIO == envio && x.INF_PRO_ID == produto).
                            OrderBy(x => x.INF_ANO).
                            ThenBy(x => x.INF_REMESSA).
                            ThenBy(x => x.INF_ENVIO).
                            ThenBy(x => x.INF_PRO_ID).
                            ThenBy(x => x.INF_TIPO)
                        );
        }

        public void ConfirmarPostagens(string _ano, string _remessa, bool _MDP)
        {
            db.Database.ExecuteSqlCommand(
                                            "EXEC COADCORP.DBO.INFORMATIVO_SEMANAL_CONFIRMAR_ENVIOS @ano, @remessa, @MDP",
                                            new SqlParameter("@ano", _ano),
                                            new SqlParameter("@remessa", _remessa),
                                            new SqlParameter("@MDP", _MDP)
                                        );
        }
    }
}
