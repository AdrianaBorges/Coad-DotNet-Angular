﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService
{
    public class RenovacaoMalaResult : WebServiceResult
    {
        [DataMember]
        public List<string> Result;
    }
}
