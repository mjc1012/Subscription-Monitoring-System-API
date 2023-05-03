﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Dtos
{
    public class ResponseDto
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Value { get; set; } = null!;
    }
}
