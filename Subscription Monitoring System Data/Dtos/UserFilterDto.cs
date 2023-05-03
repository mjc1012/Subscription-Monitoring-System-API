using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Dtos
{
    public class UserFilterDto
    {
        public int Page { get; set; }
        public string SortBy { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;    
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
