﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Object
{
    public class Error : Interface.IError
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
