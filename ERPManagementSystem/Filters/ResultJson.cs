﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Filters
{
    public class ResultJson
    {
        public dynamic Data { get; set; }
        public int HttpCode { get; set; }
        public string Error { get; set; }
    }
}
