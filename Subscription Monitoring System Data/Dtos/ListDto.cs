using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Dtos
{
    public class ListDto
    {
        public object Pagination { get; set; } = null!;
        public object Data { get; set; } = null!;
    }
}
