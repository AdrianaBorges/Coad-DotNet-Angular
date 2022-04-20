using COAD.SEGURANCA.Model.Custons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Service
{
    public class PoolEmailSRV
    {
        private IList<SmtpEmailDTO> DataSource { get; set; }

        public PoolEmailSRV()
        {
            DataSource = new List<SmtpEmailDTO>() {

                new SmtpEmailDTO(){
                        
                    CodigoSmtpEmail = 1,
                    CodigoTipo = 1,
                    Usuario = "coadcorp@coad.com.br",
                    Senha = "C04dc0rp@"
                },
                new SmtpEmailDTO(){

                    CodigoSmtpEmail = 2,
                    CodigoTipo = 2,
                    Usuario = "posvenda@coad.com.br",
                    Senha = "P05V3nd@"
                },
                new SmtpEmailDTO(){

                    CodigoSmtpEmail = 3,
                    CodigoTipo = 3,
                    Usuario = "faturamento@coad.com.br",
                    Senha = "FATURAMENTO"
                },
                new SmtpEmailDTO(){

                    CodigoSmtpEmail = 4,
                    CodigoTipo = 4,
                    Usuario = "backup@coad.com.br",
                    Senha = "Br@z1l!2014"
                },
                new SmtpEmailDTO(){

                    CodigoSmtpEmail = 5,
                    CodigoTipo = 5,
                    Usuario = "licencas@coad.com.br",
                    Senha = ""
                },
                new SmtpEmailDTO(){

                    CodigoSmtpEmail = 6,
                    CodigoTipo = 6,
                    Usuario = "envio@coad.com.br",
                    Senha = "Q1w2e3r4"
                },
            };
        }

        public SmtpEmailDTO RetornarContaSMTP(int? codSMTP)
        {
            return DataSource.Where(x => x.CodigoSmtpEmail == codSMTP).FirstOrDefault();
        }
    }
}
