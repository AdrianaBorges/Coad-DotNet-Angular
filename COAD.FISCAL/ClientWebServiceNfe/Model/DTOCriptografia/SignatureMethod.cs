﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTOCriptografia
{
    [Serializable]
    public class SignatureMethod : AlgorithmBase
    {
        public Reference Reference { get; set; }
    }
}