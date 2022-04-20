using COAD.AGENDADOR.Config;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SCHEDULER.Installers
{
    public class CustomInstaller : ServiceInstaller
    {
        [ComVisible(false)]
        [DefaultValue("")]
        [ServiceProcessDescription("ServiceInstallerDescription")]
        public new string Description
        {
            get
            {
                var desc = base.Description;
                if (desc != null)
                {
                    var objDesc = AgendadorConfig.ObterDados();
                    if(objDesc != null)
                        return objDesc.NomeDescricao + desc;
                }
                return desc;

            }
            set
            {
                base.Description = value;
            }
        }


        [DefaultValue("")]
        [ServiceProcessDescription("ServiceInstallerDisplayName")]
        public new string DisplayName
        {
            get
            {
                var desc = base.DisplayName;
                if (desc != null)
                {
                    var objDesc = AgendadorConfig.ObterDados();
                    if (objDesc != null)
                        return objDesc.NomeDescricao + desc;
                }
                return desc;

            }
            set
            {
                base.DisplayName = value;
            }
        }

        [DefaultValue("")]
        [ServiceProcessDescription("ServiceInstallerServiceName")]
        [TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new string ServiceName {
            get
            {
                var objDados = AgendadorConfig.ObterDados();
                if (objDados != null)
                {
                    return objDados.NomeService;
                }

                return "Scheduler";
            }

            set
            {
                base.ServiceName = value;
            }        
        }

    }
}
