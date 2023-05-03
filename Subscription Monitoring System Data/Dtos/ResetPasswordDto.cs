using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Dtos
{
    public class ResetPasswordDto
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string ResetPasswordToken { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;

    }
}
