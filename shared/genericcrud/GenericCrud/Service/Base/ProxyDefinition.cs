using Coad.GenericCrud.Config;
using COAD.CORPORATIVO.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service.Base
{
    public class ProxyTools<TDTO, TProxy>
    {
        private string profileName { get; set; }
        private MapperEngineWrapper _mapper {get; set;}

        public ProxyTools(string profileName)
        {
            this.profileName = profileName;
            this._mapper = MapperEngineFactory.criarMapperEngine(profileName);
        }

        public TProxy ConvertToProxy(TDTO obj)
        {
            var resultObj = _mapper.Convert<TDTO, TProxy>(obj);
            return resultObj;
        }

        public ICollection<TProxy> ConvertToProxy(IEnumerable<TDTO> lstObj)
        {
            var resultObj = _mapper.Convert<IEnumerable<TDTO>, HashSet<TProxy>>(lstObj);
            return resultObj;
        }
    }
}
